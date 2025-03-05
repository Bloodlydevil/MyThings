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
    public class Scene
    {
        private const float ProgressEnd = 0.9f;

        [SerializeField]
        private int m_SceneId;

        [SerializeField]
        private string m_SceneName;

        private AsyncOperation m_AsyncOperation;

        private Coroutine m_LoadRoutine;

        public bool IsLoaded { get; private set; }
        public int SceneId => m_SceneId;
        public bool IsLoadingAsync => m_AsyncOperation != null;

        /// <summary>
        /// A Scene Creator
        /// </summary>
        /// <param name="SceneId">The Scene Id</param>
        public Scene(int SceneId)
        {
            this.m_SceneId = SceneId;
        }
        public Scene(int SceneId, string SceneName) : this(SceneId)
        {
            m_SceneName = SceneName;
        }

        #region private

        private IEnumerator Load(Action<float> CallBackProgress,Action CallBackLoaded,Action CallBackEnd)
        {
            while (m_AsyncOperation.progress < ProgressEnd)
            {
                CallBackProgress?.Invoke(m_AsyncOperation.progress);
                yield return null;
            }

            CallBackLoaded?.Invoke();

            while(!m_AsyncOperation.isDone)
            {
                CallBackProgress?.Invoke(m_AsyncOperation.progress);
                yield return null;
            }

            IsLoaded = true;
            CallBackEnd?.Invoke();

            m_LoadRoutine = null;
        }
        private IEnumerator UnLoad(AsyncOperation operation)
        {
            while(!operation.isDone)
            {
                yield return null;
            }
        }
        private void Load(Action<float> CallbackProgress, Action CallbackLoaded, Action CallbackEnd, bool Activation)
        {
            IsLoaded = false;
            m_AsyncOperation.allowSceneActivation = Activation;
            m_LoadRoutine = SceneLoader.Instance.StartCoroutine(Load(CallbackProgress, CallbackLoaded, CallbackEnd));
        }

        #endregion

        #region Public

        /// <summary>
        /// Stop The Couroutine Used (does not Stop The Scene Load)
        /// </summary>
        public void Stop(UnloadSceneOptions options)
        {

            if (m_LoadRoutine != null)
            {
                SceneLoader.Instance.StopCoroutine(m_LoadRoutine);
            }

            SceneLoader.Instance.StartCoroutine(UnLoad(SceneManager.UnloadSceneAsync(m_SceneId, options)));
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
        public UnityEngine.SceneManagement.Scene Load(LoadSceneParameters parameters)
        {
            return SceneManager.LoadScene(m_SceneId, parameters);
        }
        /// <summary>
        /// Load The Scene Asyncronously (Allow For Only Loading Of The Scene)
        /// </summary>
        /// <param name="mode">The Scene mode</param>
        /// <param name="CallbackProgress">This Function is Feed The Scene Progress Value</param>
        /// <param name="CallbackLoaded">This Function To Call When Scene Has Finished Loading (90% load finished) activation is done after it</param>
        /// <param name="CallbackEnd">This Function Is Called When Scene has finished complete loading</param>
        /// <param name="Activation">If The Scene Should Activate as soon as it is loaded</param>
        public void LoadAsync(LoadSceneMode mode, Action<float> CallbackProgress = null, Action CallbackLoaded = null, Action CallbackEnd = null, bool Activation = false)
        {  
            m_AsyncOperation = SceneManager.LoadSceneAsync(m_SceneId, mode);
            Load(CallbackProgress, CallbackLoaded, CallbackEnd, Activation);
        }
        /// <summary>
        /// Load The Scene Asyncronously (Allow For Only Loading Of The Scene)
        /// </summary>
        /// <param name="parameters">The Scene Parameters</param>
        /// <param name="CallbackProgress">This Function is Feed The Scene Progress Value</param>
        /// <param name="CallbackLoaded">This Function To Call When Scene Has Finished Loading (90% load finished) activation is done after it</param>
        /// <param name="CallbackEnd">This Function Is Called When Scene has finished complete loading</param>
        /// <param name="Activation">If The Scene Should Activate as soon as it is loaded</param>
        public void LoadAsync(LoadSceneParameters parameters, Action<float> CallbackProgress = null, Action CallbackLoaded = null, Action CallbackEnd = null, bool Activation = false)
        {
            m_AsyncOperation = SceneManager.LoadSceneAsync(m_SceneId, parameters);
            Load(CallbackProgress, CallbackLoaded, CallbackEnd, Activation);
        }
        /// <summary>
        /// Set The SCene Activation
        /// </summary>
        /// <param name="Activation"></param>
        public void SetSceneActivation(bool Activation)
        {
            if (m_AsyncOperation != null)
                m_AsyncOperation.allowSceneActivation = Activation;
        }
        #endregion

        #region Operator

        public static explicit operator int(Scene scene) => scene.m_SceneId;
        public static explicit operator string(Scene scene) => scene.m_SceneName;
        public static bool operator ==(Scene first, Scene second) => first.m_SceneId == second.m_SceneId;
        public static bool operator !=(Scene first, Scene second) => first.m_SceneId != second.m_SceneId;
        public override bool Equals(object obj)
        {
            if (obj is Scene other)
            {
                return m_SceneId == other.m_SceneId;
            }
            return false;
        }
        public override int GetHashCode() => m_SceneId.GetHashCode();

        #endregion
    }
}