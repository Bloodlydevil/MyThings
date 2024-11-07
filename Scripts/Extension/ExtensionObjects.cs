using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Extension
{
    public static class ExtensionObjects
    {

        /// <summary>
        /// An Iterator For The objects
        /// </summary>
        /// <typeparam name="type">The Type Of THe Object</typeparam>
        /// <param name="objects">The Objects To Iterate</param>
        /// <param name="action">The Action To Perform On All the Objects</param>
        public static void Iterator<type>(this type[] objects, Action<type> action)
        {
            foreach (var i in objects)
            {
                action(i);
            }
        }
        /// <summary>
        ///  Print all the values of the IEnumerator
        /// </summary>
        /// <typeparam name="type">The IEnumerator Type</typeparam>
        /// <param name="objects">The IEnumerator</param>
        /// <returns>The Input</returns>
        public static IEnumerable<type> Print<type>(this IEnumerable<type> objects)
        {
            foreach(var i in objects)
            {
                Debug.Log(i);
            }
            return objects;
        }
    }
}
