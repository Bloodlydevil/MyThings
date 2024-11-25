using UnityEngine;

namespace MyThings.ExtendableClass
{
    /// <summary>
    /// A Singleton which is created if not present
    /// </summary>
    /// <typeparam name="type">The Componenet</typeparam>
    public class Singleton_D<type> :Singleton<type> where type : Component
    {
        /// <summary>
        /// If You Want TO Make It Of Dont Destroy On Load
        ///<code>Default To True</code>
        /// </summary>
        protected virtual bool AddToDontDestroyOnLoad => true;
        /// <summary>
        /// Get The Instance And If Null Then Create New
        /// </summary>
        public static new type Instance 
        {
            get
            {
                return _instance;
            } 
        }
        protected override void Awake()
        {
            base.Awake();
            if (AddToDontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
        }
    }
}