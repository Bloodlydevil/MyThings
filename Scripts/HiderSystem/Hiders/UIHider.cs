using System;
using UnityEngine.UI;
using MyThings.CAnimation;
using MyThings.Timer;
using UnityEngine;
using MyThings.CAnimation.Outputs;

namespace MyThings.HiderSystem.Hiders
{

    /// <summary>
    /// This Hider Hides The UI Element With Some Animations
    /// </summary>
    public class UIHider : IHider
    {
        /// <summary>
        /// The Timer Used To Hide 
        /// </summary>
        private ITimer _Timer;
        /// <summary>
        /// The Animation To Show The Images
        /// </summary>
        private IAnimation _AnimatorShow;
        /// <summary>
        /// The Animation To Hide The Images
        /// </summary>
        private IAnimation _AnimatorHide;

        public ITimer Timer { get => _Timer; set => _Timer = value; }

        /// <summary>
        /// A Constructor To Create UIHider To Hide UI Image While Reducing Alpha
        /// </summary>
        /// <param name="Time">The Time Taken To Automaticaly Hide The Images</param>
        /// <param name="TimeShow">The Animation Length To Show The Images From State Of Hiden</param>
        /// <param name="TimeHide">The Animation Length To Hide The Images From State Of Shown</param>
        /// <param name="Scale">The Time Scale</param>
        /// <param name="Same">If All The Colors Of The Images Are Same Then This Should Be Made True To Save Computation Time And Power</param>
        /// <param name="Update">Make True If The Color Of THe Images Can Be Affected By Other Things(Images Color Will Be Taken Input Each Frame)</param>
        /// <param name="images">The Images To Apply Hide To</param>
        public UIHider(float Time,float TimeShow=1,float TimeHide=1,bool Scale=true,bool Same=false,bool Update=false,params Image[] images)
        {
            _Timer = TimerManager.Create(Time, Hide, Scale: Scale);
            _AnimatorShow = CAnimator.T_Color(TimeShow, new ImageIO(images), Scale, Same, Update, new Data.Target(a: 1));
            _AnimatorHide = CAnimator.T_Color(TimeHide, new ImageIO(images), Scale, Same, Update, new Data.Target(a: 0));
        }

        /// <summary>
        /// Function TO Hide The UI
        /// </summary>
        public void Hide()
        {
            _AnimatorShow.Stop();
            _AnimatorHide.Start();
        }

        #region Interface Functions


        public void AddListenerForOnHide(Action action) => _AnimatorHide.AddCallBackOnFinish(action);
        public void ForceHide()
        {
            _Timer.Stop();
            _AnimatorHide.ForceEnd();
            _AnimatorShow.Stop();
        }
        public void End()
        {
            _Timer.Stop();
            _AnimatorHide.Stop();
            _AnimatorShow.Stop();
        }
        public void Show()
        {
            ShowOnly();
            _Timer.Start();
        }
        public void ShowOnly()
        {
            _AnimatorHide.Stop();
            _AnimatorShow.Start();
        }
        public void UnSubscribe()
        {
            _Timer.Stop();
            _AnimatorShow.Stop();
            _AnimatorHide.Stop();
        }


        #endregion
    }
}