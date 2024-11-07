using System;


namespace MyThings.Extension
{

    /// <summary>
    /// Extention class o Deal with TimeSpan Functions
    /// </summary>
    public static class ExtensionTimeSpan
    {
        /// <summary>
        /// This is An Extention Method To Calculate Time Difference
        /// </summary>
        /// <param name="FromTime">From This Time</param>
        /// <param name="ToTime">To This Time</param>
        /// <param name="Abs">Should We Get Absolute Time</param>
        /// <returns>The Time Difference</returns>
        public static TimeSpan CalculateTimeDifference(this TimeSpan FromTime, TimeSpan ToTime, bool Abs = true)
        {
            TimeSpan difference = ToTime - FromTime;
            if (Abs && difference.TotalSeconds < 0)//Absolute Time Diff
                difference += TimeSpan.FromHours(24);
            return difference;
        }
    }
}