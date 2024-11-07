using System;
using UnityEngine;
using MyThings.ClockThings.MainClock;
namespace MyThings.DayAndNight
{
    /// <summary>
    /// A Class Which Deals With Day and Night Events
    /// </summary>
    public class DayNightEvents : MonoBehaviour
    {
        [Tooltip("Referance Of The Day Night Cycle")]
        [SerializeField] private DayNightCycle DNC;

        /// <summary>
        /// An Event Which Is Called When Sun Rises
        /// </summary>
        public event Action OnSunRise = () => { Debug.Log("SunRise"); };
        /// <summary>
        /// An Event Which Is Called When Sun Sets
        /// </summary>
        public event Action OnSunSet = () => { Debug.Log("SunSet"); };
        /// <summary>
        /// An Event Which Is Called When Sun Is At Its Peak Point
        /// </summary>
        public event Action OnMidDay = () => { Debug.Log("Noon"); };
        /// <summary>
        /// An Event Which Is Called When Moon Is At Its Peak point
        /// </summary>
        public event Action OnMidNight = () => { Debug.Log("MidNight"); };

        private void Start()
        {
            // it Uses Clock Event System To Call The Events 
            ClockEventManager.Instance.Subscribe(TimeSpan.FromHours(DNC.SunRiseHour), () => OnSunRise?.Invoke());
            ClockEventManager.Instance.Subscribe(TimeSpan.FromHours(DNC.SunSetHour), () => OnSunSet?.Invoke());
            ClockEventManager.Instance.Subscribe(TimeSpan.FromHours(DNC.MidDayTime), () => OnMidDay?.Invoke());
            ClockEventManager.Instance.Subscribe(TimeSpan.FromHours(DNC.MidNightTime), () => OnMidNight?.Invoke());
        }
    }
}