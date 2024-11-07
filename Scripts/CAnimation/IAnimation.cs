using System;

namespace MyThings.CAnimation
{
    /// <summary>
    /// An Interface Used By All The Animations
    /// </summary>
    public interface IAnimation
    {
        /// <summary>
        /// A Function To Add A Action To The OnFinish CallBack Event
        /// </summary>
        /// <param name="action">The CallBack Function</param>
        public void AddCallBackOnFinish(Action action);
        /// <summary>
        /// A Function To Set A Action To The OnFinish CallBack Event
        /// </summary>
        /// <param name="action">The CallBack Function</param>
        public void SetCallBackOnFinish(Action action);
        /// <summary>
        /// A Function To Remove A Action To The OnFinish CallBack Event
        /// </summary>
        /// <param name="action">The CallBack Function</param>
        public void RemoveCallBackOnFinish(Action action);
        /// <summary>
        /// To Stop The Animation (Animation May Not Have Ended )
        /// </summary>
        public void Stop();
        /// <summary>
        /// To Start The Animation
        /// </summary>
        public void Start();
        /// <summary>
        /// To Force End The Job And End Animation
        /// </summary>
        public void ForceEnd();
    }
}