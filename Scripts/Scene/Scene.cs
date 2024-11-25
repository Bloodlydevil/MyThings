using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyThings.Scene
{
    [Serializable]
    /// <summary>
    /// Create A Scene and Load It
    /// </summary>
    public struct Scene
    {
        [SerializeField]
        private int m_SceneId;
        /// <summary>
        /// The SceneId
        /// </summary>
        public int SceneId=>m_SceneId;
        /// <summary>
        /// The Async Operation Used To Load Data Asyncronously ( If Used Else It Will be Null )
        /// </summary>
        public AsyncOperation AsyncOperation;
        /// <summary>
        /// The Couroutine Used
        /// </summary>
        public Coroutine Routine;
        public bool IsLoaded { get; private set; }

        /// <summary>
        /// A Scene Creator
        /// </summary>
        /// <param name="SceneId">The Scene Id</param>
        public Scene(int SceneId)
        {
            this.m_SceneId = SceneId;
            AsyncOperation = null;
            Routine = null;
            IsLoaded=false;
        }

        private IEnumerator Load(Action<float> CallbackProgress,Action CallbackEnd)
        {
            while (!AsyncOperation.isDone)
            {
                CallbackProgress?.Invoke(AsyncOperation.progress);
                yield return null;
            }
            CallbackEnd?.Invoke();
            IsLoaded = true;
        }
        private IEnumerator Load(Action<float> CallbackProgress,Action CallBackLoaded, Action CallbackEnd)
        {
            bool LoadedCalled = false;
            while (!AsyncOperation.isDone)
            {
                CallbackProgress?.Invoke(AsyncOperation.progress);
                if(AsyncOperation.progress>=0.9f)
                {
                    if(!LoadedCalled)
                    {
                        LoadedCalled = true;
                        CallBackLoaded();
                    }
                }
                yield return null;
            }
            CallbackEnd?.Invoke();
            IsLoaded = true;

        }
        private IEnumerator Load( Action CallbackEnd)
        {
            while (!AsyncOperation.isDone)
            {
                yield return null;
            }
            IsLoaded = true;
            CallbackEnd?.Invoke();
        }
        #region Public
        /// <summary>
        /// Stop The Couroutine Used (does not Stop The Scene Load)
        /// </summary>
        public void StopCouroutine()
        {
            if(Routine != null)
            {
                SceneLoader.Instance.StopCoroutine(Routine);
            }
        }
        /// <summary>
        /// Load Scene 
        /// </summary>
        /// <param name="mode">The Scene Mode Used</param>
        public void Load(LoadSceneMode mode)
        {
            SceneManager.LoadScene(m_SceneId, mode);
        }
        /// <summary>
        /// Load Scene
        /// </summary>
        /// <param name="parameters">The Scene Parameters Used</param>
        public void Load(LoadSceneParameters parameters)
        {
            SceneManager.LoadScene(m_SceneId, parameters);
        }
        /// <summary>
        /// Load Scene Asyncronously In Background
        /// </summary>
        /// <param name="mode">The Scene Mode Used</param>
        /// <param name="Activation">Activate Scene As Soon As It Is Loaded</param>
        public void LoadAsync(LoadSceneMode mode,bool Activation=true)
        {
            IsLoaded = false;
            AsyncOperation = SceneManager.LoadSceneAsync(m_SceneId, mode);
            AsyncOperation.allowSceneActivation = Activation;
        }
        /// <summary>
        /// Load Scene Asyncrously In Background
        /// </summary>
        /// <param name="parameters">The Scene Parameters</param>
        /// <param name="Activation">Activate Scene As Soon As It Is Loaded</param>
        public void LoadAsync(LoadSceneParameters parameters, bool Activation=true)
        {
            IsLoaded = false;
            AsyncOperation = SceneManager.LoadSceneAsync(m_SceneId, parameters);
            AsyncOperation.allowSceneActivation = Activation;
        }
        /// <summary>
        /// Deal With the Scene Loading
        /// </summary>
        /// <param name="CallbackEnd">The CallBack To Call At End</param>
        public void WaitForLoad(Action CallbackEnd)
        {
            Routine = SceneLoader.Instance.StartCoroutine(Load(CallbackEnd));
        }
        /// <summary>
        /// Deal With the Scene Loading
        /// </summary>
        /// <param name="CallbackProgress">The Progress Of The CallBack</param>
        /// <param name="CallbackEnd">The CallBack To Call At End</param>
        public void WaitForLoad(Action<float> CallbackProgress, Action CallbackEnd)
        {
            Routine = SceneLoader.Instance.StartCoroutine(Load(CallbackProgress, CallbackEnd));
        }
        /// <summary>
        /// Deal With the Scene Loading (Only Use When Scene Activation Is Turned Off)
        /// </summary>
        /// <param name="CallbackProgress">The Progress Of The CallBack</param>
        /// <param name="CallbackEnd">The CallBack To Call At End</param>
        /// <param name="CallbackLoaded">When Scene Is Loaded Then This Will Be Called</param>
        public void WaitForLoad(Action<float> CallbackProgress,Action CallbackLoaded, Action CallbackEnd)
        {
            Routine = SceneLoader.Instance.StartCoroutine(Load(CallbackProgress, CallbackLoaded, CallbackEnd));
        }
        #endregion
    }
}