using System;
using UnityEngine;

namespace MyThings.Data
{
    [Serializable]
    /// <summary>
    /// A Class To Make An Event System For A Process
    /// </summary>
    public class Prosses
    {
        [SerializeField]
        private byte m_Count;

        public event Action Start;
        public event Action End;
        public bool IsOccuring => m_Count != 0;

        /// <summary>
        ///  Start The Process
        /// </summary>
        public void StartProcess()
        {
            m_Count++;
            if (m_Count == 1)
                Start?.Invoke();
        }
        /// <summary>
        /// End The Process
        /// </summary>
        public void EndProcess()
        {
            m_Count--;
            if (m_Count==0)
            {
                End?.Invoke();
            }
            else if(m_Count<0)
            {
                Debug.Log("Going Under The Process (-ve)");
            }
        }
    }
}
