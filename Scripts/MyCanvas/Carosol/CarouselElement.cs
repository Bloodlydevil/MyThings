using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyThings.MyCanvas.Carousel
{
    public class CarouselElement : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public RectTransform RectTrans;
        public Button ClickButton;

        private Vector2 m_LastPos;
        private CarouselMain m_Movement;

        private void Start()
        {
            RectTrans = transform as RectTransform;
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            m_LastPos = eventData.position;
            m_Movement.StartUpdate();
        }

        public void OnDrag(PointerEventData eventData)
        {
            m_Movement.UpdatePosition(eventData.position - m_LastPos);
            m_LastPos = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            m_Movement.SnapToMid();
        }

        public void SetMovement(CarouselMain movement)
        {
            m_Movement = movement;
        }
    }
}