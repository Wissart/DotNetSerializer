using System;
using System.Threading;

namespace DotNetSerializer.Base.Infrastructure
{
    /// <summary>
    /// Provides a disposable scope that executes an action when disposed.
    /// </summary>
    public class DisposableScope : IDisposable
    {
        private Action _onDispose;

        /// <summary>
        /// Initializes a new scope with the specified dispose action.
        /// </summary>
        /// <param name="onDispose">Action to execute when disposed.</param>
        public DisposableScope(Action onDispose) => _onDispose = onDispose;

        /// <summary>
        /// Executes the dispose action and release resources.
        /// </summary>
        public void Dispose()
        {
            var action = Interlocked.Exchange(ref _onDispose, null);
            action?.Invoke();
        }

        /// <summary>
        /// Creates a new Disposable scope with the specified dispose action.
        /// </summary>
        /// <param name="onDispose">Action to execute when disposed.</param>
        /// <returns>A disposable scope instance.</returns>
        public static IDisposable Create(Action onDispose) => new DisposableScope(onDispose);
    }
}
