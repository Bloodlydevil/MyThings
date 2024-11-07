using MyThings.Cashe;
using MyThings.Extension;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyThings.Window.Utills
{
    /// <summary>
    /// Allow Draging of the UI While ensuring it drags from the point of mouse and not stick to the 
    /// mouse postion
    /// </summary>
    public class ProperDrag :MonoBehaviour, IDragHandler, IBeginDragHandler
    {
        public enum Mode
        {
            WorldView,
            ScreenOverlay
        }
        private List<RectTransform> m_NoDragZone=new List<RectTransform>();// The No Drag Zones
        private Vector3 m_Correction;//the correction to add to drag position
        private Camera m_Camera;// camera used
        private bool m_NoDrag;// If The Begain Is In No Drag Area Then Do Not Allow Drag
        private Mode m_Mode;

        private void Start()
        {
            Camera_C.Instance.OnCameraChanged += SetCamera;
            Camera_C.OnCameraFirstAcquired += SetCamera;
        }

        private void OnDestroy()
        {
            Camera_C.Instance.IfTrue(i => i.OnCameraChanged -= SetCamera);
            Camera_C.OnCameraFirstAcquired -= SetCamera;
        }

        private void SetCamera(Camera camera)
        {
            m_Camera = camera;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (m_NoDrag) return;
            switch(m_Mode)
            {
                case Mode.WorldView:
                    transform.position = m_Correction + m_Camera.ScreenToWorldPoint(eventData.position.ToVector3(10));
                    break;
                case Mode.ScreenOverlay:
                    transform.position = m_Correction + eventData.position.ToVector3(0);
                    break;
            }
            
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            m_NoDrag = false;
            Vector3 pos= eventData.position.ToVector3(10);
            if(m_Mode == Mode.WorldView)
            {
                pos= m_Camera.ScreenToWorldPoint(pos);
            }
            for (int i = 0; i < m_NoDragZone.Count; ++i)
            {
                if (m_NoDragZone[i].rect.Contains(pos-m_NoDragZone[i].position))
                {
                    m_NoDrag = true;
                    return;
                }
            }
            m_Correction = transform.position - pos;
        }

        /// <summary>
        /// Add rigion where we hould not allow drag if mouse is there
        /// </summary>
        /// <param name="rectTransform">The areas to leave</param>
        public void AddNoDragRegion(params RectTransform[] rectTransform)
        {
            foreach (RectTransform rect in rectTransform)
                m_NoDragZone.Add(rect);
        }

        /// <summary>
        /// Attach this dragable class to the gameobject
        /// </summary>
        /// <param name="objec">the gameobject to attach to</param>
        public static ProperDrag Attach(GameObject objec,Mode mode)
        {
            var temp = objec.AddComponent<ProperDrag>();
            temp.m_Mode = mode;
            return temp;
        }
    }
}
