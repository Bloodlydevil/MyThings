using System;
using UnityEngine;
using MyThings.Timer;

namespace MyThings.ClockThings.MainClock
{

    /// <summary>
    /// The Class which acts as clock
    /// </summary>
    public class Clock : MonoBehaviour
    {
        /// <summary>
        /// The Time Speed
        /// </summary>
        [Tooltip("The Time Speed")]
        [SerializeField] private float _TimeMultiplier;

        /// <summary>
        /// If The Time is Scaled Or Not
        /// </summary>
        [Tooltip("If The Time Is Scaled Or Not")]
        [SerializeField] private bool _TimeScale = true;


        [Header("Start")]
        [SerializeField, Range(0, 24)] private float StartHour;
        [SerializeField, Range(0, 60)] private float StartMinute;
        [SerializeField, Range(0, 60)] private float StartSecond;
        [SerializeField, Range(0, 1000)] private float StartMillisecond;



        /// <summary>
        /// The Date Time Used To Simulate clock
        /// </summary>
        private DateTime _Time;
        /// <summary>
        /// The function Used To Update
        /// </summary>
        private Action UpdateTime;
        /// <summary>
        /// The Current Time 
        /// </summary>
        public DateTime CurrentTime { get { return _Time; } }


        #region Unity Functions


        private void Awake()
        {
            _Time = DateTime.Now.Date + TimeSpan.FromHours(StartHour) + TimeSpan.FromMinutes(StartMinute) + TimeSpan.FromSeconds(StartSecond) + TimeSpan.FromMilliseconds(StartMillisecond);

            UpdateTime = getTimeUpdater(_TimeScale);
        }
        private void Update()
        {
            UpdateTime();
        }


        #endregion


        #region Private Functions


        /// <summary>
        /// Function To Get The Time Updater
        /// </summary>
        /// <param name="TimeScale">If The Time Is Scaled Or Not</param>
        /// <returns>The Function Which Updates The Time</returns>
        private Action getTimeUpdater(bool TimeScale)
        {
            if (TimeScale)
                return ScaledTime;
            return UnScaledTime;
        }
        /// <summary>
        /// Function To Call The Time Update For Scaled Time
        /// (It Might Have Some Function OverHead So Be On The LookOut)
        /// </summary>
        private void ScaledTime() => _Time = _Time.AddSeconds(_TimeMultiplier * Time.deltaTime);
        /// <summary>
        /// Function To Call The Time Update For UnScaled Time
        /// (It Might Have Some Function OverHead So Be On The LookOut)
        /// </summary>
        private void UnScaledTime() => _Time = _Time.AddSeconds(_TimeMultiplier * Time.unscaledDeltaTime);


        #endregion


        #region Public Functions


        /// <summary>
        /// Function To Change The Scale Mode Of The Time
        /// </summary>
        /// <param name="TimeScale">If Time Is scaled Or Not</param>
        public void ChangeTimeScale(bool TimeScale) => UpdateTime = getTimeUpdater(TimeScale);
        /// <summary>
        /// Function To Get A Timer For The Given Time In The clock Speed
        /// </summary>
        /// <param name="Time">The Time</param>
        /// <param name="Function">The Function</param>
        /// <param name="Repeating">If The Timer Is Repeating</param>
        /// <param name="Scale">If The Timer Is Scaled</param>
        /// <returns>The Timer In The clock Speed</returns>
        public ITimer GetTimerFor(TimeSpan Time, Action Function, bool Repeating = false, bool Scale = true) => TimerManager.Create((float)Time.TotalSeconds / _TimeMultiplier, Function, Repeating, Scale);


        #endregion
    }
}