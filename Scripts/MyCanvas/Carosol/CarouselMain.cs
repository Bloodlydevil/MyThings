using System;
using UnityEngine;
using MyThings.Data;
using MyThings.Extension;
namespace MyThings.MyCanvas.Carousel
{
    public class CarouselMain : MonoBehaviour
    {
        [SerializeField] private AnimationCurve m_Size;
        [SerializeField] private RectTransform m_Container;
        [SerializeField] private RectTransform m_ElementHolder;
        [SerializeField] private RectTransform m_ElementYLoc;
        [SerializeField] private GameObject m_ElementPrefab; // Prefab for the companion
        [SerializeField] private float m_DistanceBetweenElement = 100f; // Distance between companions
        [SerializeField] private Deque<CarouselElement> m_Element = new Deque<CarouselElement>();


        public CarouselElement ActiveElement => m_MiddleElement;
        public bool IsNotInMid => m_MiddleSnap || m_InertialSpeed != 0;
        public bool IsMoving { get;private set; }

        #region Constants

        private const float _CycleThreshold = 0.9f; // Threshold for cycling Element
        private const float _SnapThreshold = 5f; // Threshold for snapping Element
        private const float _SnapSpeed = 3f; // Speed for snapping Element
        private const float _InertiaTreshold = 10f; // Threshold for inertia snapping
        private const float _InertiaNonMoveTreshold = 0.1f; // Threshold for inertia snapping when not moving
        private const float _InertiaResistance = 1f; // Resistance to the inertia of the Element movement
        private const float _InertiaNonMoveResistance = 0.1f; // Resistance to the inertia of the Element movement when not moving
        private const float _InertiaGain = 0.1f; // Gain for the inertia of the Element movement
        private const float _ThresholdToMove = 4f; // Threshold to move the Element

        #endregion

        #region private variables

        private CarouselElement m_MiddleElement; // Offset for snapping Element
        private float m_LeftContainerLocation = 0; // Minimum Location for Element
        private float m_LeftElementThreshold = 0; //  leftmost Element Threshold (Going Beyond it Will Cycle)
        private float m_RightElementThreshold = 0; //  rightmost Element Threshold (Going Beyond it Will Cycle)
        private bool m_MiddleSnap = false; // Flag for Snaping to the nearest Element
        private bool m_InertiaSnap = false; // Flag for inertia snapping
        private float m_InertialSpeed; // Speed of the Element movement

        #endregion

        public event Action OnElementMoveEnd;
        public Func<GameObject, Transform, GameObject> CreateElement;

        #region Unity Events

        private void Start()
        {
            m_LeftContainerLocation = -(int)(m_Container.rect.width / 2);
            if (m_Element.Count == 0)
                PopulateQueue();
            else
                ReSetElementsPosition();
            if (m_Element.Count != 0)
                UpdatePosition(Vector2.zero);
        }

        private void Update()
        {
            if (m_MiddleSnap)
                MiddleSnap();
            if (m_InertiaSnap)
                InertialSnap();
            else if (0 != m_InertialSpeed)
                InertialReduce(_InertiaNonMoveResistance, _InertiaNonMoveTreshold);
        }

        private void OnValidate()
        {
            if (m_Container == null || m_ElementHolder == null || m_ElementYLoc == null || m_ElementPrefab == null)
            {
                Debug.LogError("CarouselMain: Missing references in the inspector. Please assign all required fields.");
                return;
            }
            if (m_Size == null)
            {
                Debug.LogError("CarouselMain: Size curve is not assigned. Please assign a valid AnimationCurve.");
                return;
            }
            if (m_DistanceBetweenElement <= 0)
            {
                Debug.LogWarning("CarouselMain: Distance between elements should be greater than zero. Setting to default value of 100.");
                m_DistanceBetweenElement = 100f;
            }
            if (m_Element.Count == 0)
            {
                PopulateQueue();
                return;
            }
            m_LeftContainerLocation = -(int)(m_Container.rect.width / 2);
            ReSetElementsPosition();
        }

