using MyThings.CAnimation.Outputs;
using MyThings.Data;
using System;
using UnityEngine;
using MyThings.Job_System;
namespace MyThings.CAnimation.Animation.ColorCA
{
    /// <summary>
    /// Used To Animate Color Using Animation Curve
    /// </summary>
    [Serializable]
    public class ColorCA_A : BColorCA
    {
        /// <summary>
        /// The Color Group To Animate From
        /// </summary>
        private DoubleValue<Color>[] ColorBuffer;
        /// <summary>
        ///The Animation Curve Group Used To Animate
        /// </summary>
        private AnimationCurve[] ColorCurveBuffer;


        #region Constructors


        /// <summary>
        /// Constructor To Create A Color(s) Buffer Using Single Animator
        /// </summary>
        /// <param name="Time">The Time Span Of The Animation</param>
        /// <param name="output">The Outputer Used</param>
        /// <param name="InputBuffer">The Input Colors To Animate</param>
        /// <param name="curves">The Animation Curves Used</param>
        /// <param name="TimeScale">If Time Is Scaled Or Not</param>
        public ColorCA_A(float Time, IOutput<Color> output, AnimationCurve curves, bool TimeScale = true,params DoubleValue<Color>[] InputBuffer)
        {
            _output = output;
            ColorBuffer = InputBuffer;
            _OutputBuffer = new Color[InputBuffer.Length];
            ColorCurveBuffer = new AnimationCurve[] { curves };
            if(InputBuffer.Length>1)
                _changer = new Completion(Time, ChangMulSing , TimeScale);
            else
                _changer = new Completion(Time, ChangeSingSing, TimeScale);
        }
        /// <summary>
        /// Constructor To Create A Colors Buffer Using Multiple Animator
        /// </summary>
        /// <param name="Time">The Time Span Of The Animation</param>
        /// <param name="output">The Outputer Used</param>
        /// <param name="InputBuffer">The Input Colors To Animate</param>
        /// <param name="curves">The Animation Curves Used</param>
        /// <param name="TimeScale">If Time Is Scaled Or Not</param>
        public ColorCA_A(float Time, IOutput<Color> output, AnimationCurve[] curves, bool TimeScale = true, params DoubleValue<Color>[] InputBuffer)
        {
            _output = output;
            ColorBuffer = InputBuffer;
            _OutputBuffer = new Color[curves.Length];
            ColorCurveBuffer = curves;
            if (InputBuffer.Length > 1)
                _changer = new Completion(Time, ChangeMulMul, TimeScale);
            else
                _changer = new Completion(Time, ChangeSingMul, TimeScale);
        }


        #endregion


        #region Private Functions


        /// <summary>
        /// Single Animation Curve Is Used For Multiple Colors
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangMulSing(float change)
        {
            for (int i = 0; i < ColorBuffer.Length; i++)
            {
                _OutputBuffer[i] = Color.LerpUnclamped(ColorBuffer[i]._First, ColorBuffer[i]._Second, ColorCurveBuffer[0].Evaluate(change));
            }
            _output.MulOutput(_OutputBuffer);
        }
        /// <summary>
        /// Multiple Animation Curve Is Used For Multiple colors
        /// </summary>
        /// <param name="change"></param>
        private void ChangeMulMul(float change)
        {
            for (int i = 0; i < ColorBuffer.Length; i++)
            {
                _OutputBuffer[i] = Color.LerpUnclamped(ColorBuffer[i]._First, ColorBuffer[i]._Second, ColorCurveBuffer[i].Evaluate(change));
            }
            _output.MulOutput(_OutputBuffer);
        }
        /// <summary>
        /// Single Animation Curve Is Used For Single colors
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangeSingSing(float change)
        {
            _OutputBuffer[0] = Color.LerpUnclamped(ColorBuffer[0]._First, ColorBuffer[0]._Second, ColorCurveBuffer[0].Evaluate(change));
            _output.SingOutput(_OutputBuffer[0]);
        }
        /// <summary>
        /// Multiple Animation Curve Is Used For Single colors
        /// </summary>
        /// <param name="change">The Change</param>
        private void ChangeSingMul(float change)
        {
            for (int i = 0; i < ColorCurveBuffer.Length; i++)
            {
                _OutputBuffer[i] = Color.LerpUnclamped(ColorBuffer[0]._First, ColorBuffer[0]._Second, ColorCurveBuffer[i].Evaluate(change));
            }
            _output.MulOutput(_OutputBuffer);
        }


        #endregion
    }
}