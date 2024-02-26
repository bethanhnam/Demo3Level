using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Firebase.Extensions;
using Sirenix.OdinInspector;
using Newtonsoft.Json;
using System.Threading.Tasks;

public class RemoteConfigController : MonoBehaviour
{
    public static RemoteConfigController instance;
    [HideInInspector]
    public bool IsFetch = false;

    public bool isInit = false;
    public bool isFetching = true;

    [SerializeField]
    private string my_mediation;
    [SerializeField]
    private string ads_config_native;
    [SerializeField]
    private int banner_collab;     
    [SerializeField]
    private int value_super_impression_ads=1;   
    [SerializeField]
    private int value_impress_ads=1;
    [SerializeField]
    private int banner_type;

    //[SerializeField]
    //private string data_config;
    //[ShowInInspector]
    //private DataConfig dataConfig;
    //[Button]
    //public void SSSSS()
    //{
    //    data_config = JsonConvert.SerializeObject(dataConfig);
    //}

    //[SerializeField]
    //private DataSpinRemoteConfig dataSpinRemoteConfig;
    //[Button]
    //public void SSSSS()
    //{
    //    spin_config = JsonConvert.SerializeObject(dataSpinRemoteConfig);
    //}

    //[ShowInInspector]
    //private StructMediation structMediation;

    //[Button]
    //public void InitDataMediation()
    //{
    //    Debug.Log(JsonConvert.SerializeObject(structMediation));
    //}


    //[ShowInInspector]
    //private StructNativeMediation structMediationNAtive;

    //[Button]
    //public void InitDataMediationNative()
    //{
    //    my_mediation_native = JsonConvert.SerializeObject(structMediationNAtive);
    //}

    //[ShowInInspector]
    //private StructNativeEndGame tructNativeEndGame;

    //[Button]
    //public void InitDataStructNativeEndGame()
    //{
    //    native_config_endgame = JsonConvert.SerializeObject(tructNativeEndGame);
    //}

    public string My_mediation { get => my_mediation; set => my_mediation = value; }
    public int Banner_collab { get => banner_collab; set => banner_collab = value; }
    public int Banner_type { get => banner_type; set => banner_type = value; }
    public string Ads_config_native { get => ads_config_native; set => ads_config_native = value; }
    public int Value_impress_ads { get => value_impress_ads; set => value_impress_ads = value; }
    public int Value_super_impression_ads { get => value_super_impression_ads; set => value_super_impression_ads = value; }

    public static RemoteConfigController GetInstance()
    {
        if (instance == null)
        {
            GameObject obj = new GameObject();
            instance = obj.AddComponent<RemoteConfigController>();
        }
        return instance;
    }

    private void Awake()
    {
        instance = this;
        SetDefaultValue();
        LoadDefaultValue();
    }

    public void Init()
    {
        Debug.Log("Init RemoteConfig");
        isFetching = true;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
         {
             var dependencyStatus = task.Result;
             if (dependencyStatus == Firebase.DependencyStatus.Available)
             {
                 InitData();
             }
             else
             {
                 LoadDefaultValue();
                 isInit = true;
             }
             //DataUserManager.Instance.LogProperties111();
             // FirebaseAnalyticsControl.Instance.CallEndSS();
             isFetching = false;
         });
    }

    void InitData()
    {
        Fetch();
    }

    public void Fetch()
    {
        var fetchTask = Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.FetchAsync(new TimeSpan(0));

        fetchTask.ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                Debug.Log("Fetch failed");
                LoadDefaultValue();
                isInit = true;
            }
            else
            {
                Debug.Log("Fetch completed");
                IsFetch = true;
            }
            if (IsFetch)
            {
                Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                RefrectProperties();
            }
        });

    }

    public static Firebase.RemoteConfig.ConfigValue GetValue(string key)
    {
        return Firebase.RemoteConfig.FirebaseRemoteConfig.DefaultInstance.GetValue(key);
    }

    private void RefrectProperties()
    {
        try { my_mediation = (string.IsNullOrEmpty(GetValue("my_mediation").StringValue) ? my_mediation : GetValue("my_mediation").StringValue); }
        catch { }
        try { Ads_config_native = (string.IsNullOrEmpty(GetValue("ads_config_native").StringValue) ? Ads_config_native : GetValue("ads_config_native").StringValue); }
        catch { }
        try { banner_collab = (int)GetValue("banner_collab").DoubleValue; }
        catch { }  
        try { value_super_impression_ads = (int)GetValue("value_super_impression_ads").DoubleValue; }
        catch { }  
        try { value_impress_ads = (int)GetValue("value_impress_ads").DoubleValue; }
        catch { }
        try { banner_type = (int)GetValue("banner_type").DoubleValue; }
        catch { }

        SaveValue();
        isInit = true;
    }

    public void SetDefaultValue()
    {
        if (PlayerPrefs.HasKey("my_mediation"))
        {
            PlayerPrefs.SetString("my_mediation", my_mediation);
        }
        if (PlayerPrefs.HasKey("ads_config_native"))
        {
            PlayerPrefs.SetString("ads_config_native", Ads_config_native);
        }
        if (PlayerPrefs.HasKey("banner_collab"))
        {
            PlayerPrefs.SetInt("banner_collab", banner_collab);
        } 
        if (PlayerPrefs.HasKey("value_super_impression_ads"))
        {
            PlayerPrefs.SetInt("value_super_impression_ads", value_super_impression_ads);
        }  
        if (PlayerPrefs.HasKey("value_impress_ads"))
        {
            PlayerPrefs.SetInt("value_impress_ads", value_impress_ads);
        }
        if (PlayerPrefs.HasKey("banner_type"))
        {
            PlayerPrefs.SetInt("banner_type", banner_type);
        }
    }

    public void LoadDefaultValue()
    {
        my_mediation = PlayerPrefs.GetString("my_mediation", my_mediation);
        Ads_config_native = PlayerPrefs.GetString("ads_config_native", Ads_config_native);
        banner_collab = PlayerPrefs.GetInt("banner_collab", banner_collab);
        value_super_impression_ads = PlayerPrefs.GetInt("value_super_impression_ads", value_super_impression_ads);
        value_impress_ads = PlayerPrefs.GetInt("value_impress_ads", value_impress_ads);
        banner_type = PlayerPrefs.GetInt("banner_type", banner_type);

    }

    private void SaveValue()
    {
        PlayerPrefs.SetString("my_mediation", my_mediation);
        PlayerPrefs.SetString("ads_config_native", Ads_config_native);
        PlayerPrefs.SetInt("banner_collab", banner_collab);
        PlayerPrefs.SetInt("value_super_impression_ads", value_super_impression_ads);
        PlayerPrefs.SetInt("value_impress_ads", value_impress_ads);
        PlayerPrefs.SetInt("banner_type", banner_type);
        PlayerPrefs.Save();
    }
}
