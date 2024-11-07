using System;
using MyThings.Job_System;
namespace MyThings.CAnimation.Evaluator
{
    /// <summary>
    /// A Base Class For Evaluators
    /// </summary>
    /// <typeparam name="t">The Typ Of Output Used</typeparam>
    [Serializable]
    public class Evaluator<t> : IAnimation
    {
        /// <summary>
        /// The Changer Used For Animation
        /// </summary>
        protected Completion _changer;

        /// <summary>
        /// The Output Buffer
        /// </summary>
        protected t[] _OutputBuffer;

        /// <summary>
        /// Function Signature To Give OutPut In
        /// </summary>
        /// <param name="t">The Type Of Data Provided</param>
        public delegate void Output(t[] t);

        /// <summary>
        /// The Outputer Function Used
        /// </summary>
        protected Output _outputer;

        #region Interface Functions


        public void Stop() => _changer.Stop();
        public void ForceEnd() => _changer.ForceEnd();
        public void Start() => _changer.Start();
        public void AddCallBackOnFinish(Action action) => _changer.AddOnEnd(action);
        public void SetCallBackOnFinish(Action action) => _changer.SetOnEnd(action);
        public void RemoveCallBackOnFinish(Action action) => _changer.RemoveOnEnd(action);

        #endregion
    }
}