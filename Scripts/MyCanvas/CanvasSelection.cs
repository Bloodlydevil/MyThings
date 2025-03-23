using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MyThings.MyCanvas
{

    /// <summary>
    /// A Class To Allow Selection Functionality
    /// </summary>
    public class CanvasSelection : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {

        [SerializeField] private GameObject m_SelectionVisual;
        [SerializeField] private RectTransform m_Canvas;
        
        private Vector2 m_DragStart;
        private Vector2 m_Center;


        public event Action OnSelectionEnd;
        public event Action OnDeSelect;
        public event Action<Vector2, Vector2> OnSelectAreaChange;

        [field: SerializeField] public PointerEventData.InputButton SelectionButton { get; set; }
        [field: SerializeField] public float CanvasScaleFactor { get; set; }

        private void Start()
        {
            m_SelectionVisual.SetActive(false);
        }
        /// <summary>
        /// Correct The Position Of The Mouse To Make It Centered With the Work Area
        /// </summary>
        /// <param name="MousePosition">The Raw Mouse Position To Correct</param>
        /// <returns>Corrected Mouse Position</returns>
        private Vector2 MakeMouseCorrect(Vector2 MousePosition)
        {
            
            return ((MousePosition - m_Center) /CanvasScaleFactor - m_Canvas.anchoredPosition) / m_Canvas.localScale.y;
        }

        public void SetUp(Vector2 ScreenSize)
        {
            m_Center= ScreenSize / 2 + m_Canvas.anchoredPosition * CanvasScaleFactor;
        }
        #region Events


        public void OnBeginDrag(PointerEventData eventData)
        {
            // Start The Drag From This Point 

            if (eventData.button != SelectionButton)
                return;
            m_DragStart = eventData.position;
            if (m_SelectionVisual != null)
                m_SelectionVisual.SetActive(true);

            // Remove Any Previous Selection

            OnDeSelect?.Invoke();

        }

        public void OnDrag(PointerEventData eventData)
        {
            // Show A Rectangle Up To This Point

            if (eventData.button != SelectionButton)
                return;
            Vector2 Start = MakeMouseCorrect(m_DragStart);
            Vector2 Right = MakeMouseCorrect(eventData.position);

            Vector2 left = new Vector2(Mathf.Min(Start.x, Right.x), Mathf.Min(Start.y, Right.y));
            Vector2 right = new Vector2(Mathf.Max(Start.x, Right.x), Mathf.Max(Start.y, Right.y));

            // Select All The Elements Inside The Rectangle

            OnSelectAreaChange?.Invoke(left, right);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Finish All The Selection And Adjust Everything Based On The Selection

            if (eventData.button != SelectionButton)
                return;
            if (m_SelectionVisual != null)
                m_SelectionVisual.SetActive(false);

            OnSelectionEnd?.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            // If You Are Just Clicking on The Screen Then  Selection Must Be Removed
            // Comment these out if You Dont Want Clicking Ends Selection


            if (!eventData.dragging && eventData.button == SelectionButton)
            {
                OnDeSelect?.Invoke();
            }
        }

        #endregion
    }
}