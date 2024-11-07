using UnityEngine;
using MyThings.Data;
using MyThings.Job_System;
using MyThings.Timer;

namespace MyThings.Window.Utills
{
    /// <summary>
    /// An Element Hower Way To Show And Hide Based On The Mouse
    /// </summary>
    public class ElementHowerShow : MonoBehaviour
    {
        [Tooltip("The Position When Considerd Hidden")]
        [SerializeField] private Vector3_Draw m_HidePos;
        [Tooltip("The Position When Considerd Showen")]
        [SerializeField] private Vector3_Draw m_ShowPos;
        [Tooltip("The Element To Hide And Show")]
        [SerializeField] private RectTransform m_Element;
        [Tooltip("The Time REquired To Hide And Show")]
        [SerializeField] private float m_HideShowTime;
        [Tooltip("The time Reqired Before Starting To Hide")]
        [SerializeField] private float m_HideTime;
        private Vector3Move Vector3Move;
        private Completion m_Movement;
        private ITimer m_HideTimer;
        private void Start()
        {
            Vector3Move.Position = m_Element.position;
            m_HideTimer = TimerManager.Create(m_HideTime, () => m_Movement.Start());
            m_Movement = new Completion(m_HideShowTime, MoveUpdate);
        }
        private void OnDrawGizmos()
        {
            m_HidePos.Draw(m_Element.position);
            m_ShowPos.Draw(m_Element.position);
        }
        private void OnMouseEnter()
        {
            Vector3Move.Target = Vector3Move.Position + m_ShowPos;
            m_Movement.Start();
            m_HideTimer.Stop();
        }
        private void OnMouseExit()
        {
            Vector3Move.Target = Vector3Move.Position + m_ShowPos;
            m_HideTimer.Start();
        }
        private void MoveUpdate(float per)
        {
            m_Element.position = Vector3Move.Lerp(per);
        }
    }
}