
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Pooling
{


    /// <summary>
    /// The Pools 
    /// </summary>
    /// <typeparam name="t">The Type Of The Pool</typeparam>
    public class Pool<t>
    {
        /// <summary>
        /// The Game Object These Pools Are Connected
        /// </summary>
        public GameObject _GameObject;

        /// <summary>
        /// The Parent Transform
        /// </summary>
        public Transform _transform;
        /// <summary>
        /// The Pool 
        /// </summary>
        public Queue<t> _Queue;
        /// <summary>
        /// Constructor To Create The Pool
        /// </summary>
        /// <param name="gameObject">The GameObject</param>
        /// <param name="queue">The Queue</param>
        public Pool(GameObject gameObject, Queue<t> queue,Transform transform)
        {
            _GameObject = gameObject;
            _Queue = queue;
            _transform = transform;
        }
    }
}