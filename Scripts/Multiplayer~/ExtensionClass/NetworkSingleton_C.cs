using MyThings.ExtendableClass;
using Unity.Netcode;
using UnityEngine;

namespace MyThings.Multiplayer.ExtendableClass
{
    /// <summary>
    /// A Singleton which is created if not present
    /// </summary>
    /// <typeparam name="type"></typeparam>
    public class NetworkSingleton_C<type> : NetworkSingleton<type> where type : Component
    {
        private static bool Alive = true;
        /// <summary>
        /// If You Want TO Make It Of Dont Destroy On Load
        /// </summary>
        protected virtual bool DDOL => true;
        /// <summary>
        /// Get The Instance And If Null Then Create New
        /// </summary>
        public static new type Instance
        {
            get
            {
                if (_instance == null && Alive)
                {
                    var game = new GameObject();
                    game.name = typeof(type).Name;
                    game.AddComponent<NetworkObject>();
                    _instance = game.AddComponent<type>();
                }
                return _instance;
            }
        }
        protected override void Awake()
        {
            base.Awake();
            if (DDOL)
                DontDestroyOnLoad(gameObject);
        }
        protected virtual void OnApplicationQuit()
        {
            Alive = false;
        }
    }
}