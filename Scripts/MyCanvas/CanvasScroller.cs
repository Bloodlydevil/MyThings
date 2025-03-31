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
        [SerializeField] private RectTransform NoScrollZone;

        private IJob m_Drag;

        private Vector2 m_MousePos;

        private void Start()
        {
            if (m_ForegroundDragger != null)
            {
                m_ForegroundDragger.OnDragging += ForegroundDragging;
                m_ForegroundDragger.OnDragOver += ForegroundDraggOver;
            }
            m_Drag = JobSystem.CreateJob(Drag, true);
            
        }

        private void ForegroundDraggOver()
        {
            m_ForegroundDragger.ReSetCumulative();
        }

        private void Drag(float _)
        {
            var Correction=m_BackgroundDragger.AutoDrag(m_MousePos);

            m_ForegroundDragger.ChangeLocationBy(Correction);
        }
        private void ForegroundDragging(Vector2 MousePos)
        {
            if (MousePos.IsInsideBox(NoScrollZone.anchoredPosition, NoScrollZone.rect.size))
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
        public void SetData(CanvasDragger foreground, CanvasDragger background)
        {
            if (m_ForegroundDragger != null)
            {
                m_ForegroundDragger.OnDragging -= ForegroundDragging;
                m_ForegroundDragger.OnDragOver -= ForegroundDraggOver;
            }
            
            if(m_BackgroundDragger != null) m_BackgroundDragger.StopDrag();

            m_Drag.Stop();
            m_BackgroundDragger = background;
            m_ForegroundDragger = foreground;
            m_ForegroundDragger.OnDragging += ForegroundDragging;
            m_ForegroundDragger.OnDragOver += ForegroundDraggOver;
        }
    }
}