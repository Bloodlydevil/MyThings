using MyThings.Timer;
using TMPro;
using UnityEngine;
namespace MyThings.FPS
{
    /// <summary>
    /// Class to constantly monitor fps
    /// </summary>
    public class FPSShower : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI m_text;
        [SerializeField] private float RefreshRate;
        [SerializeField] private bool m_enabled;
        [SerializeField] private bool m_DontDestroyOnLoad;

        private ITimer m_timer;
        private void Start()
        {
            if (!m_enabled)
            {
                gameObject.SetActive(false);
                return;
            }
            if(m_DontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            m_timer=TimerManager.Create(RefreshRate, () => m_text.text= FPS.FPS_string, true);
            m_timer.Start();
        }
        private void OnDestroy()
        {
            m_timer.Stop();
        }
    }
}