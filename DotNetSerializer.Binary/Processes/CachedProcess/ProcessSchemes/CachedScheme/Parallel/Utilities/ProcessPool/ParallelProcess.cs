using System;
using System.Collections.Concurrent;
using System.Threading;

namespace DotNetSerializer.Binary.Processes.CachedProcess.ProcessSchemes.CachedScheme.Parallel.Utilities.ProcessPool
{
    internal class ParallelProcess
    {
        private readonly ManualResetEventSlim _completeSignal = new ManualResetEventSlim(false);
        public void WaitFullyComplete() => _completeSignal.Wait();

        public int ID { get; }


        private volatile int _countWorker;

        private volatile bool _isCompleted;

        public ConcurrentQueue<Action> Tasks { get; }

        public ParallelProcess Prev { get; private set; }
        public ParallelProcess Next { get; private set; }

        public bool IsCompleted => _isCompleted;

        public bool IsFullyCompleted => (_countWorker == 0 && _isCompleted) && Tasks.IsEmpty;

        // Constructor for Head Process
        public ParallelProcess(int id)
        {
            ID = id;
            Tasks = new ConcurrentQueue<Action>();

            Prev = this;
            Next = this;
        }

        public ParallelProcess(int id, ParallelProcess headProcess) 
            : this(id)
        {
            Prev = headProcess.Prev;
            Prev.Next = this;
            headProcess.Prev = this;
            Next = headProcess;
        }

        public void Complete()
        {
            _isCompleted = true;
            if (IsFullyCompleted)
                _completeSignal.Set();
        }

        public void CheckIn() => Interlocked.Increment(ref _countWorker);
        public void CheckOut()
        {
            Interlocked.Decrement(ref _countWorker);
            if (IsFullyCompleted)
                _completeSignal.Set();
        }

        public void Detach()
        {
            Prev.Next = Next;
            Next.Prev = Prev;

            Prev = null;
            Next = null;
        }
    }
}
