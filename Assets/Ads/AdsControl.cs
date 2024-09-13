using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Firebase.Analytics;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using com.adjust.sdk;
using Unity.VisualScripting;
using GoogleMobileAds.Ump.Api;

public class AdsControl : MonoBehaviour
{
    public static AdsControl Instance;

    [SerializeField]
    private string defaultData;

    private string nameDefaultData = "data_adsconfig_default";

    private DataAdsMediationConfig dataAdsMediationConfig;

    private bool isInit;

    [SerializeField]
    private MaxMediation maxMediation;
    [SerializeField]
    private AdmobManualManager admobManualManager;
    [SerializeField]
    private AdmobAutoFloorManager admobAutoFloorManager;
    [SerializeField]
    private AdmobVsMaxMediationManager admobVsMaxMediationManager;
    [SerializeField]
    private NativeAdmobManager nativeAdmobManager;
    [SerializeField]
    private AdmobBanner admobBanner;

    public bool isShowingAds;
    public bool isGetReward;

    [SerializeField]
    private int banner_collab;
    [SerializeField]
    private int banner_type;

    private Action rwAction;

    private Action FAACtionDone;

    public NativeAdmobManager NativeAdmobManager { get => nativeAdmobManager; }
    public int Banner_type { get => banner_type; }
    public int Banner_collab { get => banner_collab; }
    public AdmobBanner AdmobBanner { get => admobBanner; }

