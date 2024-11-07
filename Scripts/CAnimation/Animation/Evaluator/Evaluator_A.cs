using System;
using UnityEngine;
using MyThings.Job_System;
namespace MyThings.CAnimation.Evaluator
{
    /// <summary>
    /// An Evaluator Used TO Evaluate Animation Curve
    /// </summary>
    [Serializable]
    public class Evaluator_A : Evaluator<float>
    {
        /// <summary>
        /// The Animation Curves To Evaluate
        /// </summary>
        private AnimationCurve[] Curves;
        /// <summary>
        /// A Constructor To Crete Animation To Evaluate Animation Curve
        /// </summary>
        /// <param name="MaxTime">The Max Time Of The Animation</param>
        /// <param name="output">The Function To Give Output To</param>
        /// <param name="TimeScale">The Time Scale Used</param>
        /// <param name="curves">The Animation Curves Used</param>
        public Evaluator_A(float MaxTime,Output output,bool TimeScale=true,params AnimationCurve[] curves) 
        {
            Curves=curves;
            _outputer=output;
            _OutputBuffer=new float[Curves.Length];
            _changer = new Completion(MaxTime,Update, TimeScale);
        }
        /// <summary>
        /// Function To Update The Animation Curve Values
        /// </summary>
        /// <param name="Time">The Time Fraction</param>
        private void Update(float Time)
        {
            for(int i = 0; i < Curves.Length; i++)
            {
                _OutputBuffer[i] = Curves[i].Evaluate(Time);
            }
            _outputer(_OutputBuffer);
        }
    }
}