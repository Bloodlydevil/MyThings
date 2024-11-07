using System;

namespace MyThings.Job_System
{

    /// <summary>
    /// The Jobs Which We May Have
    /// </summary>
    public class Job: IJob
    {
        
        private bool m_TimeScaled;
        private Action<float> ToPerForm;
        private bool JobPerforming;
        public bool Running { get => JobPerforming; }
        public bool TimeScaled { get => m_TimeScaled; set => m_TimeScaled = value; }

        /// <summary>
        /// Constructor To Create A Job
        /// </summary>
        /// <param name="action">The Function To Call The Job</param>
        /// <param name="Time">If THe Time IS Scaled Or Not</param>
        public Job(Action<float> action, bool Time)
        {
            ToPerForm = action;
            m_TimeScaled = Time;
            JobPerforming = false;
        }

        #region Public
        public void Start()
        {
            if (!JobPerforming)
            {
                JobPerforming= true;
                JobSystem.AddJob(this);
            }
        }
        
        public void Stop()
        {
            if (JobPerforming)
            {
                JobPerforming = false;
                JobSystem.RemoveJob(this);
            }
        }

        public void Perform(float Time)
        {
            ToPerForm(Time);
        }

        #endregion
    }
}