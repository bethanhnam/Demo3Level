using System;
using GoogleMobileAds.Api;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;


public class AdmobManager : MonoBehaviour
{
    public static AdmobManager instance;


    void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(this.gameObject);
        DontDestroyOnLoad(this.gameObject);
    }

    public bool CheckInternet()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    public void LogEventFirebase(string str)
    {
        Firebase.Analytics.FirebaseAnalytics.LogEvent(str);
    }

    public void ShowToast(string toast)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        AndroidJavaObject @static = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject androidJavaObject = new AndroidJavaClass("android.widget.Toast");
        androidJavaObject.CallStatic<AndroidJavaObject>("makeText", new object[]
        {
                @static,
                toast,
                androidJavaObject.GetStatic<int>("LENGTH_SHORT")
        }).Call("show", Array.Empty<object>());
#endif
    }
}
