using System;

namespace MyThings.Data
{
    [Serializable]
    /// <summary>
    /// A Data Type for cyclic data type
    ///<para>you Can set min value and max value</para>
    ///and The Current will always lie in that range
    /// </summary>
    public struct D_Cyclic
    {
        /// <summary>
        /// The Cyclicity of the Data
        /// <para>Max-Min+1</para>
        /// </summary>
        private readonly int m_Cyclicity;
        /// <summary>
        /// The Min Limit of the data
        /// </summary>
        private readonly int m_min;
        /// <summary>
        /// The Current Value
        /// <para>Between the range of 0 and Cyclicity</para>
        /// </summary>
        private int current;
        /// <summary>
        /// create a Cyclic Data
        /// </summary>
        /// <param name="min">The Max Range</param>
        /// <param name="max">The Min Range</param>
        public D_Cyclic(int min, int max)
        {
            m_Cyclicity = max-min+1;
            m_min = min;
            current = 0;
        }
        /// <summary>
        /// Set the current To "ath" term
        /// </summary>
        /// <param name="a">The ath term of the range</param>
        /// <returns>The Data changed returned</returns>
        public D_Cyclic Set(int a)
        {
            a%=m_Cyclicity;
            current = a;
            return Correct(this);
        }
        /// <summary>
        /// Fit the no to the range
        /// <code>example 2,8 and the no is 4 
        /// so it will be 4
        /// </code>
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public D_Cyclic Fit(int a)
        {
            a -= m_min;
            current = a;
            return Correct(this);
        }
        // Add the no. "b" to a and then modulas it by its cyclicity
        // It may be possible that no. is negative so we correct it
        public static D_Cyclic operator +(D_Cyclic a,int b)
        {
            a.current += b;
            return Correct(a);
        }
        // change the current no. be subtracting it from cyclicity
        // exp-> 2,6 and no is 3 the it will become 5 
        public static D_Cyclic operator -(D_Cyclic a)
        {
            a.current = a.m_Cyclicity - a.current;
            return a;
        }
        // increase the no and correct
        public static D_Cyclic operator ++(D_Cyclic a)
        {
            a.current++;
            return Correct(a);
        }
        // reduce the no and correct it
        public static D_Cyclic operator --(D_Cyclic a)
        {
            a.current--;
            return Correct(a);
        }
        // just subtract the no and correct it
        public static D_Cyclic operator -(D_Cyclic a,int b)
        {
            a.current -= b;
            return Correct(a);
        }
        // correct the no by doing mod of it by its cyclicity and
        // if its negative then currecting
        private static D_Cyclic Correct(D_Cyclic a)
        {
            a.current %= a.m_Cyclicity;
            if (a.current < 0) { a.current += a.m_Cyclicity; }
            return a;
        }
        // adding the no by its min value will convert it to the correct no.
        public static implicit operator int(D_Cyclic d) => d.current+d.m_min;
    }
}