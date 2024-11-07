using System;

namespace MyThings.Timer
{
    /// <summary>
    /// A timer To countdown from max to zero
    /// </summary>
    public class TimerCountdown : TimerUpdate
    {
        /// <summary>
        /// The max Time
        /// </summary>
        private int m_Max;
        /// <summary>
        /// the function to call after each sec
        /// </summary>
        private Action<int> m_Function;
        /// <summary>
        /// the Max Time
        /// </summary>
        public override float MaxTime { get => m_Max; set => m_Max = (int)value; }
        /// <summary>
        /// Create Timer Countdown
        /// </summary>
        /// <param name="max">The Max Time</param>
        /// <param name="Function">The Function To Call Each Sec</param>
        /// <param name="End">The Function To Call At the End</param>
        /// <param name="Scaled">If Time Is Scaled Or Not</param>
        public TimerCountdown(int max, Action<int> Function, Action End, bool Scaled)
        {
            m_Max = max;
            m_Function = Function;
            _Repeat = Repeat;
            _MaxTimer = 1;
            _Timer = 0;
            OnEnd = End;
            _TimeScaled = Scaled;
        }
        internal override bool Update(float time)
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
                m_Max--;
                if(m_Max>0)
                m_Function(m_Max);
                else
                {
                    _Timer = 0;
                    OnEnd();
                    return true;
                }
                _Timer -= _MaxTimer;
                return false;
            }
        }
    }
}