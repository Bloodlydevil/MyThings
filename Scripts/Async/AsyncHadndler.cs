using MyThings.ExtendableClass;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace MyThings.Async
{
    /// <summary>
    /// Runs async tasks one-by-one from a queue and dispatches callbacks on Unity main thread.
    /// </summary>
    public class AsyncHandler : Singleton_C<AsyncHandler>
    {
        private readonly ConcurrentQueue<QueuedTask> taskQueue = new ConcurrentQueue<QueuedTask>();
        private readonly Queue<Action> mainThreadCallbacks = new Queue<Action>();
        private readonly Queue<(Action<Exception>, Exception)> mainThreadErrors = new Queue<(Action<Exception>, Exception)>();

        private bool performingTask = false;

        private abstract class QueuedTask
        {
            public abstract Task ExecuteAsync(AsyncHandler handler);
        }

        private class QueuedTaskVoid : QueuedTask
        {
            private readonly Func<Task> taskFunc;
            private readonly Action onComplete;
            private readonly Action<Exception> onError;

            public QueuedTaskVoid(Func<Task> taskFunc, Action onComplete, Action<Exception> onError)
            {
                this.taskFunc = taskFunc;
                this.onComplete = onComplete;
                this.onError = onError;
            }

            public override async Task ExecuteAsync(AsyncHandler handler)
            {
                try
                {
                    await taskFunc();
                    if (onComplete != null)
                        handler.mainThreadCallbacks.Enqueue(onComplete);
                }
                catch (Exception ex)
                {
                    handler.mainThreadErrors.Enqueue((onError, ex));
                }
            }
        }

        private class QueuedTaskWithResult<T> : QueuedTask
        {
            private readonly Func<Task<T>> taskFunc;
            private readonly Action<T> onResult;
            private readonly Action<Exception> onError;

            public QueuedTaskWithResult(Func<Task<T>> taskFunc, Action<T> onResult, Action<Exception> onError)
            {
                this.taskFunc = taskFunc;
                this.onResult = onResult;
                this.onError = onError;
            }

            public override async Task ExecuteAsync(AsyncHandler handler)
            {
                try
                {
                    var result = await taskFunc();
                    if (onResult != null)
                        handler.mainThreadCallbacks.Enqueue(() => onResult(result));
                }
                catch (Exception ex)
                {
                    handler.mainThreadErrors.Enqueue((onError, ex));
                }
            }
        }

        /// <summary>
        /// Enqueue a background task with completion callback.
        /// </summary>
        public void RunTask(Func<Task> taskFunc, Action onComplete = null, Action<Exception> onError = null)
        {
            taskQueue.Enqueue(new QueuedTaskVoid(taskFunc, onComplete, onError));
        }

        /// <summary>
        /// Enqueue a background task with result callback.
        /// </summary>
        public void RunTask<T>(Func<Task<T>> taskFunc, Action<T> onResult, Action<Exception> onError = null)
        {
            taskQueue.Enqueue(new QueuedTaskWithResult<T>(taskFunc, onResult, onError));
        }

        private async void FixedUpdate()
        {
            if (performingTask || taskQueue.IsEmpty)
                return;

            if (taskQueue.TryDequeue(out var nextTask))
            {
                performingTask = true;
                await nextTask.ExecuteAsync(this);
                performingTask = false;
            }
        }

        private void LateUpdate()
        {
            while (mainThreadCallbacks.Count > 0)
            {
                mainThreadCallbacks.Dequeue()?.Invoke();
            }

            while (mainThreadErrors.Count > 0)
            {
                var (handler, exception) = mainThreadErrors.Dequeue();
                handler?.Invoke(exception);
            }
        }
    }
}
