using MyThings.Extension;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace MyThings.ExtendableClass
{

    [DefaultExecutionOrder(-1)]
    /// <summary>
    /// A Singleton To Deal With Singleton In Programs 
    /// </summary>
    /// <typeparam name="type">The Type Of The Extender</typeparam>
    public class Singleton<type> : MonoBehaviour where type : Component
    {
        protected static type _instance;
        /// <summary>
        /// If The Object Is Alive Or Not
        /// </summary>
        protected bool Alive = true;
        /// <summary>
        /// If The Instance Is Created Or Not
        /// </summary>
        public static bool HasInstance => _instance != null;
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
                Debug.LogWarning("Atempting To Create 2 Singleton");
                Alive = false;
                Destroy(gameObject);
                return;
            }
            _instance = this as type;
        }
        /// <summary>
        /// Try To Get The Instance
        /// </summary>
        /// <param name="instance">The instance</param>
        /// <returns>If instance exist</returns>
        public static bool TryGetInstance(out type instance)
        {
            instance = _instance;
            return HasInstance;
        }
    }
}