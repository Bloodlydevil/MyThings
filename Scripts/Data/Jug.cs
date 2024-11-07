using System;

namespace MyThings.Data
{
    /// <summary>
    /// A Class To Act As A Jug Where We Are Notified Whever We Overflow Or Are Empty
    /// </summary>
    [Serializable]
    public class Jug
    {
        private float m_value;
        private readonly float m_MaxValue;
        private readonly float m_MinValue;
        private readonly float m_Range;

        public bool IsFull => m_value == m_MaxValue;
        public bool IsEmpty => m_value == m_MinValue;


        public event Action OnOverFlow;
        public event Action<float> OnChange;
        public event Action OnUnderFlow;
        public event Action OnFirstNotEmpty;

        public Jug(float Max, float Min, float Curr = float.MinValue)
        {
            if (Curr != float.MinValue)
                m_value = Curr;
            else
                m_value = Min;
            m_MaxValue = Max;
            m_MinValue = Min;
            m_Range = m_MaxValue - m_MinValue;
        }
        #region private
        private Jug Mix( float item)
        {
            if (m_value == m_MinValue && item >0)
                OnFirstNotEmpty?.Invoke();
            m_value += item;
            if (m_value >= m_MaxValue)
            {
                m_value = m_MaxValue;
                OnOverFlow?.Invoke();
            }
            else if(m_value <= m_MinValue)
            {
                m_value = m_MinValue;
                OnUnderFlow?.Invoke();
            }
            OnChange?.Invoke(m_value/m_Range);
            return this;
        }
        #endregion
        /// <summary>
        /// Fill The Jug
        /// </summary>
        public void FillUp() => m_value = m_MaxValue;
        /// <summary>
        /// Empty The Jug
        /// </summary>
        public void Empty() => m_value = m_MinValue;

        #region operators
        public static Jug operator ++(Jug item)
        {
            return item.Mix(1);
        }
        public static Jug operator --(Jug item)
        {
            return item.Mix(-1);
        }
        public static Jug operator +(Jug item1,float item)
        {
            return item1.Mix(item);
        }
        public static Jug operator -(Jug item1, float item)
        {
            return item1.Mix(-item);
        }
        public static explicit operator float(Jug value)
        {
            return value.m_value;
        }
        public static bool operator ==(Jug left, float right)
        {
            return left.m_value == right;
        }
        public static bool operator !=(Jug left, float right)
        {
            return left.m_value != right;
        }
        public static bool operator <(Jug left, float right)
        {
            return left.m_value < right;
        }
        public static bool operator >(Jug left, float right)
        {
            return left.m_value > right;
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
