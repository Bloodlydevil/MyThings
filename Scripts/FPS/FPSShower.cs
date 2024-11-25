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
        private void Start()
        {
            if (!m_enabled)
            {
                gameObject.SetActive(false);
                return;
            }
            if(m_DontDestroyOnLoad)
                DontDestroyOnLoad(gameObject);
            TimerManager.Create(RefreshRate, () => m_text.text= FPS.FPS_string, true).Start();
        }
    }
}