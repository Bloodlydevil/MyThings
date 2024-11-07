using UnityEngine;

namespace MyThings.Extension
{
    /// <summary>
    /// A Class Which Deals With Gameobject Extention
    /// </summary>
    public static class ExtensionGameObject
    {
        /// <summary>
        /// Get Component In The Current Gameobject And If Not Found Then In Imidiate Child
        /// </summary>
        /// <typeparam name="t">The Componenet To Find</typeparam>
        /// <param name="gameObject">The Game Object In Which To Find</param>
        /// <returns>The Component</returns>
        public static t GetInSelfOrChildImidiate<t>(this GameObject gameObject) where t : Component
        {
            return gameObject.transform.GetInSelfOrChildImidiate<t>();
        }
        /// <summary>
        /// Get Component In The Current Gameobject And If Not Found Then In Imidiate Child
        /// </summary>
        /// <typeparam name="t">The Componenet To Find</typeparam>
        /// <param name="gameObject">The Game Object In Which To Find</param>
        /// <param name="obj">The Component</param>
        /// <returns>If Found Or Not</returns>
        public static bool TryInSelfOrChildImidiate<t>(this GameObject gameObject, out t obj) where t : Component
        {
            gameObject.transform.TryInSelfOrChildImidiate(out obj); return true;
        }
    }
}