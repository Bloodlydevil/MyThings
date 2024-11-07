using MyThings.ExtendableClass;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyThings.Scene
{
    /// <summary>
    /// The Class To Deal With Scene Loading
    /// </summary>
    public class SceneLoader : Singleton_C<SceneLoader>
    {

        /// <summary>
        /// Event Called On Scene Load (Imidiately Called When No Async Scene Is Loaded)(Gives The Target Scene)
        /// </summary>
        public event Action<int> OnSceneLoad;

        /// <summary>
        /// the Scenes To Load
        /// </summary>
        private Queue<Scene> _scenes = new Queue<Scene>();
        /// <summary>
        /// To Calculate the Wait
        /// </summary>
        float _time = 0;
        /// <summary>
        /// The Operation Required To Get Current Progress
        /// </summary>
        private AsyncOperation LoadingAsyncOperation;

        private void Update()
        {
            if (_scenes.Count!=0)
            {
                _time += Time.unscaledDeltaTime;
                if (_time > _scenes.Peek().Scene_WaitTime)
                {
                    var Scene = _scenes.Dequeue();
                    _time = 0;
                    if (Scene.Scene_Async)
                    {
                        StartCoroutine(LoadSceneAsync(Scene.Scene_Id));
                    }
                    else
                    {
                        SceneManager.LoadScene(Scene.Scene_Id);
                        OnSceneLoad?.Invoke(Scene.Scene_Id);
                    }
                }
            }
        }

        #region Private

        /// <summary>
        /// Add A Scene To The Queue
        /// </summary>
        /// <param name="scene">The Scene</param>
        private void Add(Scene scene)
        {
            _scenes.Enqueue(scene);
        }
        /// <summary>
        /// Async Call For Loadings
        /// </summary>
        /// <param name="Scene">The Scene To Load</param>
        /// <returns></returns>
        private IEnumerator LoadSceneAsync(int Scene)
        {
            LoadingAsyncOperation = SceneManager.LoadSceneAsync(Scene);
            while (!LoadingAsyncOperation.isDone)
            {
                yield return null;
            }
            LoadingAsyncOperation = null;
            OnSceneLoad?.Invoke(Scene);
        }

        #endregion

        #region Public

        /// <summary>
        /// Load The Scene
        /// </summary>
        /// <param name="sceneId">The Scene Id</param>
        /// <param name="Async">The Load Call Mode</param>
        /// <param name="LoadWait">The Wait Time Before Calling it</param>
        public void Load(int sceneId, bool Async = true, float LoadWait = 0)
        {
            Add(new(sceneId, Async, LoadWait));
        }
        /// <summary>
        /// Load the Scene but First Call The Loading Screen
        /// </summary>
        /// <param name="sceneId">The Scene Id</param>
        /// <param name="Loading_Screen">The Loading Scene Id (If Not Given then Settings.LoadingScreen Will be Used)</param>
        /// <param name="Loading_Async">Mode Of Loading Screen (Async Or Not)</param>
        /// <param name="Loading_Wait">wait Before Call Loading</param>
        /// <param name="Async">Mode Of Target Sceen</param>
        /// <param name="LoadWait">Wait Before Target Scene</param>
        public void Load_LoadingScreen(int sceneId, int Loading_Screen = -1, bool Loading_Async = true, int Loading_Wait = 0, bool Async = true, float LoadWait = 0)
        {
            Load(Loading_Screen == -1 ? Settings.LoadingScreen : Loading_Screen, Loading_Async, Loading_Wait);
            Load(sceneId, Async, LoadWait);
        }
        /// <summary>
        /// Get THe Current Loading Progress
        /// </summary>
        /// <returns>The Progress(0-1)</returns>
        public static float GetLoadingProgress()
        {
            if (_instance.LoadingAsyncOperation != null)
                return Functions.Convert(_instance.LoadingAsyncOperation.progress, 0, 0.9f, 0, 1);
            return 0;
        }

        #endregion

    }

    /// <summary>
    /// The Scene Data To Load
    /// </summary>
    internal struct Scene
    {
        /// <summary>
        /// The Scene ID
        /// </summary>
        public int Scene_Id;
        /// <summary>
        /// The Wait Time Before Calling Scene
        /// </summary>
        public float Scene_WaitTime;
        /// <summary>
        /// If To Load It Async Or Not
        /// </summary>
        public bool Scene_Async;
        /// <summary>
        /// The Constructor To Create Scene
        /// </summary>
        /// <param name="sceneId">The Scene ID</param>
        /// <param name="Async">The Call Mode</param>
        /// <param name="LoadWait">The Load Wait</param>
        public Scene(int sceneId, bool Async, float LoadWait)
        {
            Scene_Id = sceneId;
            Scene_WaitTime = LoadWait;
            Scene_Async = Async;
        }
    }
    public class SceneName
    {
        public enum Scene { }
    }
}