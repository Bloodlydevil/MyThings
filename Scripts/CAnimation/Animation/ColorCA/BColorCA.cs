using MyThings.CAnimation.Outputs;
using System;
using UnityEngine;
using MyThings.Job_System;

namespace MyThings.CAnimation.Animation.ColorCA
{
    /// <summary>
    /// The Base Class all The Color Cahnger Must Use To Have Easier Time
    /// </summary>
    [Serializable]
    public class BColorCA : IAnimation
    {
        /// <summary>
        /// The Changer Used To Animate The Color Change
        /// </summary>
        protected Completion _changer;
        /// <summary>
        /// The Output Giver Used 
        /// </summary>
        protected IOutput<Color> _output;
        /// <summary>
        /// The Colors Buffer Used To Give The Output
        /// </summary>
        protected Color[] _OutputBuffer;


        #region Interface Functions


        public void Stop()=> _changer.Stop();
        public void ForceEnd()=> _changer.ForceEnd();
        public void Start()=> _changer.Start();
        public void AddCallBackOnFinish(Action action) => _changer.AddOnEnd(action);
        public void SetCallBackOnFinish(Action action) => _changer.SetOnEnd(action);
        public void RemoveCallBackOnFinish(Action action) => _changer.RemoveOnEnd(action);


        #endregion
    }
}