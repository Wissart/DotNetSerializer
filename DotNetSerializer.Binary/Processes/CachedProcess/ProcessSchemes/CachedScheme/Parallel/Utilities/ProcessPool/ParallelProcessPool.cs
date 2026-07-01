using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool
{
    internal static class ParallelProcessPool
    {

        private static readonly Worker[] _workers;
        private static readonly Dictionary<int, ParallelProcess> _processByID;
        private static readonly ConcurrentQueue<int> _deleteQueue;

        private static int _processIDCounter;
        private static ParallelProcess _headProcess;

        private static volatile bool _isRunning;
        private static readonly object _stopStartLock = new object();
        private static CancellationTokenSource _cts;
        private static readonly ManualResetEventSlim _cleanSignal;
        private static Task _cleanTask;


        internal const int STOP_DELAY_MS = 1000;
        internal const int WORK_SPIN_WAIT_COUNT = 10;

        public const int PARALLELABLE_MIN_DATA_SIZE = 64 * 1024;

        static ParallelProcessPool()
        {
            _workers = new Worker[Environment.ProcessorCount / 2];
            for (int i = 0; i < _workers.Length; i++)
                _workers[i] = new Worker(i);

            _processByID = new Dictionary<int, ParallelProcess>();
            _deleteQueue = new ConcurrentQueue<int>();
            _cleanSignal = new ManualResetEventSlim(false);
        }

        public static ParallelProcess GetProcess() => _headProcess;

        public static void AddTask(int processID, Action task) => _processByID[processID].Tasks.Enqueue(task);


        public static int CreateProcess()
        {
            lock (_stopStartLock)
            {   
                var processID = ++_processIDCounter;

                if (_headProcess == null)
                {
                    _processByID[processID] = new ParallelProcess(processID);
                    _headProcess = _processByID[processID];
                }
                else
                    _processByID[processID] = new ParallelProcess(processID, _headProcess);

                if (!_isRunning)
                    Start();

                return processID;
            }
        }

        public static void WaitProcess(int processID)
        {
            var process = _processByID[processID];

            while (!process.Tasks.IsEmpty)
            {
                if (process.Tasks.TryDequeue(out var task))
                    task.Invoke();
            }

            process.Complete();

            process.WaitFullyComplete();

            RequestCleanup(processID);
        }

        private static void RequestCleanup(int processID)
        {
            _deleteQueue.Enqueue(processID);
            _cleanSignal.Set();
        }

        private static void Start()
        {
            _isRunning = true;
            _cts = new CancellationTokenSource();
            var token = _cts.Token;

            foreach(var worker in _workers)
            {
                worker.Start(_cts.Token);
            }

            StartCleanup(token);
        }

        public static void Stop()
        {
            _isRunning = false;
            _cts?.Cancel();

            _headProcess = null;
            _processByID.Clear();
            _processIDCounter = 0;
        }

        private static void StartCleanup(CancellationToken token)
        {
            _cleanTask = Task.Run(() =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        _cleanSignal.Wait(token);

                        bool cleanupFinished = false;
                        lock (_stopStartLock)
                        {
                            while (_deleteQueue.TryDequeue(out int processID))
                            {
                                var process = _processByID[processID];
                                if (process == _headProcess)
                                {
                                    if (process == process.Next)
                                        _headProcess = null;
                                    else
                                        _headProcess = process.Next;
                                }

                                process.Detach();
                                _processByID.Remove(processID);

                                if (_headProcess == null)
                                    cleanupFinished = true;
                            }
                        }

                        if (!cleanupFinished)
                        {
                            if (_deleteQueue.IsEmpty)
                            {
                                _cleanSignal.Reset();
                            }

                            continue;
                        }

                        _cleanSignal.Wait(STOP_DELAY_MS, token);

                        lock (_stopStartLock)
                        {
                            if (!_deleteQueue.IsEmpty || _headProcess != null)
                                continue;

                            Stop();
                            break;
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    // Normal case — token canceled, exit without exception
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Cleanup error: {ex.Message}");
                }
            });
        }
    }
}
