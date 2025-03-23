using MyThings.Data;
using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MyThings.Canvas
{
    /// <summary>
    /// A Class To Allow Movement Of The Work Area
    /// </summary>
    public class CanvasDragger : MonoBehaviour, IDragHandler
    {



        [Tooltip("The Velocity Limit for The Drag")]
        [SerializeField] private Jug m_Velocity = new Jug(50, 0);


        [Tooltip("Acceleration That The Auto Drag Should Follow")]
        [SerializeField] private AnimationCurve m_Acceleration;


        [Tooltip("The Time Required To Reach Max Velocity Of the drag")]
        [SerializeField] private Jug m_AccelerationTime = new Jug(5, 0);



        private RectTransform m_RectTransform;


        public event Action OnDragging;


        [Tooltip("The Button To Use For Dragging")]
        [field: SerializeField] public PointerEventData.InputButton DragMovementButton { get; set; } = PointerEventData.InputButton.Right;

        [Tooltip("The Canvas Scale Factor Used")]
        [field: SerializeField] public float CanvasScaleFactor { get; set; } = 1;

        [Tooltip("The Function Which Adjust The Screen When Dragging ")]
        public Func<Vector2> CanvasAdjester { get; set; }



        private void Start()
        {
            m_RectTransform = (RectTransform)transform;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button == DragMovementButton)
            {
                m_RectTransform.anchoredPosition += eventData.delta / CanvasScaleFactor;
                CanvasAdjester?.Invoke();
                OnDragging?.Invoke();
            }
        }
        /// <summary>
        /// Drag The Canvas In The Direction Where We Are Trying To Go Based On The Direction Where Mouse Is
        /// </summary>
        /// <param name="CAMousePoint">The Point Where Mouse Is .The Mouse Direction should be from Center of The Screen</param>
        /// <returns>Adjustement done While Dragging In The Direction</returns>
        public Vector2 ManualDrag(Vector3 CAMousePoint)
        {
            m_Velocity += m_Acceleration.Evaluate(m_AccelerationTime.NormalizedLevel);

            m_AccelerationTime += Time.deltaTime;

            Vector3 DragValue = CAMousePoint.normalized * m_Velocity;
            m_RectTransform.localPosition -= DragValue;



            OnDragging?.Invoke();



            // dragging Done

            // Since It Is Manual Dragging So Return The Amount Adjested




            if (CanvasAdjester != null)
            {
                Vector2 Adjust = CanvasAdjester();

                return new Vector2(Adjust.x > 0 ? DragValue.x : 0, Adjust.y > 0 ? DragValue.y : 0);

            }
            return Vector2.zero;
        }

        /// <summary>
        /// Stop The Draging Of The Canvas
        /// </summary>
        public void StopDrag()
        {
            m_Velocity.Drain();
            m_AccelerationTime.Drain();
        }
    }
}