    public bool isCanShowBanner;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isGetReward)
        {
            isGetReward = false;
            if (rwAction != null)
            {
                rwAction();
                rwAction = null;
            }
        }
    }

    public void Init(string data)
    {
        if (isInit)
        {
            return;
        }

        banner_collab = RemoteConfigController.instance.Banner_collab;
        banner_type = RemoteConfigController.instance.Banner_type;

        nativeAdmobManager.InitNativeData();

        try
        {
            if (!string.IsNullOrEmpty(data))
            {
                dataAdsMediationConfig = JsonConvert.DeserializeObject<DataAdsMediationConfig>(data);
                if (dataAdsMediationConfig == null)
                {
                    dataAdsMediationConfig = JsonConvert.DeserializeObject<DataAdsMediationConfig>(PlayerPrefs.GetString(nameDefaultData, defaultData));
                }
                else
                {
                    PlayerPrefs.SetString(nameDefaultData, data);
                }
                isInit = true;
            }
            else
            {
                dataAdsMediationConfig = JsonConvert.DeserializeObject<DataAdsMediationConfig>(PlayerPrefs.GetString(nameDefaultData, defaultData));
                isInit = true;
            }
        }
        catch
        {
            dataAdsMediationConfig = null;
            isInit = true;
        }
        if (dataAdsMediationConfig != null)
        {
            admobBanner.Init(dataAdsMediationConfig.IdBNAdmob);
            switch (dataAdsMediationConfig.TypeMediation)
            {
                case 0:
                    admobVsMaxMediationManager.Init(
                                        dataAdsMediationConfig.MaxSDKKey,
                                        dataAdsMediationConfig.IdFA,
                                        dataAdsMediationConfig.IdRW,
                                        dataAdsMediationConfig.IdBN,
                                        dataAdsMediationConfig.IdOpenAds,
                                        dataAdsMediationConfig.ListIDAddValueFA,
                                        dataAdsMediationConfig.ListIDAddValueRW,
                                        dataAdsMediationConfig.ListIDAddValueBN,
                                        dataAdsMediationConfig.ValueAdd);
                    break;
                case 1:
                    maxMediation.Init(
                                        dataAdsMediationConfig.MaxSDKKey,
                                        dataAdsMediationConfig.IdFA,
                                        dataAdsMediationConfig.IdRW,
                                        dataAdsMediationConfig.IdBN,
                                        dataAdsMediationConfig.IdOpenAds);
                    admobManualManager.Init(
                                        dataAdsMediationConfig.ListManualIDFA,
                                        dataAdsMediationConfig.ListManualIDRW,
                                        dataAdsMediationConfig.ListManualIDBN,
                                        dataAdsMediationConfig.TimeDelayLoad);
                    break;
                case 2:
                    maxMediation.Init(
                                      dataAdsMediationConfig.MaxSDKKey,
                                      dataAdsMediationConfig.IdFA,
                                      dataAdsMediationConfig.IdRW,
                                      dataAdsMediationConfig.IdBN,
                                      dataAdsMediationConfig.IdOpenAds);
                    admobAutoFloorManager.Init(
                                        dataAdsMediationConfig.DataAdmobAutoFloorFA,
                                        dataAdsMediationConfig.DataAdmobAutoFloorRW,
                                        dataAdsMediationConfig.DataAdmobAutoFloorBN,
                                        dataAdsMediationConfig.TimeDelayLoad);
                    break;
            }
        }
    }

    public void ManagerExistingPrivacySettings()
    {
        if (MaxSdk.GetSdkConfiguration().ConsentFlowUserGeography == MaxSdkBase.ConsentFlowUserGeography.Gdpr)
        {
            var cmpService = MaxSdk.CmpService;

            cmpService.ShowCmpForExistingUser(error =>
            {
                if (null == error)
                {
                    // The CMP alert was shown successfully.
                }
                else
                {
                    // The CMP alert was shown fail.
                }
            });
        }
    }

    public void ShowBannerFirstGame()
    {
        StartCoroutine(CallBanner());
    }

    private IEnumerator CallBanner()
    {
        while (!isCanShowBanner)
        {
            yield return null;
        }
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            yield return null;
        }
        ShowBanner();
    }

    public bool CheckFA()
    {
        if (isInit)
        {
            switch (dataAdsMediationConfig.TypeMediation)
            {
                case 0:
                    return admobVsMaxMediationManager.GetFA();
                case 1:
                    return (maxMediation.GetFA() != -1 || admobManualManager.GetFA() != -1);
                case 2:
                    return (maxMediation.GetFA() != -1 || admobAutoFloorManager.GetFA());
            }
        }
        return false;
    }

    public void ShowFAAds(Action a)
    {
        if (isInit)
        {
            Debug.Log(isInit + "init chưa");
            if (dataAdsMediationConfig != null)
            {
                Debug.Log("dataAdsMediationConfig khong null");
                Debug.Log("dataAdsMediationConfig.TypeMediation" + dataAdsMediationConfig.TypeMediation);
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        if (admobVsMaxMediationManager.GetFA())
                        {
                            Debug.Log("Chạy vào case 1 a");
                            FAACtionDone = a;
                            admobVsMaxMediationManager.ShowForceAds();
                        }
                        Debug.Log("Chạy vào case 1");
                        break;
                    case 1:
                        if (admobManualManager.GetFA() > maxMediation.GetFA())
                        {
                            FAACtionDone = a;
                            admobManualManager.ShowFAAd();
                        }
                        else
                        {
                            FAACtionDone = a;
                            maxMediation.ShowForceAds();
                        }
                        Debug.Log("Chạy vào case 2");
                        break;
                    case 2:
                        if (admobAutoFloorManager.GetFA())
                        {
                            FAACtionDone = a;
                            admobAutoFloorManager.ShowFAAd();
                        }
                        else
                        {
                            FAACtionDone = a;
                            maxMediation.ShowForceAds();
                        }
                        Debug.Log("Chạy vào case 3");
                        break;
                }
            }
        }
    }

    public bool CheckRW()
    {
        if (isInit)
        {
            if (dataAdsMediationConfig != null)
            {
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        return admobVsMaxMediationManager.CheckRewardReady();
                    case 1:
                        return (admobManualManager.GetRW() != -1 || maxMediation.GetRW() != -1);
                    case 2:
                        return (admobAutoFloorManager.GetRW() || maxMediation.GetRW() != -1);
                }
            }
        }

        return false;
    }

    public void ShowReward(Action _a)
    {
        rwAction = _a;

        if (isInit)
        {
            if (dataAdsMediationConfig != null)
            {
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        admobVsMaxMediationManager.ShowRewarded();
                        break;
                    case 1:
                        if (admobManualManager.GetRW() > maxMediation.GetRW())
                        {
                            admobManualManager.ShowAdRW();
                        }
                        else
                        {
                            maxMediation.ShowRewarded();
                        }
                        break;
                    case 2:
                        if (admobAutoFloorManager.GetRW())
                        {
                            admobAutoFloorManager.ShowAdRW();
                        }
                        else
                        {
                            maxMediation.ShowRewarded();
                        }
                        break;
                }
            }
        }
    }

    public void ShowBanner()
    {
        if (isInit)
        {
            if (dataAdsMediationConfig != null)
            {
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        admobVsMaxMediationManager.ShowBanner();
                        break;
                    case 1:
                        if (banner_type == 0)
                        {
                            maxMediation.ShowBanner();
                        }
                        else
                        {
                            admobBanner.ShowAdmobBanner();
                        }
                        break;
                    case 2:
                        if (banner_type == 0)
                        {
                            maxMediation.ShowBanner();
                        }
                        else
                        {
                            admobBanner.ShowAdmobBanner();
                        }
                        break;
                }
            }
        }
    }

    public void HiddenBanner()
    {
        if (isInit)
        {
            if (dataAdsMediationConfig != null)
            {
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        admobVsMaxMediationManager.HiddenBanner();
                        break;
                    case 1:
                        if (banner_type == 0)
                        {
                            maxMediation.HiddenBanner();
                        }
                        else
                        {
                            admobBanner.HiddenAdmobBanner();
                        }
                        break;
                    case 2:
                        if (banner_type == 0)
                        {
                            maxMediation.HiddenBanner();
                        }
                        else
                        {
                            admobBanner.HiddenAdmobBanner();
                        }
                        break;
                }
            }
        }
    }

    public void ShowOpenAds()
    {
        if (isInit)
        {
            if (dataAdsMediationConfig != null)
            {
                switch (dataAdsMediationConfig.TypeMediation)
                {
                    case 0:
                        admobVsMaxMediationManager.ShowOpenAdIfReady();
                        break;
                    case 1:
                        maxMediation.ShowOpenAdIfReady();
                        break;
                    case 2:
                        maxMediation.ShowOpenAdIfReady();
                        break;
                }
            }
        }
    }

    public void CallActionFa()
    {
        if (FAACtionDone != null)
        {
            FAACtionDone();
            FAACtionDone = null;
        }
    }

    public void ActiveBlockFaAds(bool value,Action action = null)
    {
        if (UIManagerNew.Instance != null && UIManagerNew.Instance.gameObject != null)
        {
            UIManagerNew.Instance.ActiveBlockFaAds(value);
            if (action != null)
            {
                DOVirtual.DelayedCall(1f, () =>
                {
                    action();
                });
            }
        }
    }
}


