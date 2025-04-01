using MyThings.Extension;
using MyThings.Job_System;
using UnityEngine;

namespace MyThings.MyCanvas
{
    /// <summary>
    /// A Scroller To Scroll The Background Object Based on Foreground
    /// </summary>
    public class CanvasScroller : MonoBehaviour
    {
        [SerializeField] private CanvasDragger m_BackgroundDragger;
        [SerializeField] private CanvasDragger m_ForegroundDragger;
        [SerializeField] private RectTransform m_NoScrollZone;
        [SerializeField] private bool m_AwakeSetUp;

        private IJob m_Drag;

        private Vector2 m_MousePos;

        private void Awake()
        {
            m_Drag = JobSystem.CreateJob(Drag, true);
            if (!m_AwakeSetUp)
                return;

            if (m_ForegroundDragger != null)
            {
                m_ForegroundDragger.OnDragging += ForegroundDragging;
                m_ForegroundDragger.OnDragOver += ForegroundDraggOver;
            }
        }
        private void ForegroundDraggOver()
        {
            m_ForegroundDragger.ReSetCumulative();
            m_Drag?.Stop();
        }

        private void Drag(float _)
        {
            var Correction=m_BackgroundDragger.AutoDrag(m_MousePos);

            m_ForegroundDragger.ChangeLocationBy(Correction);
        }
        private void ForegroundDragging(Vector2 MousePos)
        {
            if (MousePos.IsInsideBox(m_NoScrollZone.anchoredPosition, m_NoScrollZone.rect.size))
            {
                m_Drag.Stop();
                m_BackgroundDragger.StopDrag();
            }
            else
            {
                m_MousePos = MousePos;
                m_Drag.Start();
            }
        }
        public void SetData(RectTransform NoScrollZone)
        {
            m_NoScrollZone = NoScrollZone;
        }
        public void SetForeGround(CanvasDragger ForeGround)
        {
            if (m_ForegroundDragger != null)
            {
                m_ForegroundDragger.OnDragging -= ForegroundDragging;
                m_ForegroundDragger.OnDragOver -= ForegroundDraggOver;
            }
            m_ForegroundDragger = ForeGround;
            m_ForegroundDragger.OnDragging += ForegroundDragging;
            m_ForegroundDragger.OnDragOver += ForegroundDraggOver;
        }
        public void SetBackgroundData( CanvasDragger background)
        {
            if(m_BackgroundDragger != null) m_BackgroundDragger.StopDrag();

            m_Drag?.Stop();
            m_BackgroundDragger = background;
        }
    }
}