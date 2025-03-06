using MyThings.SaveSystem;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
namespace MyThings.SaveSystem
{
    /// <summary>
    /// Save And Load Data Using Thread (Does Only One Thing At a Time)
    /// </summary>
    public static class SaverAndLoaderThreaded
    {
        private class SaverAndLoaderThreadedMono : MonoBehaviour
        {
            private static SaverAndLoaderThreadedMono instance;
            public static SaverAndLoaderThreadedMono Instance
            {
                get
                {
                    if (instance == null)
                    {
                        instance = new GameObject("Saver And Loader").AddComponent<SaverAndLoaderThreadedMono>();
                    }
                    return instance;
                }
            }

            public ConcurrentQueue<(Task, Action, Action<Exception>)> SaveTasks = new ConcurrentQueue<(Task, Action, Action<Exception>)>();
            public ConcurrentQueue<(Task<Action>, Action<Exception>)> LoadTask = new ConcurrentQueue<(Task<Action>, Action<Exception>)>();

            private bool PerformingTask;
            private bool SaveTurn;
            private Queue<Action> LoadResult = new Queue<Action>();
            private Queue<(Action<Exception>, Exception)> LoadSaveError = new Queue<(Action<Exception>, Exception)>();
            private Queue<Action> Finished = new Queue<Action>();

            public void Start()
            {
                DontDestroyOnLoad(gameObject);
            }

            private async void FixedUpdate()
            {
                if (PerformingTask)
                    return;

                if (SaveTasks.Count > 0 && (SaveTurn || LoadTask.Count == 0))
                {
                    if (SaveTasks.TryDequeue(out (Task, Action, Action<Exception>) task))
                    {
                        PerformingTask = true;
                        try
                        {
                            await task.Item1;
                            Finished.Enqueue(task.Item2);
                        }
                        catch (Exception ex)
                        {
                            PerformingTask = false;
                            LoadSaveError.Enqueue((task.Item3, ex));
                        }
                        PerformingTask = false;
                    }
                }
                else if (LoadTask.Count > 0)
                {
                    if (LoadTask.TryDequeue(out (Task<Action>, Action<Exception>) task))
                    {
                        PerformingTask = true;
                        try
                        {
                            var Result = await task.Item1;
                            LoadResult.Enqueue(Result);
                        }
                        catch (Exception ex)
                        {
                            PerformingTask = false;
                            LoadSaveError.Enqueue((task.Item2, ex));
                        }
                        PerformingTask = false;
                    }
                }
                SaveTurn = !SaveTurn;
            }
            private void LateUpdate()
            {
                while (LoadResult.Count > 0)
                {
                    // could have problem with Main thread
                    LoadResult.Dequeue().Invoke();
                }
                while (LoadSaveError.Count > 0)
                {
                    var temp = LoadSaveError.Dequeue();
                    if (temp.Item1 == null)
                    {
                        Debug.LogWarning(temp.Item2);
                    }
                    else
                    {
                        temp.Item1?.Invoke(temp.Item2);
                    }
                }
                while (Finished.Count > 0)
                {
                    Finished.Dequeue()?.Invoke();
                }
            }
        }



        /// <summary>
        /// Save The Data Using Thread
        /// </summary>
        /// <param name="Data">The Data To Store</param>
        /// <param name="Path">The Path Of The Data</param>
        /// <param name="Type">The Type of Save To Use</param>
        public static void SaveDataThreaded(this object Data, string Path, SaverAndLoader.SaveType Type, Action SaveComplete = null, Action<Exception> ErrorHandler = null)
        {
            Path = SaverAndLoader.GetThreadSafePath(Path);
            Task task = Task.Run(SaverAndLoader.GetSaveAsyncDataAction(Data, Path, Type));
            SaverAndLoaderThreadedMono.Instance.SaveTasks.Enqueue((task, SaveComplete, ErrorHandler));
        }
        /// <summary>
        /// Save The Data Using Thread
        /// </summary>
        /// <param name="task">The Save Method Used</param>
        public static void SaveDataThreaded(this Action task, Action SaveComplete = null, Action<Exception> ErrorHandler = null)
        {
            SaverAndLoaderThreadedMono.Instance.SaveTasks.Enqueue((Task.Run(task), SaveComplete, ErrorHandler));
        }
        /// <summary>
        /// Load The Data Using Thread
        /// </summary>
        /// <typeparam name="T">The Type Of Object To Return</typeparam>
        /// <param name="path">The Path Of Data Stored</param>
        /// <param name="DataReciever">The Function To Feed Data After Loading</param>
        /// <param name="type">The Type Of Save Used</param>
        public static void LoadDataThreaded<T>(this string path, SaverAndLoader.SaveType type, Action<LoadedData<T>> DataReciever = null, Action<Exception> ErrorHandler = null)
        {
            path = path.GetThreadSafePath();
            Task<Action> task = Task.Run<Action>(() =>
            {
                var Data = path.LoadAsyncData<T>(type);
                return () => { DataReciever(Data); };
            });
            SaverAndLoaderThreadedMono.Instance.LoadTask.Enqueue((task, ErrorHandler));
        }
    }
}