        #endregion

        #region private methods

        private void CycleThroughForward()
        {
            var LastCompanion = m_Element.RemoveFromFront();
            LastCompanion.RectTrans.localPosition = new Vector3(m_Element.PeekBack().RectTrans.localPosition.x + m_DistanceBetweenElement,
                                                                m_ElementYLoc.anchoredPosition.y, 0);
            m_Element.AddToBack(LastCompanion);
        }

        private void CycleThroughBackward()
        {
            var FirstCompanion = m_Element.RemoveFromBack();

            FirstCompanion.RectTrans.localPosition = new Vector3(m_Element.PeekFront().RectTrans.localPosition.x - m_DistanceBetweenElement,
                                                                m_ElementYLoc.anchoredPosition.y, 0);
            m_Element.AddToFront(FirstCompanion);
        }

        private void CheckCycle()
        {

            if (m_Element.PeekFront().RectTrans.anchoredPosition.x >= m_RightElementThreshold)
                CycleThroughBackward();

            else if (m_Element.PeekBack().RectTrans.anchoredPosition.x <= m_LeftElementThreshold)
                CycleThroughForward();
        }

        private void MiddleSnap()
        {
            float offset = Mathf.Lerp(0, m_MiddleElement.RectTrans.anchoredPosition.x, Time.deltaTime * _SnapSpeed);

            UpdatePosition((-offset+ m_InertialSpeed) * Vector2.right , false);

            if (Mathf.Abs(m_MiddleElement.RectTrans.anchoredPosition.x) <= _SnapThreshold)
            {
                Vector2 delta = new Vector2(0, m_MiddleElement.RectTrans.anchoredPosition.y) - m_MiddleElement.RectTrans.anchoredPosition;

                UpdatePosition(delta, false); // Apply exact correction

                m_MiddleSnap = false; // Stop auto-setting when the companion is snapped to the center

                IsMoving = false; // Stop moving when the companion is snapped to the center
            }
        }
        private void InertialSnap()
        {
            UpdatePosition(m_InertialSpeed*Vector2.right, false);

            InertialReduce(); // Reduce the inertia speed
        }
        private void InertialReduce(float multipleFactor=1,float Threshold=_InertiaTreshold)
        {
            m_InertialSpeed = Mathf.Lerp(m_InertialSpeed, 0, _InertiaResistance * Time.deltaTime* multipleFactor);

            if (Mathf.Abs(m_InertialSpeed) < Threshold)
            {
                m_InertialSpeed = 0; // Stop inertia snapping when speed is below threshold

                if (m_InertiaSnap)
                {

                    m_MiddleSnap = true; // Start smooth snapping to the nearest companion
                    SnapToMid(); // Snap to the nearest companion

                    m_InertiaSnap = false; // Stop inertia snapping
                }
            }
        }
        private void ReSetElementsPosition()
        {
            if (m_Element.Count == 0)
            {
                return;
            }

            int midIndex = m_Element.Count / 2;

            int i = 0;

            foreach (var element in m_Element)
            {
                element.RectTrans.anchoredPosition = new Vector2((i - midIndex) * m_DistanceBetweenElement, m_ElementYLoc.anchoredPosition.y);
                element.RectTrans.localScale = Vector3.one * m_Size.Evaluate((element.RectTrans.anchoredPosition.x - m_LeftContainerLocation) / m_Container.rect.width);


                if (i == midIndex)
                    m_MiddleElement = element;
                i++;
            }

            m_LeftElementThreshold = m_Element.PeekBack().RectTrans.anchoredPosition.x - m_DistanceBetweenElement * _CycleThreshold;
            m_RightElementThreshold = m_Element.PeekFront().RectTrans.anchoredPosition.x + m_DistanceBetweenElement * _CycleThreshold;
            // These thresholds are used to determine when to cycle companions
            // its calculated based On The Left Most and Right Most Position And Adding Additional Distance With some Limitation
        }

