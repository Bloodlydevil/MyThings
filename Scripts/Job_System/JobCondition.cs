using System;

namespace MyThings.Job_System
{

    /// <summary>
    /// The Job To Perform Till It Results In True
    /// </summary>
    public class JobCondition: IJob
    {
        
        private bool m_TimeScaled;
        private Func<float,bool> ToPerForm;
        private bool JobPerforming;
        public bool Running { get => JobPerforming; }
        public bool TimeScaled { get => m_TimeScaled; set => m_TimeScaled = value; }

        /// <summary>
        /// Constructor To Create A Job
        /// </summary>
        /// <param name="action">The Function To Call The Job</param>
        /// <param name="Time">If THe Time IS Scaled Or Not</param>
        public JobCondition(Func<float, bool> action, bool Time)
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
                JobSystem.Instance.AddJob(this);
            }
        }
        public void Stop()
        {
            if (JobPerforming)
            {
                JobPerforming = false;
                JobSystem.Instance.RemoveJob(this);
            }
        }

        public void Perform(float Time)
        {
            if(ToPerForm(Time))
            {
                Stop();
            }
        }

        #endregion
    }
}