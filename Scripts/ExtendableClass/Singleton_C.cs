using UnityEngine;

namespace MyThings.ExtendableClass
{
    /// <summary>
    /// A Singleton which is created if not present
    /// </summary>
    /// <typeparam name="type">The Componenet</typeparam>
    public class Singleton_C<type> : Singleton_D<type> where type : Component
    {
        protected static bool m_Alive = true;

        /// <summary>
        /// Get The Instance And If Null Then Create New
        /// </summary>
        public static new type Instance 
        {
            get
            {
                if (_instance == null && m_Alive)
                {
                    var game = new GameObject(nameof(type));
                    game.name = typeof(type).Name;
                    _instance = game.AddComponent<type>();
                }
                return _instance;
            } 
        }
        protected virtual void OnApplicationQuit()
        {
            m_Alive = false;
        }
    }
}