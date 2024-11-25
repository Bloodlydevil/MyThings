using MyThings.Data;
using MyThings.ExtendableClass;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Job_System
{
    /// <summary>
    /// The System To Manage The Jobs ( Usualy Used To Give Non Component Function Call Every Frame)
    /// </summary>
    [DefaultExecutionOrder(-2)]
    public class JobSystem : Singleton_C<JobSystem>
    {
        /// <summary>
        /// The Total Jobs Running
        /// </summary>
        [SerializeField] private int TotalJobs;


        /// <summary>
        /// The Current Active Jobs
        /// </summary>
        private ListD<IJob> ActiveJobs=new ListD<IJob>();


        #region Unity Functions
        private void Update()
        {
            ActiveJobs.SyncRemove();
            TotalJobs = ActiveJobs.L.Count;
            for (int i = 0; i < TotalJobs; i++)
            {
                IJob job = ActiveJobs.L[i];
                job.Perform(job.TimeScaled ? Time.deltaTime : Time.unscaledDeltaTime);
            }
        }


        #endregion



        #region Static Functions


        /// <summary>
        /// Create New Job
        /// </summary>
        /// <param name="action">The Function To Call Every Frame</param>
        /// <returns>The Job</returns>
        public static IJob CreateJob(Action<float> action, bool Time)
        {
            return new Job(action, Time);
        }
        public static IJob CreateJob(Func<float, bool> action, bool Time)
        {
            return new JobCondition(action, Time);
        }


        /// <summary>
        /// Add Job To The Active Jobs ( Considers The Job Is Not Already Added )
        /// </summary>
        /// <param name="job">The Job</param>
        public void AddJob(IJob job)
        {
            ActiveJobs.Add(job);
        }


        /// <summary>
        /// Add Job To The Remove Jobs ( Considers The Job Is Not Already Added )
        /// </summary>
        /// <param name="job">The Job</param>
        public void RemoveJob(IJob job)
        {
            ActiveJobs.Remove(job);
        }


        #endregion


    }
}