        private void PopulateQueue()
        {
            m_Element.Clear();

            for (int i = 0; i < m_ElementHolder.childCount; i++)
            {
                var child = m_ElementHolder.GetChild(i).gameObject;
                if (child.TryGetComponent(out CarouselElement companion))
                {
                    m_Element.AddToFront(companion);
                    companion.SetMovement(this);
                }
                else
                {
                    Debug.LogWarning($"CarouselMain: Child {child.name} does not have a CarouselElement component.");
                }
            }
        }

        #endregion

        #region Public Methods

        public void StartUpdate()
        {
            m_MiddleSnap = false;
            m_InertiaSnap = false;
            m_InertialSpeed = 0; // Reset inertia speed when starting to drag
        }

        public void UpdatePosition(Vector2 DeltaMovement, bool External = true)
        {
            if (External && Mathf.Abs( DeltaMovement.x) > _ThresholdToMove)
                IsMoving = true;

            Vector3 delta = new Vector2(DeltaMovement.x, 0);

            foreach (var companion in m_Element)
            {
                companion.RectTrans.position += delta;
                companion.RectTrans.localScale = Vector3.one *
                        m_Size.Evaluate((companion.RectTrans.anchoredPosition.x - m_LeftContainerLocation) / m_Container.rect.width);
            }

            if (External)
                m_InertialSpeed += (_InertiaGain * DeltaMovement.x);
            CheckCycle();
        }

        public void SnapToMid()
        {
            m_MiddleElement = m_Element.PeekFront();
            foreach (var companion in m_Element)
            {
                if (Mathf.Abs(companion.RectTrans.anchoredPosition.x) < Mathf.Abs(m_MiddleElement.RectTrans.anchoredPosition.x))
                {
                    m_MiddleElement = companion;
                }
            }

            m_InertiaSnap = true;

            CheckCycle();
            OnElementMoveEnd?.Invoke();
        }

        public void AddNewItem(Action<CarouselElement> Callback=null)
        {
            GameObject companionObject;

            if (CreateElement == null)
                companionObject = Instantiate(m_ElementPrefab, m_ElementHolder);
            else
                companionObject = CreateElement(m_ElementPrefab, m_ElementHolder);

            companionObject.name += " " + m_Element.Count; // Give a unique name to the new companion

            var companion = companionObject.GetComponent<CarouselElement>();
            m_Element.AddToBack(companion);


            companion.SetMovement(this);

            m_LeftContainerLocation = -(int)(m_Container.rect.width / 2);


            ReSetElementsPosition();

            Callback?.Invoke(companion);

        }

        public void RemoveLastItem()
        {
            if (m_Element.Count == 0)
            {
                Debug.LogWarning("CarouselMain: No items to remove.");
                return;
            }
            DestroyImmediate(m_Element.RemoveFromBack().gameObject);

            ReSetElementsPosition();
        }

        public void ClearAllItems()
        {
            for (int i = m_Element.Count - 1; i >= 0; i--)
            {
                DestroyImmediate(m_Element.RemoveFromBack().gameObject);
            }
        }

        public void FocusOn(CarouselElement element)
        {
            m_MiddleElement = element;

            UpdatePosition(-element.RectTrans.anchoredPosition, false);

            CarouselElement oldElement;
            do
            {
                oldElement = m_Element.PeekFront();
                //oldElement.transform.GetChild(0).GetComponent<AlbumItemHandler>().m_TitleText.text.Print("  old", color: Color.coral);
                CheckCycle();
                //m_Element.PeekFront().transform.GetChild(0).GetComponent<AlbumItemHandler>().m_TitleText.text.Print("  new", color: Color.red);
            } while (oldElement!= m_Element.PeekFront());
        }
        public void FocusOn(int index)
        {
            index = index-m_Element.Count/2;
            while (index != 0)
            {
                if(index>0)
                {
                    m_Element.AddToBack(m_Element.RemoveFromFront());
                    index--;
                }
                else
                {
                    m_Element.AddToFront(m_Element.RemoveFromBack());
                    index++;
                }
            }

            ReSetElementsPosition();
        }
        #endregion

    }
}