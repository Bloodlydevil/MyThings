

namespace MyThings.Job_System
{
    public interface IJob
    {
        /// <summary>
        /// If The Time Is Scaled
        /// </summary>
        public bool TimeScaled { get; set; }
        /// <summary>
        /// If The Job Is Running
        /// </summary>
        public bool Running { get; }
        /// <summary>
        /// If The Job Is Run On Fixed Update
        /// </summary>
        public bool FixedUpdate { get; }
        /// <summary>
        /// Perform one iteration of job with the given time
        /// </summary>
        /// <param name="Time"></param>
        public void Perform(float Time);
        /// <summary>
        /// Start The Job When We Hit Start
        /// </summary>
        public void Start();
        /// <summary>
        /// Stop This Job
        /// </summary>
        public void Stop();
    }
}
