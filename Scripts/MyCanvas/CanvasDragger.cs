using MyThings.Data;
using MyThings.Extension;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MyThings.MyCanvas
{
    /// <summary>
    /// A Class To Allow Any Canvas To Be Dragged
    /// </summary>
    public class CanvasDragger : MonoBehaviour, IDragHandler , IPointerDownHandler
    {
        #region Members

        [Tooltip("The Velocity Limit for The auto Drag")]
        [SerializeField] private Jug m_Velocity = new Jug(50, 0);


        [Tooltip("Acceleration That The Auto Drag Should Follow")]
        [SerializeField] private AnimationCurve m_Acceleration;


        [Tooltip("The Time Required To Reach Max Velocity Of the auto drag")]
        [SerializeField] private Jug m_AccelerationTime = new Jug(5, 0);


        [Tooltip("Used For Correcting The Exact Position Of Dragging")]
        private Vector2 DeltaGrabPosition;

        [Tooltip("Event Is Called When Dragging")]
        public event Action OnDragging;


        [Tooltip("The Button To Use For Dragging")]
        [field: SerializeField] public PointerEventData.InputButton DragMovementButton { get; set; } = PointerEventData.InputButton.Right;

        [Tooltip("The Canvas Scale Factor Used")]
        [field: SerializeField] public float CanvasScaleFactor { get; set; } = 1;

        [Tooltip("The Function Which Adjust The Canvas When Dragging ")]
        public Func<Vector2> CanvasAdjester { get; set; }

        [Tooltip("The Screen Center ")]
        [field: SerializeField] public Vector2 ScreenCenter { get; set; }

        [Tooltip("The Canvas To Drag")]
        [field: SerializeField] public RectTransform Draggable { get; private set; }

        #endregion

        #region Events

        public void OnPointerDown(PointerEventData eventData)
        {
            DeltaGrabPosition = Draggable.anchoredPosition * CanvasScaleFactor - eventData.position + ScreenCenter;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == DragMovementButton)
            {

                Draggable.anchoredPosition = (eventData.position - ScreenCenter + DeltaGrabPosition) / CanvasScaleFactor;

                CanvasAdjester?.Invoke();

                OnDragging?.Invoke();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Auto Dragging Of The Canvas Using The Mouse Position
        /// </summary>
        /// <param name="MousePoint">The Mouse Position Relative To The Screen Center</param>
        /// <returns>Adjustement done While Dragging In The Direction</returns>
        public Vector2 AutoDrag(Vector3 MousePoint)
        {
            m_Velocity += m_Acceleration.Evaluate(m_AccelerationTime.NormalizedLevel);

            m_AccelerationTime += Time.deltaTime;

            Vector3 DragValue = MousePoint.normalized * m_Velocity;

            Draggable.localPosition -= DragValue;

            OnDragging?.Invoke();
            // dragging Done

            // Since It Is Manual Dragging So Return The Amount Adjested
            if (CanvasAdjester != null)
            {
                Vector2 Adjust = CanvasAdjester();

                return new Vector2(Adjust.x == 0 ? DragValue.x : 0, Adjust.y == 0 ? DragValue.y : 0);

            }
            return Vector2.zero;
        }

        /// <summary>
        /// Stop The Dragging Of The Canvas
        /// </summary>
        public void StopDrag()
        {
            m_Velocity.Drain();
            m_AccelerationTime.Drain();
        }

        #endregion

    }
}