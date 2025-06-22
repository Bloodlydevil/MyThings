using System;

[Serializable]
public class Progress
{
    private float m_CurrentProgress = 0f; // Current progress value

    public float MaxProgress { get; private set; }

    public float ProgressValue 
    { 
        get=> m_CurrentProgress / MaxProgress;
        set => UdateProgress(value * MaxProgress);
    } // Calculate progress as a fraction of current to maximum

    public event Action<float> OnProgressUpdated; // Event to notify when progress is updated
    public event Action OnProgressCompleted; // Event to notify when progress reaches maximum
    public event Action OnProgressStarted; // Event to notify when progress starts


    public Progress(float MaxProgress)
    {
        this.MaxProgress = MaxProgress;
    }
    public Progress(float MaxProgress,float CurrentProgress)
    {
        this.MaxProgress = MaxProgress;
        m_CurrentProgress = CurrentProgress;
    }

    public void UdateProgress(float value)
    {
        if(m_CurrentProgress==0f)
        {
            OnProgressStarted?.Invoke(); // Notify subscribers that progress has started
        }

        m_CurrentProgress = value;

        OnProgressUpdated?.Invoke(ProgressValue); // Notify subscribers of the updated progress

        if (m_CurrentProgress >= MaxProgress)
        {
            m_CurrentProgress = MaxProgress; // Cap at maximum
            OnProgressCompleted?.Invoke(); // Notify subscribers that progress is complete
        }
    }
}
