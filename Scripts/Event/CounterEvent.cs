using System;
namespace MyThings.Events
{

    public class CounterEvent
    {
        private int m_TotalEvent;
        private int m_SucessEvent;
        private int m_FailEvent;

        private Action<int, int> m_OnComplete;

        public CounterEvent(int TotalCount,Action<int,int> OnComplete)
        {
            m_TotalEvent = TotalCount;
            m_OnComplete= OnComplete;
        }
        public CounterEvent(Action<int,int> OnComplete)
        {
            m_OnComplete = OnComplete;
        }

        private void FinishCheck()
        {
            if (m_FailEvent + m_SucessEvent == m_TotalEvent)
                m_OnComplete?.Invoke(m_SucessEvent, m_FailEvent);
        }

        public void SetTotalCount(int Count)
        {
            m_TotalEvent= Count;
        }

        public void OnSuccess()
        {
            m_SucessEvent++;
            FinishCheck();
        }
        public void OnFail(Exception ex) 
        {
            OnFail();
        }
        public void OnFail()
        {
            m_FailEvent++;
            FinishCheck();
        }
    }
}