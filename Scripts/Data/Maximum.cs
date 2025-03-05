using System;

namespace MyThings.Data
{
    /// <summary>
    /// Store Only The Maximum Value Of The Given Values 
    /// </summary>
    /// <typeparam name="T">The Type Of Values To Expect</typeparam>
    public class Maximum<T> where T : IComparable<T>
    {
        /// <summary>
        /// The Value
        /// </summary>
        private T m_Value;

        public Maximum(T value)
        {
            m_Value = value;
        }

        /// <summary>
        /// The Value To Set
        /// </summary>
        /// <param name="otherValue"></param>
        public void SetValue(T otherValue)
        {
            m_Value= (m_Value.CompareTo(otherValue) > 0) ? m_Value : otherValue;
        }
        public static explicit operator T(Maximum<T> value) => value.m_Value;
    }
}
