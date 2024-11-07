using MyThings.Window.Windows;
using System;
using UnityEngine.UI;
namespace MyThings.Window.Utills
{
    /// <summary>
    /// A Prompt Rasier When We Click Accept Or Reject For Final Confermation
    /// </summary>
    public class AcceptRejectPrompt
    {
        private Action m_OnAccept;
        private Action m_OnReject;
        private string m_AcceptText;
        private string m_RejectText;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="acc">The Accept Button</param>
        /// <param name="rej">The REject Button</param>
        /// <param name="AcceptText">The Text To Display On Prompt</param>
        /// <param name="onAccept">The Function To Call On Accepting</param>
        /// <param name="onReject">The Function To Call When Rejecting</param>
        public AcceptRejectPrompt(Button acc, Button rej, string AcceptText,string RejectText, Action onAccept, Action onReject)
        {
            acc.onClick.AddListener(OnAdd);
            rej.onClick.AddListener(OnCancel);
            m_AcceptText = AcceptText;
            m_RejectText = RejectText;
            m_OnAccept = onAccept;
            m_OnReject = onReject;
        }
        private void OnAdd()
        {
            WindowPrompt.CreateOverLayScreen(m_AcceptText, OnActualAdd, () => { });

        }
        private void OnCancel()
        {
            WindowPrompt.CreateOverLayScreen(m_RejectText, OnActualCancel, () => { });

        }
        private void OnActualAdd()
        {
            m_OnAccept();
        }
        private void OnActualCancel()
        {
            m_OnReject();
        }
    }
}