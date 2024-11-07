using Unity.Netcode;
using UnityEngine;

namespace MyThings.Multiplayer.ExtendableClass
{
    /// <summary>
    /// A Singleton To Deal With Singleton In Network Programs 
    /// </summary>
    /// <typeparam name="type"></typeparam>
    public class NetworkSingleton<type> : NetworkBehaviour where type : Component
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
        public override void OnDestroy()
        {
            base.OnDestroy();
            _instance = null;
        }
    }
}