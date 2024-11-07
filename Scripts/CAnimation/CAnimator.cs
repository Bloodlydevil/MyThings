using System;
using UnityEngine.UI;
using UnityEngine;
using MyThings.CAnimation.Outputs;
using MyThings.Data;
using MyThings.CAnimation.Animation.ColorCA;
using MyThings.CAnimation.Evaluator;
namespace MyThings.CAnimation
{
    /// <summary>
    /// The Class Which Acts As The Middle Man For All The Animation Through Code IAnimation
    /// </summary>
    public static class CAnimator
    {
        #region Color_CA


        #region G_Color


        /// <summary>
        /// Create A Code Animation Which Cahnges The Color Of The Outputer( Given By User )
        /// </summary>
        /// <param name="Time">The Animation Time</param>
        /// <param name="outputer">The Outputer Used</param>
        /// <param name="TimeScale">If Time Is Scaled Or NOt</param>
        /// <param name="grad">The Gradients Used </param>
        /// <returns>The Animation</returns>
        public static IAnimation G_Color(float Time,IOutput<Color> outputer,bool TimeScale=true, params Gradient[] grad)
        {
            if (grad.Length > 1 && grad.Length != outputer.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            return new ColorCA_G(Time, outputer, TimeScale, grad);
        }
        /// <summary>
        /// Create A Code Animation Which Cahnges The Color Of The t
        /// </summary>
        /// <typeparam name="t">The Type Of Output (Not All Type Is Supported)</typeparam>
        /// <param name="Time">The Animation Time</param>
        /// <param name="outputBuffer">The OutPuts To Put The Output into</param>
        /// <param name="TimeScale">If Time Is Scaled Or Not</param>
        /// <param name="grad">The Gradients Used </param>
        /// <returns>The Animation</returns>
        public static IAnimation G_Color<t>(float Time,Gradient[] grad, bool TimeScale = true,params t[] outputBuffer)
        {
            if (grad.Length > 1 && grad.Length != outputBuffer.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            return new ColorCA_G(Time, GetIOutput(outputBuffer), TimeScale, grad);
        }
        /// <summary>
        /// Create A Code Animation Which Cahnges The Color Of The t
        /// </summary>
        /// <typeparam name="t">The Type Of Output (Not All Type Is Supported)</typeparam>
        /// <param name="Time">The Animation Time</param>
        /// <param name="outputBuffer">The OutPuts To Put The Output into</param>
        /// <param name="TimeScale">If Time Is Scaled Or Not</param>
        /// <param name="grad">The Gradients Used </param>
        /// <returns>The Animation</returns>
        public static IAnimation G_Color<t>(float Time, Gradient grad, bool TimeScale = true,params t[] outputBuffer)
        {
            return new ColorCA_G(Time, GetIOutput(outputBuffer), TimeScale, grad);
        }


        #endregion


        #region L_Color


        /// <summary>
        /// Create A Code Animation Which Changes The Color Of The Outputer Through The Given Color
        /// </summary>
        /// <param name="Time">The Time For The Animation</param>
        /// <param name="output">The Outputer</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="ColorBuffer">The Color Buffer</param>
        /// <returns>The IAnimation Used To Animate</returns>
        public static IAnimation L_Color(float Time,IOutput<Color> output,bool TimeScale=true,params DoubleValue<Color>[] ColorBuffer)
        {
            if(ColorBuffer.Length>1&&ColorBuffer.Length!=output.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            else
                return new ColorCA_L(Time, output,TimeScale,ColorBuffer);
        }
        /// <summary>
        ///  Create A Code Animation Which Changes The Color Of The t Through The Given Color
        /// </summary>
        /// <typeparam name="t">The Type Of The Outputer Used</typeparam>
        /// <param name="Time">The Time For The Animation</param>
        /// <param name="ColorBuffer">The Color Buffer</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="Output">The Output To Give To</param>
        /// <returns>The Animation</returns>
        public static IAnimation L_Color<t>(float Time, DoubleValue<Color> ColorBuffer, bool TimeScale=true,params t[] Output)
        {
            return new ColorCA_L(Time, GetIOutput(Output), TimeScale, ColorBuffer);
        }
        /// <summary>
        ///  Create A Code Animation Which Changes The Color Of The t Through The Given Color
        /// </summary>
        /// <typeparam name="t">The Type Of The Outputer Used</typeparam>
        /// <param name="Time">The Time For The Animation</param>
        /// <param name="ColorBuffer">The Color Buffer</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="Output">The Output To Give To</param>
        /// <returns>The Animation</returns>
        public static IAnimation L_Color<t>(float Time,DoubleValue<Color>[] ColorBuffer, bool TimeScale = true, params t[] Output)
        {
            if ( ColorBuffer.Length != Output.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            else
                return new ColorCA_L(Time, GetIOutput(Output), TimeScale, ColorBuffer);
        }


        #endregion


        #region A_Color


        /// <summary>
        /// Create A Animator To Change The Color Of The Outputer Through Animation Curve
        /// </summary>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="output">The OutPuter</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color(float Time,IOutput<Color> output, AnimationCurve Curves,bool TimeScale=true,params DoubleValue<Color>[] InputColorBuffer)
        {
            
            if(InputColorBuffer.Length>1&& InputColorBuffer.Length!=output.Length)
            {
                Debug.Log("Curves And Color Buffer Are Not Of Same Length");
                return null;
            }
            return new ColorCA_A(Time, output, Curves, TimeScale, InputColorBuffer);
        }
        /// <summary>
        /// Create A Animator To Change The Color Of The Outputer Through Animation Curve
        /// </summary>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="output">The OutPuter</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color(float Time, IOutput<Color> output, AnimationCurve[] Curves, bool TimeScale=true, params DoubleValue<Color>[] InputColorBuffer)
        {

            if (InputColorBuffer.Length > 1&&(InputColorBuffer.Length != Curves.Length|| InputColorBuffer.Length != output.Length))
            {
                Debug.Log("Values Are Not Of Same Size");
                return null;
            }
            else if(Curves.Length!=output.Length)
            {
                Debug.Log("Curves And Outputer Does Not Have Equal Length");
                return null;
            }
            return new ColorCA_A(Time, output, Curves, TimeScale, InputColorBuffer);
        }


        #region T


        /// <summary>
        /// Create A Animator To Change The Color Of The Outputs Through Animation Curve
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="outputs">The Output Values To Give</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color<t>(float Time,AnimationCurve Curves,DoubleValue<Color> InputColorBuffer,bool TimeScale=true,params t[] outputs)
        {
            return new ColorCA_A(Time, GetIOutput(outputs), Curves, TimeScale, InputColorBuffer);
        }
        /// <summary>
        /// Create A Animator To Change The Color Of The Outputs Through Animation Curve
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="outputs">The Output Values To Give</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color<t>(float Time, AnimationCurve[] Curves, DoubleValue<Color> InputColorBuffer, bool TimeScale = true, params t[] outputs)
        {
            if (Curves.Length != outputs.Length)
            {
                Debug.Log("Curves And Outputer Does Not Have Equal Length");
                return null;
            }
            return new ColorCA_A(Time, GetIOutput(outputs), Curves, TimeScale, InputColorBuffer);
        }
        /// <summary>
        /// Create A Animator To Change The Color Of The Outputs Through Animation Curve
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="outputs">The Output Values To Give</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color<t>(float Time, AnimationCurve Curves, DoubleValue<Color>[] InputColorBuffer, bool TimeScale = true, params t[] outputs)
        {
            if (InputColorBuffer.Length != outputs.Length)
            {
                Debug.Log("Colors And Outputer Does Not Have Equal Length");
                return null;
            }
            return new ColorCA_A(Time, GetIOutput(outputs), Curves, TimeScale, InputColorBuffer);
        }
        /// <summary>
        /// Create A Animator To Change The Color Of The Outputs Through Animation Curve
        /// </summary>
        /// <typeparam name="t"></typeparam>
        /// <param name="Time">The TimeSpan Of The Animation</param>
        /// <param name="Curves">The Animation Curves</param>
        /// <param name="InputColorBuffer">The Input Color Buffer</param>
        /// <param name="TimeScale">The TimeScaled Used</param>
        /// <param name="outputs">The Output Values To Give</param>
        /// <returns>The Animation To Change Color</returns>
        public static IAnimation A_Color<t>(float Time, AnimationCurve[] Curves, DoubleValue<Color>[] InputColorBuffer, bool TimeScale = true, params t[] outputs)
        {
            if (InputColorBuffer.Length != Curves.Length|| InputColorBuffer.Length != outputs.Length)
            {
                Debug.Log("Values Are Not Of Same Size");
                return null;
            }
            return new ColorCA_A(Time, GetIOutput(outputs), Curves, TimeScale, InputColorBuffer);
        }


        #endregion


        #endregion


        #region T_Color


        /// <summary>
        /// Create A Animator TO Change The Value Of The Output Color By Specific RGBA
        /// </summary>
        /// <param name="Time">The Animation Length</param>
        /// <param name="outputer">The Outputer To OutPut The Value</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="AllSame">If All The Value Of The Images Is To Be Given Same</param>
        /// <param name="Update">If Value Should Be Updated </param>
        /// <param name="targets">The Targets</param>
        /// <returns>The Animation</returns>
        public static IAnimation T_Color(float Time,IOutput<Color> outputer,bool TimeScale=true,bool AllSame=false,bool Update=false,params Target[] targets)
        {
            if (targets.Length > 1 && targets.Length != outputer.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            return new TColorCA(Time, outputer, TimeScale, AllSame, Update, targets);
        }
        /// <summary>
        /// Create A Animator TO Change The Value Of The Output Color By Specific RGBA
        /// </summary>
        /// <typeparam name="t">If Supported Creates The Outputer</typeparam>
        /// <param name="Time">The Animation Length</param>
        /// <param name="targets">The Targets</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="AllSame">If All The Value Of The Images Is To Be Given Same</param>
        /// <param name="Update">If Value Should Be Updated </param>
        /// <param name="outputs">The Outputer To OutPut The Value</param>
        /// <returns>The Animation</returns>
        public static IAnimation T_Color<t>(float Time, Target[] targets, bool TimeScale=true, bool AllSame = false, bool Update = false, params t[] outputs)
        {
            if (targets.Length > 1 && targets.Length != outputs.Length)
            {
                Debug.Log("MisMatch Length");
                return null;
            }
            return new TColorCA(Time, GetIOutput(outputs), TimeScale, AllSame, Update, targets);
        }
        /// <summary>
        /// Create A Animator TO Change The Value Of The Output Color By Specific RGBA
        /// </summary>
        /// <typeparam name="t">If Supported Creates The Outputer</typeparam>
        /// <param name="Time">The Animation Length</param>
        /// <param name="targets">The Targets</param>
        /// <param name="TimeScale">The TimeScale</param>
        /// <param name="AllSame">If All The Value Of The Images Is To Be Given Same</param>
        /// <param name="Update">If Value Should Be Updated </param>
        /// <param name="outputs">The Outputer To OutPut The Value</param>
        /// <returns>The Animation</returns>
        public static IAnimation T_Color<t>(float Time, Target targets, bool TimeScale = true, bool AllSame = false, bool Update = false, params t[] outputs)
        {
            return new TColorCA(Time, GetIOutput(outputs), TimeScale, AllSame, Update, targets);
        }


        #endregion


        #endregion



        #region Evalutor

        /// <summary>
        /// Function To Create Animation Curve Evaluator
        /// </summary>
        /// <param name="MaxTime">The Max Time</param>
        /// <param name="TimeScale">The Time Scale</param>
        /// <param name="output">The Function To Give Output In</param>
        /// <param name="Curves">The Curves</param>
        /// <returns>The Animation</returns>
        public static IAnimation A_Evaluator(float MaxTime,bool TimeScale,Evaluator<float>.Output output,params AnimationCurve[] Curves)
        {
            return new Evaluator_A(MaxTime, output, TimeScale, Curves);
        }

        #endregion



        #region Restart For All The IAnimation (To Provide Visual That It Is A Function Of The IAnimation)
        #endregion


        #region Public Functions


        public static IOutput<Color> GetIOutput<t>(params t[] Data)
        {
            if (typeof(t) == typeof(Image))
                return new ImageIO((Image[])(object)Data);
            throw new Exception("No Such Type Of Outputer Available Try To Create Your Own");
        }


        #endregion
    }
}
