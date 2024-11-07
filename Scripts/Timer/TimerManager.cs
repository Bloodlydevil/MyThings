using MyThings.Data;
using MyThings.ExtendableClass;
using System;
using UnityEngine;

namespace MyThings.Timer
{

    /// <summary>
    /// The Class Which Can Be Used To Set A Timer
    /// </summary>
    [DefaultExecutionOrder(-2)]
    public class TimerManager : Singleton_C<TimerManager>
    {
        [SerializeField] private int TotalTimersRunning;
        /// <summary>
        /// The List For All The Timers In The Game
        /// </summary>
        private ListD<TimerUpdate> Timers=new ListD<TimerUpdate>();


        #region Unity Functions


        private void Update()
        {
            float time = Time.deltaTime;
            float Timee = Time.unscaledDeltaTime;
            Timers.SyncRemove();
            TotalTimersRunning = Timers.L.Count;
            for (int i = 0; i < TotalTimersRunning; ++i)
            {
                var t = Timers.L[i];
                if (t.Update(t._TimeScaled ? time : Timee))
                {
                    t.Stop();
                }
            }
        }

        #endregion


        #region Static Functions

        /// <summary>
        /// The Function To Create A Timer
        /// </summary>
        /// <param name="MaxTimer">The Time Period</param>
        /// <param name="action">The Function</param>
        /// <param name="Repeating">If The Timer Is Repeating</param>
        /// <param name="Scale">If The Time Is Scaled</param>
        /// <returns>Timer Object To Call The Important Functions</returns>
        public static ITimer Create(float MaxTimer, Action action, bool Repeating = false, bool Scale = true)
        {
            return new TimerUpdate(action, MaxTimer, Repeating, Scale);
        }
        /// <summary>
        /// The Function To Create A Random Timer
        /// </summary>
        /// <param name="action">The Function</param>
        /// <param name="MaxTime">The Max Time For Random Time</param>
        /// <param name="Repeating">If The Timer Is Repeating Or Not</param>
        /// <param name="Scale">If The Time Should Be Scaled Or Not</param>
        /// <returns>Timer Object To Call The Important Functions</returns>
        public static ITimer CreateContRandom(Action action, float MaxTime, bool Repeating = false, bool Scale = true)
        {
            return new TimerUpdate(action, UnityEngine.Random.Range(0, MaxTime), Repeating, Scale);
        }
        /// <summary>
        /// Create A count down timer from the time
        /// </summary>
        /// <param name="MaxTime">Max Time</param>
        /// <param name="Tick">The Function to call each sec</param>
        /// <param name="End">the Function to call in the end</param>
        /// <param name="Scale">the scale to use</param>
        /// <returns>Timer Object</returns>
        public static ITimer CreateCountDown(int MaxTime, Action<int> Tick, Action End, bool Scale = true)
        {
            return new TimerCountdown(MaxTime, Tick, End, Scale);
        }
        /// <summary>
        /// Create A Update with Custom FrameRate(Only Slower Allowed)
        /// </summary>
        /// <param name="FrameRate">The FrameRate</param>
        /// <param name="action">The Function To Call</param>
        /// <param name="Scale">The Scale</param>
        /// <returns></returns>
        public static ITimer CreateFrame(int FrameRate, Action action, bool Scale = true)
        {
            return new TimerUpdate(action,1.0f/FrameRate, true, Scale);
        }
        /// <summary>
        /// Function To Add Timer To Update List
        /// </summary>
        /// <param name="timer">This Timer</param>
        public void AddToActive(TimerUpdate timer) => Timers.Add(timer);
        /// <summary>
        /// A Function To Add The Timer To The Delete List
        /// </summary>
        /// <param name="timer">The Timer</param>
        public void AddToDelete(TimerUpdate timer) 
        {
            Timers.Remove(timer);
        }


        #endregion
    }
}