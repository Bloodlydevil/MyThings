using System;

namespace MyThings.Job_System
{
    public partial class JobSystem 
    {
        /// <summary>
        /// The Job To Perform Till It Results In True
        /// </summary>
        public class JobCondition : IJob
        {

            private Func<float, bool> ToPerForm;
            private bool JobPerforming;
            private bool m_FixedUpdate;
            public bool Running { get => JobPerforming; }
            public bool TimeScaled { get; set; }

            public bool FixedUpdate => m_FixedUpdate;

            /// <summary>
            /// Constructor To Create A Job
            /// </summary>
            /// <param name="action">The Function To Call The Job</param>
            /// <param name="Time">If THe Time IS Scaled Or Not</param>
            public JobCondition(Func<float, bool> action, bool Time, bool FixedUpdate = false)
            {
                ToPerForm = action;
                TimeScaled = Time;
                JobPerforming = false;
                m_FixedUpdate = FixedUpdate;
            }

            #region Public

            public void Start()
            {
                if (!JobPerforming)
                {
                    JobPerforming = true;
                    Instance.AddJob(this);
                }
            }
            public void Stop()
            {
                if (JobPerforming)
                {
                    JobPerforming = false;
                    Instance.RemoveJob(this);
                }
            }

            public void Perform(float Time)
            {
                if (ToPerForm(Time))
                {
                    Stop();
                }
            }

            #endregion
        }
    }
}