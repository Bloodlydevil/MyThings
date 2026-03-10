using UnityEngine;

namespace MyThings.ExtendableClass
{
    /// <summary>
    /// A Singleton which is created if not present
    /// </summary>
    /// <typeparam name="type">The Componenet</typeparam>
    public class Singleton_C<type> : Singleton_D<type> where type : Component
    {
        public static bool m_Alive = false;

        /// <summary>
        /// Get The Instance And If Null Then Create New
        /// </summary>
        public static new type Instance 
        {
            get
            {
                if (_instance == null && !m_Alive)
                {
                    var game = new GameObject(nameof(type));
                    game.name = typeof(type).Name;
                    _instance = game.AddComponent<type>();
                    m_Alive=true;
                }
                return _instance;
            } 
        }
        protected virtual void OnDestroy()
        {
            m_Alive = false;
        }
        protected virtual void OnApplicationQuit()
        {
            m_Alive = false;
        }
    }
}