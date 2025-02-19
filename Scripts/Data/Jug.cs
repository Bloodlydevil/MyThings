using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MyThings.Data
{
    /// <summary>
    /// A Jug To Store Data (Lower Limit And Upper Limit) With Some Events To Notify
    /// </summary>
    [Serializable]
    public class Jug
    {
        private float m_value;
        private float m_Capacity;
        [SerializeField, HorizontalGroup(order: 1, GroupName = "Limit"), ValidateInput("@"+nameof(m_MaxValue) + ">" + nameof(m_MinValue))]
        private float m_MaxValue;
        [SerializeField, HorizontalGroup(order: 2, GroupName = "Limit"), ValidateInput("@"+nameof(m_MaxValue) + ">" + nameof(m_MinValue))]
        private float m_MinValue;


        public event Action OnOverFlow;
        public event Action<float> OnChange;
        public event Action<float> OnNoChange;
        public event Action<float> OnBurstChange;
        public event Action<float> OnSmoothChange;
        public event Action OnUnderFlow;
        public event Action OnFirstNotEmpty;

        /// <summary>
        /// Is The Jug Completely Filled (Current value is equal to max value)
        /// </summary>
        public bool IsFull => m_value == m_MaxValue;
        /// <summary>
        /// Is The Jug Completely Empty (Current Value Is equal To Min Value)
        /// </summary>
        public bool IsEmpty => m_value == m_MinValue;
        /// <summary>
        /// The Max Limit The Water Can Go
        /// </summary>
        public float MaxLimit
        {
            get => m_MaxValue;
            set
            {
                m_MaxValue = value;
                CalculateCapacity();
            }
        }
        /// <summary>
        /// The Min Limit The Water Can Be
        /// </summary>
        public float MinLimit
        {
            get => m_MinValue;
            set 
            { 
                m_MinValue = value;
                CalculateCapacity();
            }
        }
        /// <summary>
        /// The Current Capacity Of The jug
        /// </summary>
        public float Capacity => m_Capacity;
        /// <summary>
        /// The Normalized Level Of The Water (0 to 1)
        /// </summary>
        public float NormalizedLevel => (m_value -m_MinValue)/ m_Capacity;

        public Jug(float Max, float Min, float Curr = float.MinValue)
        {
            m_value = Curr != float.MinValue ? Curr : Min;
            m_MaxValue = Max;
            m_MinValue = Min;
            CalculateCapacity();
        }

        #region private

        /// <summary>
        /// If The Water Flow Has Exceeded The Max Limit Trigger an Event
        /// </summary>
        private void TriggerOverflowIfNeeded()
        {
            if (m_value >= m_MaxValue)
            {
                m_value = m_MaxValue;
                OnOverFlow?.Invoke();
            }
        }

        /// <summary>
        /// If The Water Level Has Receded The Min Limit Trigger An Event
        /// </summary>
        private void TriggerUnderflowIfNeeded()
        {
            if (m_value <= m_MinValue)
            {
                m_value = m_MinValue;
                OnUnderFlow?.Invoke();
            }
        }
        /// <summary>
        /// Calculate The Current Capacity
        /// </summary>
        private void CalculateCapacity()
        {
            m_Capacity=(m_MaxValue - m_MinValue);
        }
        /// <summary>
        /// Add Water To The Current Jug
        /// </summary>
        /// <param name="AdditionalWater">The Water To Add or remove</param>
        /// <param name="Burst">If We Are Adding The Water In Burst</param>
        /// <returns></returns>
        private Jug AddWater(float AdditionalWater, bool Burst = false)
        {
            if (IsEmpty && AdditionalWater > 0)
                OnFirstNotEmpty?.Invoke();
            float old = m_value;
            m_value += AdditionalWater;
            TriggerOverflowIfNeeded();
            TriggerUnderflowIfNeeded();

            var normalizedValue = NormalizedLevel;
            if(old==m_value)
            {
                OnNoChange?.Invoke(normalizedValue);
                return this;
            }
            OnChange?.Invoke(normalizedValue);

            if (Burst)
                OnBurstChange?.Invoke(normalizedValue);
            else
                OnSmoothChange?.Invoke(normalizedValue);

            return this;
        }
        #endregion
        /// <summary>
        /// Fill The Jug Completely With Water
        /// </summary>
        public void FillUp() => m_value = m_MaxValue;
        /// <summary>
        /// Drain All The Water In The Jug
        /// </summary>
        public void Drain() => m_value = m_MinValue;
        /// <summary>
        /// Add The Water To The Jug In Burst Mode
        /// </summary>
        /// <param name="AdditionalWater">The Extra Water To Add</param>
        public void BurstAdd(float AdditionalWater) => AddWater(AdditionalWater, true);
        /// <summary>
        /// Add Water To The Jug And Return The Delta Water Change That Occured In The Jug
        /// </summary>
        /// <param name="AdditionalWater">The Extra Water To Add</param>
        /// <returns>The Delta Change in The Jug Water Level</returns>
        public float ChangeOnWaterAdd(float AdditionalWater)
        {
            float old = m_value;
            AddWater(AdditionalWater);
            return old - m_value;
        }
        /// <summary>
        /// Some
        /// </summary>
        public void SetUpJug()
        {
            CalculateCapacity();
        }
        #region operators
        public static Jug operator ++(Jug item) => item.AddWater(1);
        public static Jug operator --(Jug item) => item.AddWater(-1);
        public static Jug operator +(Jug item1, float item) => item1.AddWater(item);
        public static Jug operator -(Jug item1, float item) => item1.AddWater(-item);

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
