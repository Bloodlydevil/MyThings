using System.Collections.Generic;

namespace MyThings.Data
{
    public class ListCAD<Type>
    {
        /// <summary>
        /// Buffer To Store Create List
        /// </summary>
        private Queue<Type> m_CreateBuffer = new Queue<Type>();
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
        /// Add To List (only records Now)
        /// </summary>
        /// <param name="type">The Element</param>
        public void Add(Type type)
        {
            m_CreateBuffer.Enqueue(type);
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
        /// Add The Elements
        /// </summary>
        public void SyncAdd()
        {
            while (m_CreateBuffer.Count > 0)
            {
                m_CurrentBuffer.Add(m_CreateBuffer.Dequeue());
            }
        }
    }
}