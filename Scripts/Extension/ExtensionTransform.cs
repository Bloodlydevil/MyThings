using System;
using UnityEngine;


namespace MyThings.Extension
{

    public static class ExtensionTransform
    {
        /// <summary>
        /// This Is An Extention Method Which Is Used To Find A T In The Imidiate Childs Of The Transform
        /// </summary>
        /// <param name="transform">the transfrom where to find</param>
        /// <typeparam name="t">The type To Find</typeparam>
        /// <returns>The Object</returns>
        public static t GetInChildImidiate<t>(this Transform transform) where t : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.TryGetComponent(out t ok))
                    return ok;
            }
            return null;
        }
        /// <summary>
        /// This Is An Extention Method Which Is Used To Find A T In The Imidiate Childs Of The Transform
        /// </summary>
        /// <typeparam name="t">The type To Find</typeparam>
        /// <param name="transform">the transfrom where to find</param>
        /// <param name="obj">the Object</param>
        /// <returns>If Found</returns>
        public static bool TryInChildImidiate<t>(this Transform transform,out t obj) where t : Component
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.TryGetComponent(out obj))
                {
                    return true;
                }
            }
            obj = null;
            return false;
        }
        /// <summary>
        /// Get The Component from the current object before going for its child
        /// </summary>
        /// <typeparam name="t">the type</typeparam>
        /// <param name="transform">the transform</param>
        /// <returns>the componenet</returns>
        public static t GetInSelfOrChildImidiate<t>(this Transform transform) where t : Component
        {
            if(transform.TryGetComponent(out t obj))
                return obj;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.TryGetComponent(out t ok))
                    return ok;
            }
            return null;
        }
        /// <summary>
        /// Get The Component from the current object before going for its child
        /// </summary>
        /// <typeparam name="t">the type</typeparam>
        /// <param name="transform">the transform</param>
        /// <param name="obj">the componenet</param>
        /// <returns>If Got Or Not</returns>
        public static bool TryInSelfOrChildImidiate<t>(this Transform transform, out t obj) where t : Component
        {
            if (transform.TryGetComponent(out obj))
            {
                return true;
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.TryGetComponent(out obj))
                {
                    return true;
                }
            }
            obj = null;
            return false;
        }
        /// <summary>
        /// Find the child recusively
        /// </summary>
        /// <param name="transform">the object in which to find</param>
        /// <param name="Name">the name of the obj</param>
        /// <param name="DepthLimit">the depth to test for</param>
        /// <returns>found object</returns>
        public static Transform  RecursiveFind(this Transform transform,string Name,int DepthLimit=5)
        {
            if (DepthLimit < 0)
                return null;
            foreach(Transform child in transform)
            {
                if (child.name == Name)
                    { return child; }
                else
                {
                    var trans= RecursiveFind(transform, child.name, DepthLimit-1);
                    if (trans != null)
                    { return trans; }
                }
            }
            return null;
        }
        /// <summary>
        /// A Helper Funtion To Run Any Code For Each Child Of The Transform
        /// </summary>
        /// <param name="transform">The Transform To Run On</param>
        /// <param name="action">The Function To Perform</param>
        public static void ForEachChild(this Transform transform, Action<Transform> action)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                action(transform.GetChild(i));
            }
        }
    }
}