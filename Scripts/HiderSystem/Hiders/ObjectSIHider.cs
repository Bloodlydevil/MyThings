using System;
using UnityEngine;
using MyThings.Timer;

namespace MyThings.HiderSystem.Hiders
{

    /// <summary>
    /// This Hider Hides Only Object
    /// </summary>
    public class ObjectSIHider : IHider
    {
        /// <summary>
        /// The Object To Hide
        /// </summary>
        private GameObject[] _Object;
        /// <summary>
        /// The Timer For The UIHider
        /// </summary>
        private ITimer _Timer;
        public ITimer Timer { get => _Timer; set => _Timer = value; }


        /// <summary>
        /// A Constructor To Create The ObjectTo Hide IHider
        /// </summary>
        /// <param name="Time">Thr Time It Takes To Hide</param>
        /// <param name="Scale">If The Time Is Scale</param>
        /// <param name="Object">The Objects TO Hide And Show</param>
        public ObjectSIHider(float Time, bool Scale, GameObject[] Object)
        {
            _Object = Object;
            _Timer = TimerManager.Create(Time, Hide, Scale);
        }


        /// <summary>
        /// Hide The Objects At The End Or When Forced Hide
        /// </summary>
        public void Hide()
        {
            for (int i = 0; i < _Object.Length; i++)
            {
                _Object[i].SetActive(false);
            }
        }


        #region Interface Functions


        public void UnSubscribe()
        {
            _Object = null;
            _Timer.Stop();
        }
        public void Show()
        {
            ShowOnly();
            _Timer.ReSetTime();
            _Timer.Start();
        }
        public void ForceHide()
        {
            _Timer.Stop();
            Hide();
        }
        public void End()
        {
            _Timer.Stop();
        }
        public void ShowOnly()
        {
            for (int i = 0; i < _Object.Length; i++)
            {
                _Object[i].SetActive(true);
            }
        }
        public void AddListenerForOnHide(Action action)
        {
            _Timer.AddOnEnd(action);
        }


        #endregion
    }
}