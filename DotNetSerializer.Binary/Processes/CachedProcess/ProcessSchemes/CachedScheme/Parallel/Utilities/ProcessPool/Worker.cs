using System;
using System.Threading;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool
{
    internal class Worker
    {

        private const int WORK_DELAY_MS = 200;
        private readonly int _id;

        private Thread _thread;

        public Worker(int id)
        {
            _id = id;
        }


        public void Start(CancellationToken token) => CreateThread(token).Start();
        private Thread CreateThread(CancellationToken token)
        {
            _thread = new Thread(() => WorkLoop(token))
            {
                IsBackground = true,
                Name = $"Worker №{_id}",
                Priority = ThreadPriority.AboveNormal,
            };

            return _thread;
        }

        private void WorkLoop(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    var process = ParallelProcessPool.GetProcess();

                    while(process != null)
                    {
                        if (process.IsCompleted)
                        {
                            if (process == process.Next) break;

                            process = process.Next;
                            continue;
                        }

                        process.CheckIn();
                        while (process.Tasks.TryDequeue(out var task))
                        {
                            task.Invoke();
                        }
                        process.CheckOut();

                        process = process.Next;
                    }

                    Thread.Sleep(WORK_DELAY_MS);
                }
            }
            catch (OperationCanceledException)
            {
                // Normal case — token canceled, exit without exception
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Worker error: {ex.Message}");
            }
        }


    }
}
