using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyThings.Window.Utills
{
    public class AcceptReject : MonoBehaviour
    {
        [SerializeField] private Button m_Accept;
        [SerializeField] private Button m_Reject;

        public void SetUp(UnityAction Accept, UnityAction Reject)
        {
            m_Accept.onClick.AddListener(Accept);
            m_Reject.onClick.AddListener(Reject);
        }
    }
}
