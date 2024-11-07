namespace MyThings.Data
{
    /// <summary>
    /// The Wrapper Class To Store A Min And Max Value
    /// </summary>
    /// <typeparam name="t">The Type</typeparam>
    [System.Serializable]
    public class DoubleValue<t>
    {
        /// <summary>
        /// The Min
        /// </summary>
        public t _First;
        /// <summary>
        /// The Max
        /// </summary>
        public t _Second;
        /// <summary>
        /// Constructor To Create MinMax Value Holder
        /// </summary>
        /// <param name="Min">The Min Value</param>
        /// <param name="Max">The Max Value</param>
        public DoubleValue(t Min,t Max) { _First = Min;_Second = Max; }
        /// <summary>
        /// Just A Default Constructor
        /// </summary>
        public DoubleValue() { }
    }
}