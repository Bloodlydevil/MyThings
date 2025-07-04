using System;
using System.Threading.Tasks;
using MyThings.Async;

namespace MyThings.SaveSystem
{
    /// <summary>
    /// Save and load data using background threads and return results on Unity's main thread.
    /// </summary>
    public static class SaverAndLoaderThreaded
    {
        /// <summary>
        /// Save the data using a background thread.
        /// </summary>
        public static void SaveDataThreaded(this object data, string path, SaverAndLoader.SaveType type,
                                            Action onComplete = null, Action<Exception> onError = null)
        {
            path = SaverAndLoader.GetThreadSafePath(path);
            var saveAction = SaverAndLoader.GetSaveAsyncDataAction(data, path, type);

            AsyncHandler.Instance.RunTask(
                taskFunc: () => Task.Run(saveAction),
                onComplete: onComplete,
                onError: onError
            );
        }

        /// <summary>
        /// Save using a custom action in the background.
        /// </summary>
        public static void SaveDataThreaded(this Action task, Action onComplete = null, Action<Exception> onError = null)
        {
            AsyncHandler.Instance.RunTask(
                taskFunc: () => Task.Run(task),
                onComplete: onComplete,
                onError: onError
            );
        }

        /// <summary>
        /// Load the data using a background thread, then return result on the main thread.
        /// </summary>
        public static void LoadDataThreaded<T>(this string path, SaverAndLoader.SaveType type,
                                               Action<LoadedData<T>> onResult = null, Action<Exception> onError = null)
        {
            path = SaverAndLoader.GetThreadSafePath(path);

            AsyncHandler.Instance.RunTask(
                taskFunc: () => Task.Run(() => path.LoadAsyncData<T>(type)),
                onResult: onResult,
                onError: onError
            );
        }
    }
}
