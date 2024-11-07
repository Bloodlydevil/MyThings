using MyThings.Events;
using MyThings.ExtendableClass;
using MyThings.Timer;
using System;
using UnityEngine;

namespace MyThings.Cashe
{
    /// <summary>
    /// A Class To Get The Main Camera and tell it to change it to allow global change
    /// </summary>
    public class Camera_C : Singleton_C<Camera_C>
    {

        private Camera m_camera;
        /// <summary>
        /// The Current Camera
        /// </summary>
        public Camera Camera=>m_camera;


        /// <summary>
        /// Event on camera changed
        /// </summary>
        public event Action<Camera> OnCameraChanged;

        private static EventOnce<Action<Camera>> m_up=new EventOnce<Action<Camera>>();
        /// <summary>
        /// Call this event only first time camera is set up(to ease some class)
        /// </summary>
        public static event Action<Camera> OnCameraFirstAcquired
        {
            add => m_up += value;
            remove => m_up -= value;
        }

        protected override void Awake()
        {
            base.Awake();
            m_camera=Camera.main;
            m_up.Call(i => i?.Invoke(m_camera), i => i(m_camera));
        }

        public void ReCacheCamera(Camera camera)
        {
            if (m_camera!=camera)
            {
                m_camera = camera;
                OnCameraChanged?.Invoke(m_camera);
            }
        }
        public void ReCacheCamera()
        {
            var temp = Camera.main;
            if (m_camera != temp)
            {
                m_camera = temp;
                OnCameraChanged?.Invoke(m_camera);
            }
        }
    }
}