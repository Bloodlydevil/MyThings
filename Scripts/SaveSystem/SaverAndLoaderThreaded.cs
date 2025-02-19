using MyThings.SaveSystem;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
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

        public ConcurrentQueue<Task> SaveTasks = new ConcurrentQueue<Task>();
        public ConcurrentQueue<Task<Action>> LoadTask = new ConcurrentQueue<Task<Action>>();

        private bool PerformingTask;
        private bool SaveTurn;
        private Queue<Action> LoadResult = new Queue<Action>();

        public void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private async void FixedUpdate()
        {
            if (PerformingTask)
                return;
            
            if ( SaveTasks.Count > 0 && (SaveTurn || LoadTask.Count == 0 ))
            {
                if(SaveTasks.TryDequeue(out Task task))
                {
                    PerformingTask = true;
                    await task;
                    PerformingTask = false;
                }
            }
            else if (LoadTask.Count > 0)
            {
                if(LoadTask.TryDequeue(out Task<Action> task))
                {
                    PerformingTask = true;
                    var Result=await task;
                    LoadResult.Enqueue(Result);
                    PerformingTask = false;
                }
            }
            SaveTurn = !SaveTurn;
        }
        private void LateUpdate()
        {
            while(LoadResult.Count>0)
            {
                // could have problem with Main thread
                LoadResult.Dequeue().Invoke();
            }
        }
    }

    

    /// <summary>
    /// Save The Data Using Thread
    /// </summary>
    /// <param name="Data">The Data To Store</param>
    /// <param name="Path">The Path Of The Data</param>
    /// <param name="Type">The Type of Save To Use</param>
    public static void SaveDataThreaded(this object Data, string Path,SaverAndLoader.SaveType Type)
    {
        Path = SaverAndLoader.GetThreadSafePath(Path);
        Task task = Task.Run(SaverAndLoader.GetSaveAsyncDataAction(Data, Path, Type));
        SaverAndLoaderThreadedMono.Instance.SaveTasks.Enqueue(task);
    }
    /// <summary>
    /// Save The Data Using Thread
    /// </summary>
    /// <param name="task">The Save Method Used</param>
    public static void SaveDataThreaded(this Action task)
    {
        SaverAndLoaderThreadedMono.Instance.SaveTasks.Enqueue(Task.Run(task));
    }
    /// <summary>
    /// Load The Data Using Thread
    /// </summary>
    /// <typeparam name="T">The Type Of Object To Return</typeparam>
    /// <param name="path">The Path Of Data Stored</param>
    /// <param name="DataReciever">The Function To Feed Data After Loading</param>
    /// <param name="type">The Type Of Save Used</param>
    public static void LoadDataThreaded<T>(this string path,Action<LoadedData<T>> DataReciever,SaverAndLoader.SaveType type)
    {
        path = path.GetThreadSafePath();
        Task<Action> task = Task.Run<Action>(() =>
        {
            var Data = path.LoadAsyncData<T>(type);
            return () => { DataReciever(Data); };
        });
        SaverAndLoaderThreadedMono.Instance.LoadTask.Enqueue(task);
    }
    /// <summary>
    /// Delete The File
    /// </summary>
    /// <param name="path">The File Path</param>
    public static void DeleteData(this string path)
    {
        SaverAndLoader.DeleteData(path);
    }
}