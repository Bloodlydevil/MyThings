using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Timer
{
    /// <summary>
    /// Performs Task When It Is Free
    /// </summary>
    [Serializable]
    public class Scheduler
    {
        [SerializeReference] private ITimer m_timer;
        public bool Override;
        public Queue<Action> Tasks=new Queue<Action>();

        /// <summary>
        /// It Creates A schedular where to performs each task after maxTime
        /// </summary>
        /// <param name="maxTime">The Time after which to alow task performation</param>
        /// <param name="Override">Override old task with new one</param>
        /// <param name="Scale">the scale of the time</param>
        public Scheduler(float maxTime, bool Override = false, bool Scale = false)
        {
            this.Override = Override;
            m_timer = TimerManager.Create(maxTime, Perform, true, Scale);
        }
        private void Perform()
        {
            if(Tasks.Count!=0)
            {
                Tasks.Dequeue().Invoke();
            }
            if (Tasks.Count == 0)
            {
                m_timer.Stop();
            }
        }
        /// <summary>
        /// Schedule a tast to perform when it is free
        /// </summary>
        /// <param name="Task">the task to perform</param>
        public void Schedule(Action Task)
        {
            if (Override)
            {
                Tasks.Clear();
                Tasks.Enqueue(Task);
            }
            else
                Tasks.Enqueue(Task);
            if (!m_timer.Running)
            {
                Tasks.Dequeue().Invoke();
                m_timer.Start();
            }
        }
        /// <summary>
        /// Cancel the task
        /// </summary>
        public void Cancel()
        {
            m_timer.Stop();
            Tasks.Clear();
        }
    }
}
