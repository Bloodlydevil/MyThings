
// maybe add feature for multiple sun some other time
// when you have moon (proper) then try to make its proper fall and rise
using System;
using UnityEngine;
using MyThings.ClockThings.MainClock;
using MyThings.Extension;

namespace MyThings.DayAndNight
{

    /// <summary>
    /// The Class Which Deals With Day And Night Cycle
    /// </summary>
    public class DayNightCycle : MonoBehaviour
    {
        [Header("Sun")]
        [Tooltip("the Light Used To Sumulate The Sun ( Brigtest Light In The Scene )")]
        [SerializeField] private Light _Sun;
        /// <summary>
        /// Transform Of The Sun Cashed in
        /// </summary>
        private Transform _SunTransform;


        [Header("Moon")]
        [Tooltip("The Light Used TO Simulate The Moon ")]
        [SerializeField] private Light _Moon;
        /// <summary>
        /// The Transform Of The Moon Cashed In
        /// </summary>
        private Transform _MoonTransform;


        [Header("Lighting Preset")]


        [SerializeField] private Gradient _DaySkyColor;
        [SerializeField] private Gradient _NightSkyColor;

        [SerializeField] private Gradient _DayEquatorColor;
        [SerializeField] private Gradient _NightEquatorColor;

        [SerializeField] private Gradient _DaySunColor;
        [SerializeField] private Gradient _NightSunColor;

        [SerializeField] private Gradient _DayMoonColor;
        [SerializeField] private Gradient _NightMoonColor;




        [Header("Time")]
        [Tooltip("The Clock Used TO Set Day And Night cycle")]
        [SerializeField] private Clock _Clock;





        /// <summary>
        /// The Sun Rise Time Stored In The Form Of The Time Span
        /// </summary>
        public TimeSpan SunRiseTime { get => _SunRiseTime; }
        private TimeSpan _SunRiseTime;
        /// <summary>
        /// The Sun Set Time Stored In The Form Of The Time Span
        /// </summary>
        public TimeSpan SunSetTime { get => _SunSetTime; }
        private TimeSpan _SunSetTime;
        /// <summary>
        /// The Time Span From The Sun Rise To The Sun Set
        /// </summary>
        public TimeSpan SunRiseToSunSet { get => _SunRiseToSunSet; }
        private TimeSpan _SunRiseToSunSet;
        /// <summary>
        /// The Time Span From Sun Set TO The Sun Rise
        /// </summary>
        public TimeSpan SunSetToSunRise { get => _SunSetToSunRise; }
        private TimeSpan _SunSetToSunRise;




        [Header("Testing")]
        [Tooltip("For Testing In Editor ")]
        [SerializeField, Range(0, 24)] private float DayTime;
        [Header("Day Settings")]
        [Tooltip("The SunRise Time")]
        [SerializeField] private float _SunRiseHour;
        [Tooltip("The SunSet Time")]
        [SerializeField] private float _SunSetHour;



        /// <summary>
        /// The SunRiseTime
        /// </summary>
        public float SunRiseHour { get => _SunRiseHour; set => _SunRiseHour = value; }
        /// <summary>
        /// The SunSet Time
        /// </summary>
        public float SunSetHour { get => _SunSetHour; set => _SunSetHour = value; }
        /// <summary>
        /// The Mid Day Time
        /// </summary>
        public float MidDayTime
        {
            get
            {
                int time = (int)(_SunRiseHour + _SunSetHour) / 2;
                if (_SunRiseHour > _SunSetHour)
                    time += 12;
                if (time > 24)
                    return time - 24;
                return time;
            }
        }
        /// <summary>
        ///  The Mid Night Time
        /// </summary>
        public float MidNightTime
        {
            get
            {
                int time = (int)(_SunRiseHour + _SunSetHour) / 2;
                if (_SunRiseHour < _SunSetHour)
                    time += 12;
                if (time > 24)
                    return time - 24;
                return time;
            }
        }




        #region Unity Functions


