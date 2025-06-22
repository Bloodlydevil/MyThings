using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyThings.Data
{
    [Serializable]
    public class ProgressTracker
    {
        private readonly Dictionary<string, Progress> m_EventProgress = new();
        private readonly HashSet<string> m_EventCompleted = new();
        private float m_TotalProgress = 0f;

        public event Action<string> OnEventCompleted;
        public event Action OnAllEventsCompleted;

        public float Progress => m_TotalProgress / (m_EventProgress.Count + m_EventCompleted.Count); // Calculate progress as a fraction of completed events

        public void AddEvent(string eventName)=> AddEvent(eventName, new Progress(1)); // Default progress of 1 for new events
        public void AddEvent(string eventName, Progress progress)
        {
            progress.OnProgressCompleted += () => CompleteEvent(eventName); // Subscribe to event completion
            if (string.IsNullOrEmpty(eventName)) return;
            if (!m_EventProgress.ContainsKey(eventName) || m_EventCompleted.Contains(eventName)) return;
            m_EventProgress.Add(eventName, progress);
        }

        public void UpdateEventProgress(string eventName, float progress)
        {
            if (string.IsNullOrEmpty(eventName) || !m_EventProgress.ContainsKey(eventName) || m_EventCompleted.Contains(eventName)) return;
            if (progress < 0 || progress > 1)
            {
                Debug.LogWarning($"Progress value {progress} for event '{eventName}' is out of bounds. It should be between 0 and 1.");
                return; // Ensure progress is between 0 and 1
            }
            if (progress == 1f)
            {
                m_TotalProgress -= m_EventProgress[eventName].ProgressValue; // Remove previous progress before completing
                CompleteEvent(eventName);
            }
            else
                m_EventProgress[eventName].ProgressValue = progress;
        }

        public void CompleteEvent(string eventName)
        {
            if (string.IsNullOrEmpty(eventName)) return;
            m_EventProgress.Remove(eventName);
            m_EventCompleted.Add(eventName);
            m_TotalProgress++;
            OnEventCompleted?.Invoke(eventName);
            if(m_EventProgress.Count == 0)
            {
                OnAllEventsCompleted?.Invoke(); // Notify when all events are completed
            }
        }
    }
}