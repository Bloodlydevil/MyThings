using MyThings.Cashe;
using MyThings.Extension;
using MyThings.Pointer;
using MyThings.Window.WindowHelper;
using MyThings.Window.Utills;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
namespace MyThings.Window
{

    public class WindowBasic : MonoBehaviour
    {
        [field: SerializeField] public Canvas Canvas { get; private set; }
        [SerializeField] private Vector2 m_Size;
        [SerializeField] private TextMeshProUGUI m_Title;
        [SerializeField] private RectTransform m_AvailableArea;
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private Vector2 m_ExitSize;
        [SerializeField] private RectTransform m_RightEmpty;
        [SerializeField] private GameObject m_Window;
        public IWindowHelper Helper;
        private RectTransform rectTransform;

        public string Title { get => m_Title.text; set => m_Title.text = value; }
        public RectTransform AvailableArea => m_AvailableArea;
        public event Action OnWindowClose;

        public void SetUp(ProperDrag.Mode mode)
        {
            Helper = gameObject.GetComponent<IWindowHelper>();
            Camera_C.Instance.OnCameraChanged += SetCamera;
            Camera_C.OnCameraFirstAcquired += SetCamera;
            rectTransform = Canvas.GetComponent<RectTransform>();
            //rectTransform.sizeDelta = m_Size;
            CursorLock.Unlock();
            m_CloseButton.onClick.AddListener(() => Destroy(gameObject));
            //m_CloseButton.GetComponent<RectTransform>().sizeDelta = m_ExitSize;
            //m_Title.transform.localScale = m_ExitSize.Make(m_ExitSize.x);
            //m_AvailableArea.offsetMax = -m_ExitSize;
            //m_RightEmpty.offsetMax = new Vector2(0, -m_ExitSize.y);
            //m_RightEmpty.offsetMin = new Vector2(-m_ExitSize.x, 0);
            ProperDrag.Attach(m_Window, mode).AddNoDragRegion(m_RightEmpty);//, (RectTransform)m_CloseButton.transform);

        }
        private void OnDestroy()
        {
            Camera_C.Instance.IfTrue(i => i.OnCameraChanged -= SetCamera);
            Camera_C.OnCameraFirstAcquired -= SetCamera;
            CursorLock.RevertBack();
            OnWindowClose?.Invoke();
        }
        private void SetCamera(Camera camera)
        {
            Canvas.worldCamera = camera;
        }
        public void SetSize(Vector2 size)
        {
            m_Size = size;
        }
    }
}