using UnityEngine;

namespace MyThings.ExtendableClass
{

    [DefaultExecutionOrder(-1)]
    /// <summary>
    /// A Singleton To Deal With Singleton In Programs 
    /// </summary>
    /// <typeparam name="type">The Type Of The Extender</typeparam>
    public class Singleton<type> : MonoBehaviour where type : Component
    {
        /// <summary>
        /// The Instance Which Follows Singleton Pattern
        /// </summary>
        protected static type _instance;
        /// <summary>
        /// If The Instance Is Created Or Not
        /// </summary>
        public static bool HasInstance => _instance != null;
        /// <summary>
        /// Try To Get The Instance
        /// </summary>
        public static type TryGetInstance => HasInstance ? _instance : null;
        /// <summary>
        /// Instance Of The Class Which Follows Singleton Pattern
        /// </summary>
        public static type Instance { get => _instance; }
        /// <summary>
        /// Use Base.Awake When Overiding
        /// </summary>
        protected virtual void Awake()
        {
            if (!Application.isPlaying)
                return;
            if (_instance != null)
            {
                Debug.LogError("Atempting To Create 2 Singleton");
                Destroy(gameObject);
            }
            _instance = this as type;
        }
        /// <summary>
        /// Use Base.OnDestroy When Overiding
        /// </summary>
        protected virtual void OnDestroy()
        {
            _instance = null;
        }
    }
}