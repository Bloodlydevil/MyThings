using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MyThings.Data
{
    [Serializable]
    public class Jug
    {
        private float m_value;
        [SerializeField, HorizontalGroup(order: 1, GroupName = "Limit"), ValidateInput("@"+nameof(m_MaxValue) + ">" + nameof(m_MinValue))]
        private float m_MaxValue;
        [SerializeField, HorizontalGroup(order: 2, GroupName = "Limit"), ValidateInput("@"+nameof(m_MaxValue) + ">" + nameof(m_MinValue))]
        private float m_MinValue;

        public event Action OnOverFlow;
        public event Action<float> OnChange;
        public event Action<float> OnTryChange;
        public event Action<float> OnBurstChange;
        public event Action<float> OnSmothChange;
        public event Action OnUnderFlow;
        public event Action OnFirstNotEmpty;

        public bool IsFull => m_value == m_MaxValue;
        public bool IsEmpty => m_value == m_MinValue;

        public Jug(float Max, float Min, float Curr = float.MinValue)
        {
            m_value = Curr != float.MinValue ? Curr : Min;
            m_MaxValue = Max;
            m_MinValue = Min;
        }

        #region private

        private void TriggerOverflowIfNeeded()
        {
            if (m_value >= m_MaxValue)
            {
                m_value = m_MaxValue;
                OnOverFlow?.Invoke();
            }
        }

        private void TriggerUnderflowIfNeeded()
        {
            if (m_value <= m_MinValue)
            {
                m_value = m_MinValue;
                OnUnderFlow?.Invoke();
            }
        }

        private Jug Mix(float item, bool Burst = false)
        {
            if (IsEmpty && item > 0)
                OnFirstNotEmpty?.Invoke();
            float old = m_value;
            m_value += item;
            TriggerOverflowIfNeeded();
            TriggerUnderflowIfNeeded();

            var normalizedValue = m_value / (m_MaxValue - m_MinValue);
            if(old==m_value)
            {
                OnTryChange?.Invoke(normalizedValue);
                return this;
            }
            OnChange?.Invoke(normalizedValue);

            if (Burst)
                OnBurstChange?.Invoke(normalizedValue);
            else
                OnSmothChange?.Invoke(normalizedValue);

            return this;
        }
        #endregion

        public void FillUp() => m_value = m_MaxValue;
        public void Empty() => m_value = m_MinValue;

        public void ExpandMaxBy(float DeltaJugIncrease)
        {
            m_MaxValue += DeltaJugIncrease;
        }

        public void DecreaseMaxBy(float DeltaJugDecrease)
        {
            m_MaxValue -= DeltaJugDecrease;
        }

        public void ExpandMinBy(float DeltaJugIncrease)
        {
            m_MinValue += DeltaJugIncrease;
        }

        public void DecreaseMinBy(float DeltaJugDecrease)
        {
            m_MinValue -= DeltaJugDecrease;
        }

        public void BurstChange(float value) => Mix(value, true);
        public float AnyChange(float Add)
        {
            float old = m_value;
            Mix(Add);
            return old - m_value;
        }

        #region operators
        public static Jug operator ++(Jug item) => item.Mix(1);
        public static Jug operator --(Jug item) => item.Mix(-1);
        public static Jug operator +(Jug item1, float item) => item1.Mix(item);
        public static Jug operator -(Jug item1, float item) => item1.Mix(-item);

        public static implicit operator float(Jug value) => value.m_value;

        public static bool operator ==(Jug left, float right) => left.m_value == right;
        public static bool operator !=(Jug left, float right) => left.m_value != right;
        public static bool operator <(Jug left, float right) => left.m_value < right;
        public static bool operator >(Jug left, float right) => left.m_value > right;
        public override bool Equals(object obj)
        {
            if (obj is Jug jug)
            {
                return m_value == jug.m_value && m_MaxValue == jug.m_MaxValue && m_MinValue == jug.m_MinValue;
            }
            return false;
        }

        public override int GetHashCode() => HashCode.Combine(m_value, m_MaxValue, m_MinValue);
        #endregion
    }
}
