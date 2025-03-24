using System;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MyThings.MyCanvas
{

    /// <summary>
    /// A Class To Allow Any sort Of Selection To Be Done
    /// </summary>
    public class CanvasSelection : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Tooltip("The Visual Used In The Selection")]
        [SerializeField] private GameObject m_SelectionVisual;
        [Tooltip("The Selection Location (The Playe Where We Can Select)")]
        [SerializeField] private RectTransform m_SelectionArea;

        [Tooltip("The Location From Where Selection Dragg Has Started")]
        private Vector2 m_DragStart;
        [Tooltip("The Center Of The Screen")]
        private Vector2 m_Center;

        [Tooltip("This Event Is Called When Selection Has Finnished")]
        public event Action OnSelectionEnd;
        [Tooltip("This Event Is Called When Selection Has Been Performed")]
        public event Action OnDeSelect;
        [Tooltip("This Event Is Called When Any Selection Area Is Changed")]
        public event Action<Vector2, Vector2> OnSelectAreaChange;

        [Tooltip("The Button Used During Selection")]
        [field: SerializeField] public PointerEventData.InputButton SelectionButton { get; set; }
        [Tooltip("The Canvas Scale Factor")]
        [field: SerializeField] public float CanvasScaleFactor { get; set; }



        private void Start()
        {
            m_SelectionVisual.SetActive(false);
        }


        /// <summary>
        /// Correct The Position Of The Mouse To Make It Centered With the Selection Area
        /// </summary>
        /// <param name="MousePosition">The Raw Mouse Position To Correct</param>
        /// <returns>Corrected Mouse Position</returns>
        private Vector2 MakeMouseCorrect(Vector2 MousePosition)
        {
            
            return ((MousePosition - m_Center) /CanvasScaleFactor - m_SelectionArea.anchoredPosition) / m_SelectionArea.localScale.y;
        }
        /// <summary>
        /// Set Up The Selction Canvas Class
        /// </summary>
        /// <param name="ScreenSize">The Size Of The Screen</param>
        public void SetUp(Vector2 ScreenSize)
        {
            m_Center= ScreenSize / 2 + m_SelectionArea.anchoredPosition * CanvasScaleFactor;
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