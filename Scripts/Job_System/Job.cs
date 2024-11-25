using System;

namespace MyThings.Job_System
{

    /// <summary>
    /// The Jobs Which We May Have
    /// </summary>
    public class Job: IJob
    {
        public Action<float> ToPerForm {  get; set; }
        private bool JobPerforming;
        public bool Running { get => JobPerforming; }
        public bool TimeScaled { get; set; }

        /// <summary>
        /// Constructor To Create A Job
        /// </summary>
        /// <param name="action">The Function To Call The Job</param>
        /// <param name="Time">If THe Time IS Scaled Or Not</param>
        public Job(Action<float> action, bool Time)
        {
            ToPerForm = action;
            TimeScaled = Time;
            JobPerforming = false;
        }
        public Job()
        {

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
            ToPerForm(Time);
        }

        #endregion
    }
}