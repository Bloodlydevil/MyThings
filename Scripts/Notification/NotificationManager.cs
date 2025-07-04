using System;
using UnityEngine;
using MyThings.ExtendableClass;

#if UNITY_ANDROID
using Unity.Notifications.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class NotificationManager : Singleton<NotificationManager>
{

    protected override void Awake()
    {
        base.Awake();
        if (Alive)
        {
            RegisterNotificationChannel();
        }
    }

    private void RegisterNotificationChannel()
    {
#if UNITY_ANDROID
        var channel = new AndroidNotificationChannel()
        {
            Id = "default_channel",
            Name = "Default Channel",
            Importance = Importance.High,
            Description = "Generic notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
#endif
    }

    public void ScheduleNotification(string title, string message, TimeSpan delay)
    {
        CancelAllNotifications(); // Optional: prevents duplicates

#if UNITY_ANDROID
        var notification = new AndroidNotification()
        {
            Title = title,
            Text = message,
            FireTime = DateTime.Now.Add(delay)
        };
        AndroidNotificationCenter.SendNotification(notification, "default_channel");
#elif UNITY_IOS
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = delay,
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            Identifier = "_local_notification",
            Title = title,
            Body = message,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            Trigger = timeTrigger
        };
        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public void CancelAllNotifications()
    {
#if UNITY_ANDROID
        AndroidNotificationCenter.CancelAllScheduledNotifications();
#elif UNITY_IOS
        iOSNotificationCenter.RemoveAllScheduledNotifications();
#endif
    }
}
