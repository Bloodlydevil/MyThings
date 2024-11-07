using MyThings.CAnimation.Outputs;
using UnityEngine;
using MyThings.Job_System;
namespace MyThings.CAnimation.Animation.ColorCA
{
    /// <summary>
    /// Used To Animate Color Using Gradient
    /// </summary>
    [System.Serializable]
    public class ColorCA_G : BColorCA
    {
        /// <summary>
        /// The Gradients
        /// </summary>
        private Gradient[] _gradients;


        #region Constructors

        /// <summary>
        /// Constructor To Create The CAnimation
        /// </summary>
        /// <param name="Time">The Time Of The Animation</param>
        /// <param name="output">The Outputer</param>
        /// <param name="gradients">The Gradients</param>
        /// <param name="TimeScale">The Time Scale</param>
        public ColorCA_G(float Time, IOutput<Color> output, bool TimeScale = true,params Gradient[] gradients)
        {
            _output = output;
            _gradients = gradients;
            _OutputBuffer = new Color[gradients.Length];
            if (gradients.Length > 1)
                _changer = new Completion(Time, ChangMul, TimeScale);
            else
                _changer = new Completion(Time, ChangeSing, TimeScale);
        }


        #endregion


        #region Private Functions


        /// <summary>
        /// We Consider Thier is equal no of outputs giver and Gradient Used
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangMul(float change)
        {
            for (int i = 0; i < _gradients.Length; i++)
            {
                _OutputBuffer[i] = _gradients[i].Evaluate(change);
            }
            _output.MulOutput(_OutputBuffer);
        }
        /// <summary>
        /// We Consider Single Color Is Given To All The Output Giver
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangeSing(float change)
        {
            _OutputBuffer[0] = _gradients[0].Evaluate(change);
            _output.SingOutput(_OutputBuffer[0]);
        }


        #endregion
    }
}