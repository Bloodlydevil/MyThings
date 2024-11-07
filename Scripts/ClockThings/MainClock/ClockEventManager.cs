// Corrently This Is Quite A Mess And Fixed On Clock Event

// New Day Is Based On The Day Of The Year So Not So Accurate

using MyThings.ExtendableClass;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.ClockThings.MainClock
{
    /// <summary>
    /// This Is The Manager Which Uses Clock To Call Event Or Delegates At Specified Time 
    /// <para>The Clock Must Be Set Up Before Hand So It Cant Be Singleton_D</para>
    /// And Clock Should Be Only Inside Game Not Live EveryWhere
    /// </summary>
    [DefaultExecutionOrder(-1)]
    [RequireComponent(typeof(Clock))]
    public class ClockEventManager : Singleton<ClockEventManager>
    {
        /// <summary>
        /// Clock Used To Deal With Everything
        /// </summary>
        private Clock C;
        /// <summary>
        /// The Clock Event Running
        /// </summary>
        private List<ClockEventDT> _ClockEvents = new();
        /// <summary>
        /// The Clock Event Required To Remove
        /// </summary>
        private List<ClockEventDT> _RemoveClockEvent = new();
        /// <summary>
        ///  True When There Is Any CLlock Event To Remove
        /// </summary>
        private bool Remove = false;
        /// <summary>
        ///  Current Day Of The Year ( Used To Know If The Day Has Change Or Not )
        /// </summary>
        private int Day = 0;


        #region Unity Functions


        protected override void Awake()
        {
            base.Awake();
            C = GetComponent<Clock>();
        }
        private void Start()
        {
            Day = C.CurrentTime.DayOfYear;
        }
        private void Update()
        {
            DateTime Time = C.CurrentTime;
            if (Day != Time.DayOfYear)
            {
                ReSetEvents();
                Day = Time.DayOfYear;
            }
            for (int i = 0; i < _ClockEvents.Count; i++)
            {
                if (_ClockEvents[i].Check)
                {
                    if (_ClockEvents[i].Time < Time.TimeOfDay)
                    {
                        _ClockEvents[i].OnTimePass();
                        _ClockEvents[i].Check = false;
                    }
                }
            }
            if (Remove)
            {
                for (int i = 0; i < _RemoveClockEvent.Count; i++)
                {
                    _ClockEvents.Remove(_RemoveClockEvent[i]);
                }
                Remove = false;
            }
        }


        #endregion


        #region private Function


        /// <summary>
        /// Function Used To Re Set All The Clock Events To Again Check For Them From The Next Day
        /// </summary>
        private void ReSetEvents()
        {
            int len = _ClockEvents.Count;
            for (int i = 0; i < len; i++)
                _ClockEvents[i].Check = true;
        }


        #endregion


        #region Public Function


        /// <summary>
        /// Function Used To Subscribe To An Event Based On Its Time ( Same Time Events Are Called At The Same Time )
        /// </summary>
        /// <param name="time">The Time At Which To Call The Event</param>
        /// <param name="Function">The Function To Call At The Time Given</param>
        public void Subscribe(TimeSpan time, Action Function)
        {
            int len = _ClockEvents.Count;
            for (int i = 0; i < len; i++)
            {
                if (_ClockEvents[i].Time == time)
                {
                    _ClockEvents[i].OnTimePass += Function;
                    return;
                }
            }
            _ClockEvents.Add(new ClockEventDT(Function, time));
        }
        /// <summary>
        /// Function Used To UnSubscribe From This Event System
        /// </summary>
        /// <param name="time">The time Given At The Time Of Subscribing</param>
        /// <param name="Function">The Function Given At The Time Of Subscribing</param>
        public void UnSubscribe(TimeSpan time, Action Function)
        {
            int len = _ClockEvents.Count;
            for (int i = 0; i < len; i++)
            {
                if (_ClockEvents[i].Time == time)
                {
                    _ClockEvents[i].OnTimePass -= Function;
                    if (_ClockEvents[i].OnTimePass == null)
                    {
                        _RemoveClockEvent.Add(_ClockEvents[i]);
                        Remove = true;
                    }
                    return;
                }
            }
        }


        #endregion


        #region IClockEvent

        /// <summary>
        /// A Private Class Which Is Used To Create a Time Event Which Uses TimeSpan To Store The Time
        /// </summary>
        private class ClockEventDT : IClockEvent
        {
            /// <summary>
            /// The Function(s) To Call When Time Is up
            /// </summary>
            public Action OnTimePass;
            /// <summary>
            /// The Time At Which To Call The Function(s)
            /// </summary>
            public TimeSpan Time;
            /// <summary>
            /// Is The Event Called in This Day
            /// </summary>
            public bool Check = true;


            /// <summary>
            /// A Constructor used To Crate An Clock Event ( Must Be Only Created Only When New Time Is Given )
            /// </summary>
            /// <param name="onTimePass">The Function(s) To Call When Time Is Up</param>
            /// <param name="time">The Time At Which The Function(s) Are Supposed To be Called</param>
            public ClockEventDT(Action onTimePass, TimeSpan time)
            {
                OnTimePass = onTimePass;
                Time = time;
            }
            public void RemoveEvent(Action Function)
            {
                OnTimePass -= Function;
                if (OnTimePass == null)
                {
                    _instance._RemoveClockEvent.Add(this);
                    _instance.Remove = true;
                }
            }
        }


        #endregion
    }
}
