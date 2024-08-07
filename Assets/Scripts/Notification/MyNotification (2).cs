using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Notifications;
#if UNITY_ANDROID
using Unity.Notifications.Android;
using UnityEngine.Android;
#elif UNITY_IOS
using Unity.Notifications.iOS;
#endif

public class MyNotification
{
    private const string channelId = "default_channel";
    private const string channelName = "Default Channel";
    private const string channelDes = "Generic notifications";
    private const string smallIcon = "smallicon";
    private const string largeIcon = "largeicon";
    private const string banner = "banner";

    private bool isInit = false;

    public static void CreateChannel()
    {
#if UNITY_ANDROID
        AndroidNotificationChannel a = new AndroidNotificationChannel(channelId, channelName, channelDes, Importance.Default);
        AndroidNotificationCenter.RegisterNotificationChannel(a);
#elif UNITY_IOS
        StartCoroutine(RequestAuthorizationIos());
#endif
        //AndroidNotificationCenter.Initialize();
    }
#if UNITY_ANDROID
    private void RequestAuthorizationAndroid()
    {
        if (!Permission.HasUserAuthorizedPermission("android.permission.POST_NOTIFICATIONS"))
        {
            Permission.RequestUserPermission("android.permission.POST_NOTIFICATIONS");
        }
    }
#endif

#if UNITY_IOS
    private IEnumerator RequestAuthorizationIos()
    {
        using var req = new AuthorizationRequest(AuthorizationOption.Alert | AuthorizationOption.Badge, true);

        while (!req.IsFinished)
        {
            yield return null;
        }
    }
#endif

    // Use this for initialization
    public static void SendPush(string title, string des, DateTime timeDelay, string callBack = "")
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidNotificationCenter.SendNotification(new AndroidNotification()
        {
            Title = title,
            Text = des,
            FireTime = timeDelay,
            IntentData = callBack,
            SmallIcon = smallIcon,
            LargeIcon = largeIcon
        }, channelId);
        //NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(timeDelay), title, des, new Color(0, 0.6f, 1), NotificationIcon.Message);
#elif UNITY_IOS
        var timeTrigger = new iOSNotificationTimeIntervalTrigger()
        {
            TimeInterval = timeDelay.Subtract(DateTime.Now),
            Repeats = false
        };

        var notification = new iOSNotification()
        {
            // You can specify a custom identifier which can be used to manage the notification later.
            // If you don't provide one, a unique string will be generated automatically.
            //Identifier = "_notification_01",
            Identifier = channelId,
            Title = title,
            Body = des,
            Subtitle = des,
            ShowInForeground = true,
            ForegroundPresentationOption = (PresentationOption.Alert | PresentationOption.Sound),
            CategoryIdentifier = channelName,
            ThreadIdentifier = channelDes,
            Trigger = timeTrigger,
        };

        iOSNotificationCenter.ScheduleNotification(notification);
#endif
    }

    public static void CancelAll()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidNotificationCenter.CancelAllNotifications();
        //NotificationManager.CancelAll();
#elif UNITY_IOS
        try
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

#endif
    }

    public static void CancelAllScheduleNotification()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidNotificationCenter.CancelAllNotifications();
//NotificationManager.CancelAll();
#elif UNITY_IOS
        try
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }

#endif
    }

    public static string GetNotificationCallback()
    {
        string callBack = string.Empty;
#if UNITY_ANDROID && !UNITY_EDITOR

#endif
        return callBack;
    }
}
