using System;

namespace MyThings.Job_System
{
    public partial class JobSystem
    {
        /// <summary>
        /// The Jobs Which We May Have
        /// </summary>
        public class Job : IJob
        {
            private bool m_JobPerforming;
            private bool m_FixedUpdate;
            private Action<float> ToPerForm;
            public bool Running { get => m_JobPerforming; }
            public bool TimeScaled { get; set; }

            public bool FixedUpdate => m_FixedUpdate;

            /// <summary>
            /// Constructor To Create A Job
            /// </summary>
            /// <param name="action">The Function To Call The Job</param>
            /// <param name="Time">If THe Time IS Scaled Or Not</param>
            /// <param name="FixedUpdate">If We Should Used Fixed Update Or Nots</param>
            public Job(Action<float> action, bool Time, bool FixedUpdate = false)
            {
                ToPerForm = action;
                TimeScaled = Time;
                m_JobPerforming = false;
                m_FixedUpdate = FixedUpdate;
            }
            public Job(bool FixedUpdate=false,bool Time=false) 
            {
                TimeScaled = Time;
                m_JobPerforming = false;
                m_FixedUpdate = FixedUpdate;
            }
            #region Public
            public void Start()
            {
                if (!m_JobPerforming)
                {
                    m_JobPerforming = true;
                    Instance.AddJob(this);
                }
            }

            public void Stop()
            {
                if (m_JobPerforming)
                {
                    m_JobPerforming = false;
                    Instance.RemoveJob(this);
                }
            }

            public void Perform(float Time)
            {
                ToPerForm(Time);
            }
            public void SetAction(Action<float> action)
            {
                ToPerForm = action;
            }
            #endregion
        }
    }
}