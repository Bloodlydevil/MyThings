using System;
using System.Collections.Generic;

namespace MyThings.Timer
{

    /// <summary>
    ///  A Object Which Is Used For Timer
    /// </summary>
    public interface ITimer
    {
        //Be On The Lookout For UnScaled Time Frame Bug Where After Minimising The Game The The UnScaled Time Calculates
        //It Later On And Add To The First Frame Update When Calling The 
        //Time.Unscaleddeltatime

        #region Values


        /// <summary>
        /// The Current Time Moves From 0 To Max Time
        /// </summary>
        public float Time { get; set; }
        /// <summary>
        /// The Max Time
        /// </summary>
        public float MaxTime { get; set; }
        /// <summary>
        /// If The Timer Is Running Or Not
        /// </summary>
        public bool Running { get; }
        /// <summary>
        /// If The Repeat Mode Is On Or Not
        /// </summary>
        public bool Repeat { get; set; }
        /// <summary>
        /// If Time Is Scaled Or Not
        /// </summary>
        public bool Scaled { get; set; }
        /// <summary>
        /// The Connected Timers
        /// </summary>
        public List<ITimer> ConnectedTimer { get; }


        #endregion


        #region Functions


        /// <summary>
        /// Reset The Time To Start From Start
        /// </summary>
        public void ReSetTime();
        /// <summary>
        /// Stop The Timer By Removing It From The Updating List
        /// </summary>
        /// <param name="End">Call On End</param>
        public void Stop(bool End = false);
        /// <summary>
        /// A Function To Reset The timer With New Values
        /// </summary>
        /// <param name="Time">The Time At Which To End Or Repeat</param>
        /// <param name="action">The Function</param>
        /// <param name="Scaled">If Time Should Be Scaled</param>
        /// <param name="Repeat"> If Timer Should Be repeated</param>
        public void Reset(float Time, Action action, bool Scaled = true, bool Repeat = false);
        /// <summary>
        /// A Funciion To Connect A Timer With another Like Links
        /// </summary>
        /// <param name="timer">The New Timer To Be Connected To Old</param>
        /// <returns>The Old Timer</returns>
        public ITimer SetConnectedTimer(ITimer timer);
        /// <summary>
        /// Function TO Remove The Connected Timer
        /// </summary>
        public void RemoveConnectedTimer();
        /// <summary>
        /// Function TO Remove The Connected Timer from Active Update List
        /// </summary>
        public void RemoveConnectedTimerUL();
        /// <summary>
        /// Function To Start The Timer
        /// </summary>
        /// <param name="Zero">Set The Timer To Zero</param>
        public void Start(bool Zero = true);
        /// <summary>
        /// Method To Add Timer To Another Timer
        /// </summary>
        /// <param name="MaxTimer">The Time At Which To Repeate</param>
        /// <param name="action">The Function</param>
        /// <param name="Scale">The Scale For The Time</param>
        /// <returns>The Old Timer</returns>
        public ITimer Add(float MaxTimer, Action action, bool Scale = true);
        /// <summary>
        /// Set The OnEnd Function
        /// </summary>
        /// <param name="funtion">The Function</param>
        public void SetOnEnd(Action funtion);
        /// <summary>
        /// Add A Function To The OnEnd
        /// </summary>
        /// <param name="funtion">The Function</param>
        public void AddOnEnd(Action funtion);
        /// <summary>
        /// Remove A Function From The OnEnd
        /// </summary>
        /// <param name="function">The Function</param>
        public void RemoveOnEnd(Action function);


        #endregion
    }
}