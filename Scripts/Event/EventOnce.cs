using System;

namespace MyThings.Events
{
    /// <summary>
    /// Call the event once and if any one tries to sub in future it will automaticaly call it
    /// <code>
    /// EventOnce<temp> t;
    /// event temp tt
    /// {
    /// add{}
    /// remove{}
    /// }
    /// </code>
    /// </summary>
    /// <typeparam name="temp">the event type</typeparam>
    public class EventOnce<temp> where temp : Delegate
    {
        /// <summary>
        /// Take Event in and call it
        /// </summary>
        /// <param name="t"></param>
        public delegate void EventCaller(temp t);


        private bool m_Call;
        private temp m_OnCall;
        private EventCaller m_Caller;

        /// <summary>
        /// Call the event (This Function Has No Way Of Knowing How To Call The Function Type so User Has To Provide(The Delegate Can Have Values TO Be Input))
        /// </summary>
        /// <param name="temp"> call now but with check for it can be null</param>
        /// <param name="ForFuture">call way in future (no checks needed)</param>
        public void Call(EventCaller temp, EventCaller ForFuture)
        {
            m_Call = true;
            temp(m_OnCall);
            m_OnCall = null;
            m_Caller = ForFuture;
        }
        public static EventOnce<temp> operator +(EventOnce<temp> first, temp second)
        {
            if (first.m_Call)
            {
                first.m_Caller(second);
                return first;
            }
            first.m_OnCall = Delegate.Combine(first.m_OnCall,second) as temp;
            return first;
        }
        public static EventOnce<temp> operator -(EventOnce<temp> first, temp second)
        {
            first.m_OnCall = Delegate.Remove(first.m_OnCall,second) as temp;
            return first;
        }
    }
}