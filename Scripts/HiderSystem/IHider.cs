using System;
using MyThings.Timer;

namespace MyThings.HiderSystem
{

    /// <summary>
    /// The Interface Which All Type Of Hider Must Have
    /// </summary>
    public interface IHider
    {
        /// <summary>
        /// The Timer This IHider Uses
        /// </summary>
        public ITimer Timer { get; set; }
        /// <summary>
        /// Remove The Element From AutoMatic IHider System
        /// </summary>
        public void UnSubscribe();
        /// <summary>
        /// Show The Element And Start The Timer
        /// </summary>
        public void Show();
        /// <summary>
        /// Only Show The Elements And Not Start The Timer
        /// </summary>
        public void ShowOnly();
        /// <summary>
        /// ForceFully Hide The Element
        /// </summary>
        public void ForceHide();
        /// <summary>
        /// End All Animation With or Without Finishing it
        /// </summary>
        public void End();
        /// <summary>
        /// Add Listener To Call When Hiding 
        /// </summary>
        /// <param name="action"></param>
        public void AddListenerForOnHide(Action action);
    }
}