using MyThings.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MyThings.MyCanvas
{
    /// <summary>
    /// A Class To Allow Zoom Feature On Any Canvas
    /// </summary>
    public class CanvasZoom : MonoBehaviour, IScrollHandler
    {
        [Tooltip("The Target On Which Zoom Must Be Done")]
        [field:SerializeField] public RectTransform ZoomTarget { get; private set; }

        [Tooltip("The Canvas Scale Factor To Be Used")]
        [field: SerializeField] public float CanvasScaleFactor { get; set; }



        [Tooltip("This event Is Called When Any Zoom Occures ")]
        public event Action OnZoom;



        [Tooltip("The Minimum Zoom It Can Go")]
        [Min(0), SerializeField] private float m_minScale;
        [Tooltip("The Maximum Zoom It Can Go")]
        [Min(0), SerializeField] private float m_maxScale;

        [Tooltip("Minimum of 6 as The base Tick Of Scroll Is Set To 6 Currently")]
        [Min(6), SerializeField] private float m_Sensitivity;

        [Tooltip("The Current Value Of Zoom IS Stored")]
        private Jug m_Zoom;

        [Tooltip("The Center Of The Screen")]
        private Vector2 m_CenterLocation;




        private void Awake()
        {
            m_Zoom = new Jug(m_Sensitivity, 0, m_Sensitivity);
        }
        private void OnValidate()
        {
            transform.localScale = Vector3.one * m_maxScale;
        }
        /// <summary>
        /// Cal This Function To Set Up The Zoom Class
        /// </summary>
        /// <param name="ScreenSize">The Size Of The Screen </param>
        public void SetUp(Vector2 ScreenSize)
        {
            m_CenterLocation = ScreenSize / 2 + ZoomTarget.anchoredPosition * CanvasScaleFactor;
        }
        public void OnScroll(PointerEventData eventData)
        {

            float change = m_Zoom.ChangeOnWaterAdd(eventData.scrollDelta.y);
            if (change != 0)
            {
                Vector3 updatedPoint = eventData.position - m_CenterLocation;
                float oldScale = transform.localScale.y;
                transform.localScale = Vector3.one * Mathf.Lerp(m_minScale, m_maxScale, (m_Zoom / m_Sensitivity));
                float ScaleFactor = transform.localScale.y / oldScale;

                // When Scalling We Want To Make Sure The Center Remains same
                transform.localPosition *= ScaleFactor;// When Scalling We Want
                                                       // When Scalling Up Or Down We Want To Make The center the Mouse
                transform.localPosition += updatedPoint * (1 - ScaleFactor) / CanvasScaleFactor;
                OnZoom?.Invoke();
            }
        }
        /// <summary>
        /// Get The Max Global Scale Which This Transform Can Get
        /// </summary>
        /// <returns>The Scale(use Only X)</returns>
        public float GetMaxGlobalScale()
        {
            return (transform.lossyScale.x / transform.localScale.x) * m_maxScale;
        }
    }
}