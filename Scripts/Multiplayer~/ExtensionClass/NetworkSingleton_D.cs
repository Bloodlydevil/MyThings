using MyThings.ExtendableClass;
using Unity.Netcode;
using UnityEngine;

namespace MyThings.Multiplayer.ExtendableClass
{
    /// <summary>
    /// A Singleton which is created if not present
    /// </summary>
    /// <typeparam name="type"></typeparam>
    public class NetworkSingleton_D<type> : NetworkSingleton<type> where type : Component
    {
        protected static bool Quit = false;
        /// <summary>
        /// Get The Instance And If Null Then Create New
        /// </summary>
        public static new type Instance
        {
            get
            {
                if (_instance == null && (!Quit))
                {
                    _instance = new GameObject().AddComponent<type>();
                    _instance.gameObject.AddComponent<NetworkObject>();
                }
                return _instance;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }
        protected virtual void OnApplicationQuit()
        {
            Quit = true;
            Destroy(gameObject);
        }
    }
}