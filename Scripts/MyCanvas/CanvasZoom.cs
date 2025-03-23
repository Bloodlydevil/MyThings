using MyThings.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyThings.MyCanvas
{
    /// <summary>
    /// A Class To Allow Zoom functionality In the Work Area
    /// </summary>
    public class CanvasZoom : MonoBehaviour, IScrollHandler
    {
        [SerializeField] private RectTransform m_ZoomTarget;

        [Min(0), SerializeField] private float m_minScale;

        [Min(0), SerializeField] private float m_maxScale;

        [Tooltip("Minimum of 6 as The base Tick Of Scroll Is Set To 6 Currently")]
        [Min(6), SerializeField] private float m_Sensitivity;

        private Jug m_CurrentScroll;

        private Vector2 m_CenterLocation;

        public event Action OnScrolling;

        [field: SerializeField] public float CanvasScaleFactor { get; set; }

        private void Awake()
        {
            m_CurrentScroll = new Jug(m_Sensitivity, 0, m_Sensitivity);
        }
        private void OnValidate()
        {
            transform.localScale = Vector3.one * m_maxScale;
        }
        public void SetUp(Vector2 ScreenSize)
        {
            m_CenterLocation = ScreenSize / 2 + m_ZoomTarget.anchoredPosition * CanvasScaleFactor;
        }
        public void OnScroll(PointerEventData eventData)
        {

            float change = m_CurrentScroll.ChangeOnWaterAdd(eventData.scrollDelta.y);
            if (change != 0)
            {
                Vector3 updatedPoint = eventData.position - m_CenterLocation;
                float oldScale = transform.localScale.y;
                transform.localScale = Vector3.one * Mathf.Lerp(m_minScale, m_maxScale, (m_CurrentScroll / m_Sensitivity));
                float ScaleFactor = transform.localScale.y / oldScale;

                // When Scalling We Want To Make Sure The Center Remains same
                transform.localPosition *= ScaleFactor;// When Scalling We Want
                                                       // When Scalling Up Or Down We Want To Make The center the Mouse


                transform.localPosition += updatedPoint * (1 - ScaleFactor) / CanvasScaleFactor;
                OnScrolling?.Invoke();
            }
        }
    }
}