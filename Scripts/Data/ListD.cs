using System;
using System.Collections.Generic;

namespace MyThings.Data
{
    [Serializable]
    /// <summary>
    /// A List To Safely Remove List Item When It Is Save To Remove
    /// </summary>
    /// <typeparam name="Type">Type Of List</typeparam>
    public class ListD<Type>
    {
        /// <summary>
        /// Current List 
        /// </summary>
        private List<Type> m_CurrentBuffer = new List<Type>();
        /// <summary>
        /// Buffer To Delete
        /// </summary>
        private Queue<Type> m_DeleteBuffer = new Queue<Type>();
        /// <summary>
        /// List
        /// </summary>
        public List<Type> L => m_CurrentBuffer;

        /// <summary>
        /// Add To List
        /// </summary>
        /// <param name="type">The Element</param>
        public void Add(Type type)
        {
            m_CurrentBuffer.Add(type);
        }
        /// <summary>
        /// Remove From List (Only Records Now)
        /// </summary>
        /// <param name="type">The Element To Remove</param>
        public void Remove(Type type)
        {
            m_DeleteBuffer.Enqueue(type);
        }
        /// <summary>
        /// Remove The Elements
        /// </summary>
        public void SyncRemove()
        {
            while (m_DeleteBuffer.Count > 0)
            {
                m_CurrentBuffer.Remove(m_DeleteBuffer.Dequeue());
            }
        }
        /// <summary>
        /// Clear All The Buffers
        /// </summary>
        public void Clear()
        {
            m_CurrentBuffer.Clear();
            m_DeleteBuffer.Clear();
        }
    }
}