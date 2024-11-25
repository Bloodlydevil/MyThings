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

        public string Title { get => m_Title.text; set => m_Title.text = value; }
        public RectTransform AvailableArea => m_AvailableArea;
        public event Action OnWindowClose;

        public void SetUp(WindowMode mode)
        {
            if (TryGetComponent(out IWindowHelper comp))
            {
                Helper = comp;
            }
            Camera_C.Instance.OnCameraChanged += SetCamera;
            Camera_C.OnCameraFirstAcquired += SetCamera;
            CursorLock.Unlock();
            m_CloseButton.onClick.AddListener(() => Destroy(gameObject));
            ProperDrag.Attach(m_Window, mode).AddNoDragRegion(m_RightEmpty);

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