namespace MyThings.Data
{
    /// <summary>
    /// A Data To Hold Name Used In Program And Name To Show
    /// </summary>
    public struct ReflectionName
    {
        /// <summary>
        /// The Actual Name Of The Thing
        /// </summary>
        public string MemberName { get;private set; }
        /// <summary>
        /// The Name To Show
        /// </summary>
        public string ShowName { get; set; }

        public ReflectionName(string memberName,string showName)
        {
            MemberName = memberName;
            ShowName = showName;
        }
    }
}