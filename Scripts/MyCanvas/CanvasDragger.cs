using MyThings.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MyThings.MyCanvas
{
    /// <summary>
    /// A Class To Allow Any Canvas To Be Dragged
    /// </summary>
    public class CanvasDragger : MonoBehaviour, IDragHandler , IPointerDownHandler ,IBeginDragHandler ,IEndDragHandler
    {
        #region Members

        [Tooltip("The Velocity Limit for The auto Drag")]
        [SerializeField] private Jug m_Velocity = new Jug(50, 0);

        [Tooltip("Acceleration That The Auto Drag Should Follow")]
        [SerializeField] private AnimationCurve m_Acceleration;

        [Tooltip("The Time Required To Reach Max Velocity Of the auto drag")]
        [SerializeField] private Jug m_AccelerationTime = new Jug(5, 0);

        [Tooltip("The Default Scale Used To Allow For Proper Dragging Even If Scale Is Changed")]
        private float m_DefaultScale;

        [Tooltip("once Some External Influe Comes Then This Helps With Countering It")]
        private Vector2 m_DeltaCenter;

        [Tooltip("Event Is Called When Dragging (Mouse Location Is Send)")]
        public event Action<Vector2> OnDragging;

        [Tooltip("Event Is Called When Dragging Starts")]
        public event Action OnDragStart;

        [Tooltip("Event Is Called When Dragging Ends")]
        public event Action OnDragOver;

        [Tooltip("The Button To Use For Dragging")]
        [field: SerializeField] public PointerEventData.InputButton DragMovementButton { get; set; } = PointerEventData.InputButton.Right;

        [Tooltip("The Canvas Scale Factor Used")]
        [field: SerializeField] public float CanvasScaleFactor { get; set; } = 1;

        [Tooltip("The Function Which Adjust The Canvas When Dragging ")]
        public Func<Vector2> CanvasAdjester { get; set; }

        [Tooltip("The Screen Center ")]
        [field: SerializeField] public Vector2 ScreenCenter { get; set; }

        [Tooltip("The Canvas To Drag")]
        [field: SerializeField] public RectTransform Draggable { get; set; }

        [Tooltip("Used For Correcting The Exact Position Of Dragging")]
        public Vector2 DeltaGrabPosition { get; set; }

        #endregion

        #region Private
        /// <summary>
        /// Get The Multiplier To Use For The Objects
        /// </summary>
        /// <returns></returns>
        private float GetMultiplier()
        {
            return (m_DefaultScale / (Draggable.lossyScale.x / Draggable.localScale.x));
        }

        #endregion

        #region Events

        public void OnPointerDown(PointerEventData eventData)
        {
            SetGrabPostion(eventData.position);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDragStart?.Invoke();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == DragMovementButton)
            {
                // This Will Not Work If Draggable Object Is A Local of Another Object And That Moves

                // Maybe The Problem Will Solve Itself if i Set DeltaCener to Be Node Location

                // DeltaGrab Position Also Helps With A Local Objects Positioning Correction

                Draggable.anchoredPosition = m_DeltaCenter
                    +((eventData.position - ScreenCenter)* GetMultiplier()+ DeltaGrabPosition) / CanvasScaleFactor;

                CanvasAdjester?.Invoke();

                OnDragging?.Invoke(eventData.position- ScreenCenter);
            }
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            OnDragOver?.Invoke();
        }
        #endregion

        #region Public

        public void SetGrabPostion(Vector2 position)
        {
            DeltaGrabPosition = Draggable.anchoredPosition * CanvasScaleFactor - (position - ScreenCenter) * GetMultiplier();
        }

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

            OnDragging?.Invoke(MousePoint);
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
        public void ReSetCumulative()
        {
            m_DeltaCenter = Vector2.zero;
        }
        /// <summary>
        /// Set The Base Scale Of The Dragger
        /// </summary>
        /// <param name="LossyScale">The Scale To Use</param>
        public void SetBaseScale(float LossyScale)
        {
            m_DefaultScale = LossyScale;
        }
        /// <summary>
        /// Change The Location Of The Transform By <paramref name="DeltaLocation"/>
        /// </summary>
        /// <param name="DeltaLocation">The Change In The Location</param>
        public void ChangeLocationBy(Vector2 DeltaLocation)
        {
            m_DeltaCenter += DeltaLocation* GetMultiplier();
            Draggable.localPosition +=(Vector3) DeltaLocation* GetMultiplier();
        }
        #endregion

    }
}