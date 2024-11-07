using TMPro;
using UnityEngine;
using MyThings.Extension;
using MyThings.Job_System;
namespace MyThings.TextPopUpSystem
{

    public class Text_Pop : MonoBehaviour, IText_Pop
    {
        [Tooltip("The Text Used")]
        [SerializeField] private TextMeshProUGUI Text;
        [Tooltip("The way In Which Color Must Change")]
        [SerializeField] private AnimationCurve _Color;
        [Tooltip("The way In Which Scale Must Change")]
        [SerializeField] private AnimationCurve _Scale;
        [Tooltip("The default Max Amount Of Time The Text Will Stay On The Screen")]
        [SerializeField] private float Max_timer;
        //private IAnimation Animate;
        /// <summary>
        /// The Changer To Call The Function
        /// </summary>
        private Completion _Change;
        /// <summary>
        /// The Default Scale
        /// </summary>
        private Vector3 _DefaultScale;
        /// <summary>
        /// The Transform To Change The Scale Of
        /// </summary>
        private Transform _CurrentTransform;




        private void Awake()
        {
            _CurrentTransform = transform;
            _DefaultScale = _CurrentTransform.localScale;
            _Change = new Completion(0, Updat);
            //Animate = CAnimator.A_Evaluator(Max_timer, true, (float[] a) => { Text.SetColorA(a[0]); _CurrentTransform.localScale = _DefaultScale * a[1]; }, _Color, _Scale);
            _Change.AddOnEnd(() => Text_PopUpSystem.UnPopUp(this));
        }


        #region Private

        /// <summary>
        /// The Update Which Is Called By Change
        /// </summary>
        /// <param name="a">The Progress</param>
        private void Updat(float a)
        {
                Text.SetColorA(_Color.Evaluate(a));// could be chnaged to A_Colors CA but no need
                _CurrentTransform.localScale = _DefaultScale * _Scale.Evaluate(a);
        }
        /// <summary>
        /// Se The Text Of The Pop Up
        /// </summary>
        /// <param name="text">The Text</param>
        /// <param name="MaxTimer">The Time For Which It Stays</param>
        /// <param name="TimeScale">If time is Scaled </param>
        public void SetText(string text, float MaxTimer = -1, bool TimeScale = true)
        {
            Text.text = text;
            gameObject.SetActive(true);
            _Change.TimeScale = TimeScale;
            if (MaxTimer == -1)
            {
                _Change.MaxVaue = Max_timer;
            }
            else
                _Change.MaxVaue = MaxTimer;
            _Change.Start();
        }

        #endregion

    }
}