[Serializable]
public class DataAdsMediationConfig
{
    [SerializeField]
    private int typeMediation;
    [SerializeField]
    private string maxSDKKey;
    [SerializeField]
    private string idOpenAds;
    [SerializeField]
    private string idFA;
    [SerializeField]
    private string idRW;
    [SerializeField]
    private string idBN;
    [SerializeField]
    private DataAdmobAutoFloor dataAdmobAutoFloorFA;
    [SerializeField]
    private DataAdmobAutoFloor dataAdmobAutoFloorRW;
    [SerializeField]
    private DataAdmobAutoFloor dataAdmobAutoFloorBN;
    [SerializeField]
    private List<StructID> listManualIDFA;
    [SerializeField]
    private List<StructID> listManualIDRW;
    [SerializeField]
    private List<StructID> listManualIDBN;
    [SerializeField]
    private List<StructID> listIDAddValueFA;
    [SerializeField]
    private List<StructID> listIDAddValueRW;
    [SerializeField]
    private List<StructID> listIDAddValueBN;
    [SerializeField]
    private float timeDelayLoad;
    [SerializeField]
    private float valueAdd;
    [SerializeField]
    private string idBNAdmob;

    public int TypeMediation { get => typeMediation; set => typeMediation = value; }
    public string MaxSDKKey { get => maxSDKKey; set => maxSDKKey = value; }
    public string IdOpenAds { get => idOpenAds; set => idOpenAds = value; }
    public string IdFA { get => idFA; set => idFA = value; }
    public string IdRW { get => idRW; set => idRW = value; }
    public string IdBN { get => idBN; set => idBN = value; }
    public DataAdmobAutoFloor DataAdmobAutoFloorFA { get => dataAdmobAutoFloorFA; set => dataAdmobAutoFloorFA = value; }
    public List<StructID> ListManualIDFA { get => listManualIDFA; set => listManualIDFA = value; }
    public List<StructID> ListIDAddValueFA { get => listIDAddValueFA; set => listIDAddValueFA = value; }
    public float TimeDelayLoad { get => timeDelayLoad; set => timeDelayLoad = value; }
    public DataAdmobAutoFloor DataAdmobAutoFloorRW { get => dataAdmobAutoFloorRW; set => dataAdmobAutoFloorRW = value; }
    public DataAdmobAutoFloor DataAdmobAutoFloorBN { get => dataAdmobAutoFloorBN; set => dataAdmobAutoFloorBN = value; }
    public List<StructID> ListManualIDRW { get => listManualIDRW; set => listManualIDRW = value; }
    public List<StructID> ListManualIDBN { get => listManualIDBN; set => listManualIDBN = value; }
    public List<StructID> ListIDAddValueRW { get => listIDAddValueRW; set => listIDAddValueRW = value; }
    public List<StructID> ListIDAddValueBN { get => listIDAddValueBN; set => listIDAddValueBN = value; }
    public float ValueAdd { get => valueAdd; set => valueAdd = value; }
    public string IdBNAdmob { get => idBNAdmob; set => idBNAdmob = value; }
}

[Serializable]
public class DataAdmobAutoFloor
{
    [SerializeField]
    private List<StructID> listManualID;
    [SerializeField]
    private string idHi;
    [SerializeField]
    private string idMe;

    public List<StructID> ListManualID { get => listManualID; set => listManualID = value; }
    public string IdHi { get => idHi; set => idHi = value; }
    public string IdMe { get => idMe; set => idMe = value; }
}

[Serializable]
public class StructID : IComparable<StructID>
{
    [SerializeField]
    private string id;
    [SerializeField]
    private float value;

    public string Id { get => id; set => id = value; }
    public float Value { get => value; set => this.value = value; }

    public int CompareTo(StructID other)
    {
        if (other.Value < value)
        {
            return +1;
        }
        else
        {
            if (other.Value > value)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}