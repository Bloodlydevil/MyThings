using System;

namespace MyThings.Job_System
{ 
    /// <summary>
    /// A Simple Class To Call A Function To Change Something (It Gives A Value Which Starts From 0 Till 1 )
    /// </summary>
    public class Completion
    {
        #region Values
        /// <summary>
        /// The Current Value
        /// </summary>
        private float _CurrentValue;
        /// <summary>
        /// The Max value The Value can go
        /// </summary>
        private float _MaxValue;
        /// <summary>
        /// The Differance from min Till max
        /// </summary>
        private float _Differance;


        /// <summary>
        /// The Job Used To Get The Update
        /// </summary> 
        private IJob _Job;
        /// <summary>
        /// Function(s) To Call On the End
        /// </summary>
        private Action _OnEnd;
        /// <summary>
        /// The Function To Call On Update
        /// </summary>
        private Action<float> _OnUpdate;
        #endregion

        #region  Properties
        /// <summary>
        /// The Current Value
        /// </summary>
        public float CurrentValue { get => _CurrentValue; set => _CurrentValue = value; }

        /// <summary>
        /// The Max Value This Change Can Go
        /// </summary>
        public float MaxVaue
        {
            get => _MaxValue;
            set
            {
                _Differance += value - _MaxValue;
                _MaxValue = value;
            }
        }

        /// <summary>
        /// The Min Value 
        /// </summary>
        public float MinValue { get => _MaxValue - _Differance; set => _Differance = _MaxValue - value; }
        /// <summary>
        /// The TimeScale Of The Change
        /// </summary>
        public bool TimeScale { get => _Job.TimeScaled;set=> _Job.TimeScaled = value; }


        /// <summary>
        /// If The Change Is Running
        /// </summary>
        /// <returns>Change Running</returns>
        public bool IsRunning => _Job.Running;

        #endregion


        #region Constructors


        /// <summary>
        /// Constructor To Create Change (It will Move with the speed of Time.delta time and go till maxtime)
        /// </summary>
        /// <param name="MaxValue">The MaxValue It Can Go</param>
        /// <param name="MinValue">The Min Value It Can Go</param>
        /// <param name="OnUpdate">The Function Which Will Recieve Value from 0 till 1 ( 0 means The Start And 1 Means The End )</param>
        /// <param name="TimeScale">The Time Should Be Scaled Or Not</param>
        public Completion(float MaxValue,Action<float> OnUpdate, bool TimeScale=true, float MinValue = 0)
        {
            _CurrentValue = MinValue;
            _MaxValue=MaxValue;
            _Differance = MaxValue-MinValue;
            _OnUpdate = OnUpdate;
            _Job = JobSystem.CreateJob(Update, TimeScale);
        }
        /// <summary>
        /// Constructor To Create Change
        /// </summary>
        /// <param name="CurrentValue">The Current Value</param>
        /// <param name="MaxValue">The MaxValue It Can Go</param>
        /// <param name="MinValue">The Min Value It Can Go</param>
        /// <param name="OnUpdate">The Function Which Will Recieve Value from 0 till 1 ( 0 means The Start And 1 Means The End)</param>
        /// <param name="TimeScale">The Time Should Be Scaled Or Not</param>
        public Completion(float CurrentValue, float MaxValue,Action<float> OnUpdate, bool TimeScale= true,float MinValue = 0)
        {
            _CurrentValue = CurrentValue;
            _MaxValue = MaxValue;
            _Differance = MaxValue - MinValue;
            _OnUpdate = OnUpdate;
            _Job = JobSystem.CreateJob(Update, TimeScale);
        }


        #endregion


        #region Private Functions


        /// <summary>
        /// Function To Get The Update
        /// </summary>
        /// <param name="time">The delta Time</param>
        private void Update(float time)
        {
            _CurrentValue += time;
            if (_CurrentValue > _MaxValue)
            {
                _CurrentValue = _MaxValue;
                _Job.Stop();
                _OnEnd?.Invoke();
                return;
            }
            _OnUpdate(_CurrentValue/_Differance);
        }


        #endregion


        #region Public Functions


        /// <summary>
        /// Function To Start The Change (Current is set to min)
        /// </summary>
        public void Start()
        {
            _CurrentValue = _MaxValue - _Differance;
            _Job.Start();
        }

        /// <summary>
        /// Function To Resume The Change (Current is not changed )
        /// </summary>
        public void Resume()
        {
            _Job.Start();
        }

        /// <summary>
        /// Function To Stop The Change
        /// </summary>
        public void Stop()
        {
            _Job.Stop();
        }

        /// <summary>
        /// Function To End The Change As It Would Do
        /// </summary>
        public void ForceEnd()
        {
            _CurrentValue = _MaxValue;
            _Job.Stop();
            _OnUpdate(1);
            _OnEnd?.Invoke();
        }

        /// <summary>
        /// Function To Set Max value (Current Value Changes To Equate The Progress On The New Value)
        /// </summary>
        /// <param name="value">The Maximum Value Of The Range</param>
        public void SetMaxValue(float value)
        {
            _CurrentValue = Functions.Convert(_CurrentValue, _MaxValue - _Differance, _MaxValue, _MaxValue - _Differance, value);
            _Differance += value - _MaxValue;
            _MaxValue = value;
        }

        /// <summary>
        /// Function To Set Min value (Current Value Changes To Equate The Progress On The New Value)
        /// </summary>
        /// <param name="value">The Minimum Value Of The Range</param>
        public void SetMinValue(float value)
        {
            _CurrentValue = Functions.Convert(_CurrentValue, _MaxValue - _Differance, _MaxValue, value, _MaxValue);
            _Differance = _MaxValue - value;
        }


        /// <summary>
        /// Function To Add a On End Event
        /// </summary>
        /// <param name="action">The Function To Call On End Of The Change</param>
        public void AddOnEnd(Action action) => _OnEnd += action;
        /// <summary>
        /// Function To Remove An Added OnEnd Event
        /// </summary>
        /// <param name="action">The Function To Remove From On End Event</param>
        public void RemoveOnEnd(Action action) => _OnEnd -= action;
        /// <summary>
        /// Function To Set the On End Callback
        /// </summary>
        /// <param name="action">The Function To Set On end</param>
        public void SetOnEnd(Action action) => _OnEnd = action;

        #endregion
    }
}