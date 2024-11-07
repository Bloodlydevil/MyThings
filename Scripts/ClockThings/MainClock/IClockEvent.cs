using System;

namespace MyThings.ClockThings.MainClock
{

    /// <summary>
    /// Interface Used To Deal With Events Of The Clock
    /// </summary>
    public interface IClockEvent
    {
        /// <summary>
        /// Remove The Event To Stop The Calling Function
        /// </summary>
        /// <param name="Function">The Function Used To Subscribe The Event</param>
        public void RemoveEvent(Action Function);
    }

}