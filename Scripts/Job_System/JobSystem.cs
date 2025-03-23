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
    public partial class JobSystem : Singleton_C<JobSystem>
    {
        /// <summary>
        /// The Total Jobs Running
        /// </summary>
        [SerializeField] private int TotalUpdateJobs;
        [SerializeField] private int TotalFixedUpdateJobs;

        /// <summary>
        /// The Current Active Jobs
        /// </summary>
        private ListD<IJob> ActiveUpdateJobs=new ListD<IJob>();
        private ListD<IJob> ActiveFixedUpdateJobs=new ListD<IJob>();

        #region Unity Functions
        private void Update()
        {
            ActiveUpdateJobs.SyncRemove();
            TotalUpdateJobs = ActiveUpdateJobs.L.Count;
            for (int i = 0; i < TotalUpdateJobs; i++)
            {
                IJob job = ActiveUpdateJobs.L[i];
                job.Perform(job.TimeScaled ? Time.deltaTime : Time.unscaledDeltaTime);
            }
        }
        private void FixedUpdate()
        {
            ActiveFixedUpdateJobs.SyncRemove();
            TotalFixedUpdateJobs = ActiveFixedUpdateJobs.L.Count;
            for (int i = 0; i < TotalFixedUpdateJobs; i++)
            {
                IJob job = ActiveFixedUpdateJobs.L[i];
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
            if (job.FixedUpdate)
                ActiveFixedUpdateJobs.Add(job);
            else
                ActiveUpdateJobs.Add(job);
        }


        /// <summary>
        /// Add Job To The Remove Jobs ( Considers The Job Is Not Already Added )
        /// </summary>
        /// <param name="job">The Job</param>
        public void RemoveJob(IJob job)
        {
            if (job.FixedUpdate)
                ActiveFixedUpdateJobs.Remove(job);
            else
                ActiveUpdateJobs.Remove(job);
        }


        #endregion


    }
}