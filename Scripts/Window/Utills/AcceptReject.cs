using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MyThings.Window.Utills
{
    /// <summary>
    /// A class To Hold Accept And Reject For Any window
    /// </summary>
    public class AcceptReject : MonoBehaviour
    {
        /// <summary>
        /// The Accept Button
        /// </summary>
        public Button Accept;
        /// <summary>
        /// The Reject Button
        /// </summary>
        public Button Reject;

        /// <summary>
        /// Link The Button With The Functions
        /// </summary>
        /// <param name="Accept">The Accept Function</param>
        /// <param name="Reject">The Reject Function</param>
        public void Link(UnityAction Accept, UnityAction Reject)
        {
            this.Accept.onClick.AddListener(Accept);
            this.Reject.onClick.AddListener(Reject);
        }
    }
}
