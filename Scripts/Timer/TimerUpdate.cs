using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyThings.Timer
{

    /// <summary>
    /// A Object Which Is Used For Timer ( It Is Using Update Of THe Unity To Do The Work And Is Non Thread Work Flow (Change It LAter On If Slow))
    /// </summary>
    public class TimerUpdate : ITimer
    {
        #region Values

        /// <summary>
        /// If The Action Is repeating
        /// </summary>
        protected bool _Repeat;

        /// <summary>
        /// If The Timer Is Running
        /// </summary>
        protected bool _Running;

        /// <summary>
        /// The Timer Used For Time Calculate
        /// </summary>
        protected float _Timer;

        /// <summary>
        /// The Max Timer For Repeating
        /// </summary>
        protected float _MaxTimer;

        /// <summary>
        /// The Function To Call On Timer Finishes
        /// </summary>
        protected Action OnEnd;

        /// <summary>
        /// If The Time Should be Scaled Or Not
        /// </summary>
        public bool _TimeScaled;

        /// <summary>
        /// Connected Timer To This TImer
        /// </summary>
        private List<ITimer> _ConnectedTimer;

        public float Time
        {
            get => _Timer; set
            {
                _Timer = value;
                if (_Timer > _MaxTimer) _Timer = _MaxTimer;
            }
        }
        public bool Running { get => _Running; }
        public List<ITimer> ConnectedTimer => _ConnectedTimer;
        public virtual float MaxTime { get => _MaxTimer; set => _MaxTimer = value; }
        public bool Repeat { get => _Repeat; set => _Repeat = value; }
        public bool Scaled { get => _TimeScaled; set => _TimeScaled = value; }


        #endregion


        /// <summary>
        /// Constructor To Create The Timer
        /// </summary>
        /// <param name="End">The Function</param>
        /// <param name="Repeat">If It Is Repeating</param>
        /// <param name="MaxTimer">The Time To Repeat In</param>
        /// <param name="Scaled">If The Time Should Be Scaled</param>
        internal TimerUpdate(Action End, float MaxTimer, bool Repeat, bool Scaled)
        {
            _Repeat = Repeat;
            _Running = false;
            _MaxTimer = MaxTimer;
            _Timer = 0;
            OnEnd = End;
            _TimeScaled = Scaled;
        }
        internal TimerUpdate() { }

        /// <summary>
        /// The Function Called On Update To Update The Timer
        /// </summary>
        /// <param name="time">The Time With Which To Update</param>
        /// <returns>If Timer has Finished (The Repeat Is Off ) Or Not (On Repeat)</returns>
        internal virtual bool Update(float time)
        {
            if (!_Running)
                return false;
            if (_Timer < _MaxTimer)
            {
                _Timer += time;
                return false;
            }
            else
            {
                OnEnd();
                if (_Repeat)
                {
                    _Timer -= _MaxTimer;
                    return false;
                }
                _Timer = 0;
                return true;
            }
        }


        #region Interface Functions


        public void ReSetTime()
        {
            _Timer = 0;
        }
        public void Stop(bool End = false)
        {
            if (!_Running)
                return;
            if (End) OnEnd();
            _Running = false;
            if (TimerManager.Instance != null)
                TimerManager.Instance.AddToDelete(this);
            RemoveConnectedTimerUL();
        }
        public void Reset(float Time, Action action, bool Scaled = true, bool Repeat = false)
        {
            _Timer = Time;
            _Running = false;
            OnEnd = action;
            _TimeScaled = Scaled;
            _Repeat = Repeat;
        }
        public ITimer SetConnectedTimer(ITimer timer)
        {
            _ConnectedTimer ??= new List<ITimer>();
            if (!_ConnectedTimer.Contains(timer))
                _ConnectedTimer.Add(timer);
            return this;
        }
        public void RemoveConnectedTimer()
        {
            RemoveConnectedTimerUL();
            _ConnectedTimer?.Clear();
        }
        public void RemoveConnectedTimerUL()
        {
            if (_ConnectedTimer != null)
            {
                for (int i = 0; i < _ConnectedTimer.Count; i++)
                {
                    TimerManager.Instance.AddToDelete((TimerUpdate)_ConnectedTimer[i]);
                }
            }
        }
        public void Start(bool Zero = true)
        {
            if (Zero) _Timer = 0;
            if (_Running) return;
            _Running = true;
            TimerManager.Instance.AddToActive(this);
            if (_ConnectedTimer != null)
                for (int i = 0; i < _ConnectedTimer.Count; i++)
                {
                    _ConnectedTimer[i].Start();
                }
        }
        public ITimer Add(float MaxTimer, Action action, bool Scale = true)
        {
            return SetConnectedTimer(TimerManager.Create(MaxTimer, action, true, Scale));
        }
        public void SetOnEnd(Action funtion) => OnEnd = funtion;
        public void AddOnEnd(Action funtion) => OnEnd += funtion;
        public void RemoveOnEnd(Action function) => OnEnd -= function;


        #endregion
    }
}