        private void Awake()
        {
            _SunTransform = _Sun.transform;
            _MoonTransform = _Moon.transform;
            _SunRiseTime = TimeSpan.FromHours(_SunRiseHour);
            _SunSetTime = TimeSpan.FromHours(_SunSetHour);
            _SunRiseToSunSet = _SunRiseTime.CalculateTimeDifference(_SunSetTime);
            _SunSetToSunRise = _SunSetTime.CalculateTimeDifference(_SunRiseTime);
        }
        private void Update()
        {
            RotateSunAndMoon(_Clock.CurrentTime);
        }
        private void OnValidate()
        {
            Awake();
            RotateSunAndMoon(DateTime.Now.Date + TimeSpan.FromHours(DayTime));
        }


        #endregion


        #region private Functions


        /// <summary>
        /// Function To Rotate Sun And Moon
        /// </summary>
        /// <param name="Time">The Current Time</param>
        private void RotateSunAndMoon(DateTime Time)
        {
            float SunRotation;

            if (IsDayTime(Time))
            {
                // day time

                float per = (float)(TimeSinceSunRise(Time).TotalMinutes / _SunRiseToSunSet.TotalMinutes);
                SunRotation = Mathf.Lerp(0, 180, per);
                UpdateDayLighting(per);
            }
            else
            {
                // night time

                float per = (float)(TimeSinceSunSet(Time).TotalMinutes / _SunSetToSunRise.TotalMinutes);
                SunRotation = Mathf.Lerp(180, 360, per);
                UpdateNightLighting(per);
            }
            float MoonRotation = SunRotation + 180;
            if (MoonRotation > 360) MoonRotation -= 360;
            _SunTransform.rotation = Quaternion.AngleAxis(SunRotation, Vector3.right);//Make Shift Moon Movement
            _MoonTransform.rotation = Quaternion.AngleAxis(MoonRotation, Vector3.right);
        }
        /// <summary>
        /// Function To Update The Lighting of the day
        /// </summary>
        /// <param name="TimeFrac">The Time Fraction (from 0 to 1)</param>
        private void UpdateDayLighting(float TimeFrac)
        {
            RenderSettings.ambientEquatorColor = _DayEquatorColor.Evaluate(TimeFrac);
            RenderSettings.ambientSkyColor = _DaySkyColor.Evaluate(TimeFrac);
            _Sun.color = _DaySunColor.Evaluate(TimeFrac);
            _Moon.color = _DayMoonColor.Evaluate(TimeFrac);
        }
        /// <summary>
        /// Function To Update The Lighting of the Night
        /// </summary>
        /// <param name="TimeFrac">The Time Fraction (from 0 to 1)</param>
        private void UpdateNightLighting(float TimeFrac)
        {
            RenderSettings.ambientEquatorColor = _NightEquatorColor.Evaluate(TimeFrac);
            RenderSettings.ambientSkyColor = _NightSkyColor.Evaluate(TimeFrac);
            _Sun.color = _NightSunColor.Evaluate(TimeFrac);
            _Moon.color = _NightMoonColor.Evaluate(TimeFrac);
        }


        #endregion


        #region Public Functions


        /// <summary>
        /// If It Is Day Time Or Not
        /// </summary>
        /// <param name="Time">The Current Time</param>
        /// <returns>If It Is Day Time Or Not</returns>
        public bool IsDayTime(DateTime Time) => Time.TimeOfDay > _SunRiseTime && Time.TimeOfDay < _SunSetTime;
        /// <summary>
        /// If It Is Night Time Or Not
        /// </summary>
        /// <param name="Time">The Current Time</param>
        /// <returns>If It Is Night Or Not</returns>
        public bool IsNightTime(DateTime Time) => Time.TimeOfDay < _SunRiseTime || Time.TimeOfDay > _SunSetTime;
        /// <summary>
        /// Function To Get The Time Since Sun Rise
        /// </summary>
        /// <param name="Time">The Current Time</param>
        /// <returns>The Time Since Sun Rise</returns>
        public TimeSpan TimeSinceSunRise(DateTime Time) => _SunRiseTime.CalculateTimeDifference(Time.TimeOfDay);
        /// <summary>
        /// Function To Get Time Since Sun Set
        /// </summary>
        /// <param name="Time">The Current Time</param>
        /// <returns>The Time Since Sun Set</returns>
        public TimeSpan TimeSinceSunSet(DateTime Time) => _SunSetTime.CalculateTimeDifference(Time.TimeOfDay);


        #endregion
    }
}