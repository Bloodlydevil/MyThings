using MyThings.CAnimation.Outputs;
using MyThings.Data;
using UnityEngine;
using MyThings.Job_System;
namespace MyThings.CAnimation.Animation.ColorCA
{
    /// <summary>
    /// A Animator Used To Linearly Animate From The Colors
    /// </summary>
    [System.Serializable]
    public class ColorCA_L : BColorCA
    {
        /// <summary>
        /// The Color(s) Used To Animate
        /// </summary>
        private DoubleValue<Color>[] _ColorBuffer;
        public DoubleValue<Color>[] ColorBuffer { get => _ColorBuffer; set => _ColorBuffer = value; }

        #region Constructors


        /// <summary>
        /// Contructor To Create ColorCA_L
        /// </summary>
        /// <param name="Time">The Animation Span</param>
        /// <param name="output">The Outputer Used</param>
        /// <param name="InputBuffer">The Color(s) Used</param>
        /// <param name="TimeScale">The TimeScale</param>
        public ColorCA_L(float Time, IOutput<Color> output, bool TimeScale = true,params DoubleValue<Color>[] InputBuffer)
        {
            _output = output;
            _ColorBuffer = InputBuffer;
            _OutputBuffer = new Color[InputBuffer.Length];
            if (InputBuffer.Length > 1)
                _changer = new Completion(Time, ChangMul, TimeScale);
            else
                _changer = new Completion(Time, ChangeSing, TimeScale);
        }

        // make some color cahnge
        #endregion


        #region Private Functions


        /// <summary>
        /// We Consider Thier is equal no of outputs giver and Gradient Used
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangMul(float change)
        {
            for (int i = 0; i < _ColorBuffer.Length; i++)
            {
                _OutputBuffer[i] = Color.LerpUnclamped(_ColorBuffer[i]._First, _ColorBuffer[i]._Second, change);
            }
            _output.MulOutput(_OutputBuffer);
        }
        /// <summary>
        /// We Consider Single Color Is Given To All The Output Giver
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangeSing(float change)
        {
            _OutputBuffer[0] = Color.LerpUnclamped(_ColorBuffer[0]._First, _ColorBuffer[0]._Second, change);
            _output.SingOutput(_OutputBuffer[0]);
        }


        #endregion

    }
}
