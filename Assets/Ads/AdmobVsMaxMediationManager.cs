using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.adjust.sdk;
using GoogleMobileAds.Api;
using System;
using Unity.Advertisement.IosSupport.Components;
using Unity.Advertisement.IosSupport;

public class AdmobVsMaxMediationManager : MonoBehaviour
{

    private bool isInit = false;
    private bool initAdmob = false;

    private List<StructID> listManualIDFA;
    private List<StructID> listManualIDRW;
    private List<StructID> listManualIDBN;
    private float valueAdd;

    private string maxSDK = "";
    private string idFAMax = "";
    private string idRWMax = "";
    private string idBNMax = "";
    private string idOpenAds = "";

    private bool initMax;

    public void Init(string _maxSDK, string _idFaMax, string _idRWMax, string _idBnMax, string _idOpenAds,
                    List<StructID> _structIDsFA, List<StructID> _structIDsRW, List<StructID> _structIDsBN, float _valueadd)
    {
        maxSDK = _maxSDK;
        idFAMax = _idFaMax;
        idRWMax = _idRWMax;
        idBNMax = _idBnMax;
        idOpenAds = _idOpenAds;

        if (!initMax)
        {
            initMax = true;
            MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration =>
            {
                InitOpenAds();
                InitializeInterstitialAds();
                InitializeRewardedAds();
                if (AdsControl.Instance.Banner_type == 0)
                {
                    InitializeBannerAds();
                    AdsControl.Instance.isCanShowBanner = true;
                }

#if UNITY_IOS
            //call ATT
            ATTrackingStatusBinding.RequestAuthorizationTracking();
            // check with iOS to see if the user has accepted or declined tracking
           var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
				if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.AUTHORIZED)
				{
					AdsControl.Instance.ManagerExistingPrivacySettings();
				}
#else
                AdsControl.Instance.ManagerExistingPrivacySettings();
                Debug.Log("Unity iOS Support: App Tracking Transparency status not checked, because the platform is not iOS.");
#endif
            };
            MaxSdk.SetSdkKey(maxSDK);
            MaxSdk.InitializeSdk();
        }
        valueAdd = _valueadd;

        listManualIDFA = _structIDsFA;
        if (listManualIDFA == null || listManualIDFA.Count == 0)
        {
        }
        else
        {
            listManualIDFA.Sort();
        }

        listManualIDRW = _structIDsRW;
        if (listManualIDRW == null || listManualIDRW.Count == 0)
        {
        }
        else
        {
            listManualIDRW.Sort();
        }

        //listManualIDBN = _structIDsBN;
        //if (listManualIDBN == null || listManualIDBN.Count == 0)
        //{
        //    isInit = false;
        //}
        //else
        //{
        //    listManualIDBN.Sort();
        //}

        InitAdmobSDK();
    }

    private void InitAdmobSDK()
    {
        if (!initAdmob)
        {
            initAdmob = true;
            MobileAds.Initialize(initStatus =>
            {
                //Debug.LogError("mobileads");
                isInit = true;
                if (AdsControl.Instance.Banner_type == 1)
                {
                    AdsControl.Instance.AdmobBanner.LoadBannerAdMob();
                    AdsControl.Instance.isCanShowBanner = true;
                }
            });
        }
    }

    #region Open Ads ----------------------------------------------------
    private int openAdsRetryAttempt;

    private void InitOpenAds()
    {
        MaxSdkCallbacks.AppOpen.OnAdLoadedEvent += AppOpen_OnAdLoadedEvent;
        MaxSdkCallbacks.AppOpen.OnAdHiddenEvent += OnAppOpenDismissedEvent;
        MaxSdkCallbacks.AppOpen.OnAdRevenuePaidEvent += AppOpen_OnAdRevenuePaidEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += AppOpen_OnAdDisplayedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayedEvent += AppOpen_OnAdDisplayedEvent;
        MaxSdkCallbacks.AppOpen.OnAdDisplayFailedEvent += AppOpen_OnAdDisplayFailedEvent;
        MaxSdkCallbacks.AppOpen.OnAdLoadFailedEvent += AppOpen_OnAdLoadFailedEvent;

        LoadOpenAds();
    }

    private void AppOpen_OnAdLoadFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2)
    {
        openAdsRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, openAdsRetryAttempt));


        Invoke("LoadOpenAds", (float)retryDelay);
    }

    private void AppOpen_OnAdDisplayFailedEvent(string arg1, MaxSdkBase.ErrorInfo arg2, MaxSdkBase.AdInfo arg3)
    {
        AdsControl.Instance.isShowingAds = false;
        MaxSdk.LoadAppOpenAd(idOpenAds);
    }

    private void AppOpen_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
    }

    private void AppOpen_OnAdLoadedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {
    }

    private void AppOpen_OnAdRevenuePaidEvent(string arg1, MaxSdkBase.AdInfo adInfo)
    {
        FirebaseEvent.LogEventAdsImpresstion(
                        AdjustConfig.AdjustAdRevenueSourceAppLovinMAX,
                        adInfo.AdUnitIdentifier,
                        adInfo.Placement,
                        adInfo.AdFormat,
                        adInfo.Revenue,
                        adInfo.NetworkName);
    }

    public void OnAppOpenDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        AdsControl.Instance.isShowingAds = false;
        MaxSdk.LoadAppOpenAd(idOpenAds);
    }

    private void LoadOpenAds()
    {
        MaxSdk.LoadAppOpenAd(idOpenAds);
    }

    public void ShowOpenAdIfReady()
    {
        if (AdsControl.Instance.isShowingAds)
        {
            return;
        }

        if (MaxSdk.IsAppOpenAdReady(idOpenAds))
        {
            AdsControl.Instance.isShowingAds = true;
            MaxSdk.ShowAppOpenAd(idOpenAds);
        }
        else
        {
            MaxSdk.LoadAppOpenAd(idOpenAds);
        }
    }
    #endregion Open Ads -------------------------------

    #region Interstitial Ad Methods---------------------------------------------------------------------
    private int interstitialRetryAttempt;

    private double faValueMax;
    public double FaValueMax { get => faValueMax; }


    private void InitializeInterstitialAds()
    {
        MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
        MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialFailedEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += InterstitialFailedToDisplayEvent;
        MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialDismissedEvent;
        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += OnInterstitialRevenuePaidEvent;
        MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += Interstitial_OnAdDisplayedEvent;

        LoadInterstitial();
    }

    private void LoadInterstitial()
    {
        MaxSdk.LoadInterstitial(idFAMax);
    }

    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        interstitialRetryAttempt = 0;
        faValueMax = adInfo.Revenue;
        LoadInterstitialAdAdmob(faValueMax);
    }

    private void OnInterstitialFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        interstitialRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, interstitialRetryAttempt));

        Debug.Log("Interstitial failed to load with error code: " + errorInfo.Code);

        Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void InterstitialFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial failed to display with error code: " + errorInfo.Code);
        AdsControl.Instance.isShowingAds = false;
        AdsManager.instance.CallCloseFA();
        LoadInterstitial();
        AdsControl.Instance.ActiveBlockFaAds(false);
        AdsControl.Instance.CallActionFa();
    }

    private void OnInterstitialDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Interstitial dismissed");
        AdsControl.Instance.isShowingAds = false;
        AdsManager.instance.CallCloseFA();
        LoadInterstitial();
        AdsControl.Instance.ActiveBlockFaAds(false);
        AdsControl.Instance.CallActionFa();
    }

    private void OnInterstitialRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FirebaseEvent.LogEventAdsImpresstion(
                       AdjustConfig.AdjustAdRevenueSourceAppLovinMAX,
                       adInfo.AdUnitIdentifier,
                       adInfo.Placement,
                       adInfo.AdFormat,
                       adInfo.Revenue,
                       adInfo.NetworkName);
    }
    private void Interstitial_OnAdDisplayedEvent(string arg1, MaxSdkBase.AdInfo arg2)
    {

    }

    public void ShowForceAds()
    {
        if (AdsControl.Instance.isShowingAds)
        {
            Debug.Log("Chạy vào case 1 khong show");
            return;
        }

        if ((initMax && MaxSdk.IsInterstitialReady(idFAMax)) && (interstitialAd != null && interstitialAd.CanShowAd()))
        {
            if (faValueMax > valueFAAdmob)
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    MaxSdk.ShowInterstitial(idFAMax);
                    Debug.Log("Chạy vào case 1 show 1");
                });
            }
            else
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    interstitialAd.Show();
                    Debug.Log("Chạy vào case 1 show 2");
                });
            }
        }
        else
        {
            if (initMax && MaxSdk.IsInterstitialReady(idFAMax))
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    MaxSdk.ShowInterstitial(idFAMax);
                    Debug.Log("Chạy vào case 1 show 3");
                });
            }
            else
            {
                if (interstitialAd != null && interstitialAd.CanShowAd())
                {
                    AdsControl.Instance.isShowingAds = true;
                    AdsControl.Instance.ActiveBlockFaAds(true, () =>
                    {
                        interstitialAd.Show();
                        Debug.Log("Chạy vào case 1 show 4");
                    });
                }
            }
        }
    }

    public bool GetFA()
    {
        if ((initMax && MaxSdk.IsInterstitialReady(idFAMax)) || (interstitialAd != null && interstitialAd.CanShowAd()))
        {
            return true;
        }

        return false;
    }

    public string GetIDFA(double _curV)
    {
        if (!isInit || isLoadingFAAdmob)
        {
            return string.Empty;
        }

        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            return string.Empty;
        }

        double x = _curV * 1000f + valueAdd;

        for (int i = 0; i < listManualIDFA.Count; i++)
        {
            if (i < listManualIDFA.Count - 1)
            {
                if (listManualIDFA[i].Value >= x)
                {
                    valueFAAdmob = listManualIDFA[i].Value;
                    return listManualIDFA[i].Id;
                }
            }
            else
            {
                valueFAAdmob = listManualIDFA[listManualIDFA.Count - 1].Value;
                return listManualIDFA[listManualIDFA.Count - 1].Id;
            }
        }

        return string.Empty;
    }


    private InterstitialAd interstitialAd;
    private bool isLoadingFAAdmob;

    private double valueFAAdmob;

    private IEnumerator CalLoadInter()
    {
        if (isInit && !isLoadingFAAdmob)
        {
            isLoadingFAAdmob = true;
            yield return new WaitForSecondsRealtime(0.3f);
            DestroyFAAdmob();
            yield return new WaitForSecondsRealtime(0.1f);
            isLoadingFAAdmob = false;
            LoadInterstitialAdAdmob(valueFAAdmob);
        }
    }

    public void LoadInterstitialAdAdmob(double v)
    {
        if (!isInit || isLoadingFAAdmob)
        {
            return;
        }

        string _idAds = GetIDFA(v);
        if (string.IsNullOrEmpty(_idAds))
        {
            return;
        }

        isLoadingFAAdmob = true;
        //
        // up the old ad before loading a new one.
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        // create our request used to load the ad.
        var adRequest = new AdRequest();


        // send the request to load the ad.
        InterstitialAd.Load(_idAds, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                isLoadingFAAdmob = false;
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
            });
    }

    private void RegisterEventHandlers(InterstitialAd ad)
    {
        ad.OnAdPaid += (AdValue adValue) =>
        {
            FirebaseEvent.LogEventAdsImpresstion(
                    AdjustConfig.AdjustAdRevenueSourceAdMob,
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    adValue.Value / 1000000f,
                    string.Empty);
        };
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        ad.OnAdClicked += () =>
        {
            Debug.Log("Interstitial ad was clicked.");
        };
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Interstitial ad full screen content opened.");
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            AdsControl.Instance.isShowingAds = false;
            AdsManager.instance.CallCloseFA();

            StartCoroutine(CalLoadInter());
            AdsControl.Instance.ActiveBlockFaAds(false);
            AdsControl.Instance.CallActionFa();
        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            AdsControl.Instance.isShowingAds = false;
            AdsManager.instance.CallCloseFA();

            StartCoroutine(CalLoadInter());
            AdsControl.Instance.ActiveBlockFaAds(false);
            AdsControl.Instance.CallActionFa();
        };
    }

    private void DestroyFAAdmob()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

    }

    #endregion Interstitial Ad Methods---------------------------------------------------------------------

    #region Rewarded Ad Methods----------------------------------------------------------------
    private int rewardedRetryAttempt;

    private double rwValueMax;
    public double RwValueMax { get => rwValueMax; }

    private void InitializeRewardedAds()
    {
        MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
        MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdFailedEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
        MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
        MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
        MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdDismissedEvent;
        MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;

        LoadRewardedAd();
    }

    private void LoadRewardedAd()
    {
        MaxSdk.LoadRewardedAd(idRWMax);
    }

    private void ShowRewardedAd()
    {
        if (MaxSdk.IsRewardedAdReady(idRWMax))
        {
            MaxSdk.ShowRewardedAd(idRWMax);
        }
        else
        {
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad loaded");
        rewardedRetryAttempt = 0;
        rwValueMax = adInfo.Revenue;
        LoadRewardedAdMob(rwValueMax);
    }

    private void OnRewardedAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        rewardedRetryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, rewardedRetryAttempt));

        Debug.Log("Rewarded ad failed to load with error code: " + errorInfo.Code);

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad failed to display with error code: " + errorInfo.Code);
        AdsControl.Instance.isShowingAds = false;
        LoadRewardedAd();
        AdsControl.Instance.ActiveBlockFaAds(false);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad displayed");
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad clicked");
    }

    private void OnRewardedAdDismissedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        Debug.Log("Rewarded ad dismissed");
        AdsControl.Instance.isShowingAds = false;
        LoadRewardedAd();
        AdsControl.Instance.ActiveBlockFaAds(false);
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        AdsControl.Instance.isGetReward = true;
    }
    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FirebaseEvent.LogEventAdsImpresstion(
                        AdjustConfig.AdjustAdRevenueSourceAppLovinMAX,
                        adInfo.AdUnitIdentifier,
                        adInfo.Placement,
                        adInfo.AdFormat,
                        adInfo.Revenue,
                        adInfo.NetworkName);
    }

    public void ShowRewarded()
    {
        if (MaxSdk.IsRewardedAdReady(idRWMax) && (isInit && _rewardedAd != null && _rewardedAd.CanShowAd()))
        {
            if (rwValueMax > valueRWAdmob)
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    MaxSdk.ShowRewardedAd(idRWMax);
                });
            }
            else
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    _rewardedAd.Show((Reward rw) =>
                    {
                        AdsControl.Instance.isGetReward = true;
                    }
                    );
                });
            }
        }
        else
        {
            if (MaxSdk.IsRewardedAdReady(idRWMax))
            {
                AdsControl.Instance.isShowingAds = true;
                AdsControl.Instance.ActiveBlockFaAds(true, () =>
                {
                    MaxSdk.ShowRewardedAd(idRWMax);
                });
            }
            else
            {
                if (isInit && _rewardedAd != null && _rewardedAd.CanShowAd())
                {
                    AdsControl.Instance.isShowingAds = true;
                    AdsControl.Instance.ActiveBlockFaAds(true, () =>
                    {
                        _rewardedAd.Show((Reward rw) =>
                        {
                            AdsControl.Instance.isGetReward = true;
                        }
                        );
                    });         
                }
            }
        }
    }

    public bool CheckRewardReady()
    {
        return (MaxSdk.IsRewardedAdReady(idRWMax) || (isInit && _rewardedAd != null && _rewardedAd.CanShowAd()));
    }

    public string GetIDRW(double _curV)
    {
        if (!isInit || isLoadingRWAdmob)
        {
            return string.Empty;
        }

        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            return string.Empty;
        }

        double x = _curV * 1000f + valueAdd;

        for (int i = 0; i < listManualIDRW.Count; i++)
        {
            if (i < listManualIDRW.Count - 1)
            {
                if (listManualIDRW[i].Value >= x)
                {
                    valueRWAdmob = listManualIDRW[i].Value;
                    return listManualIDRW[i].Id;
                }
            }
            else
            {
                valueRWAdmob = listManualIDRW[listManualIDRW.Count - 1].Value;
                return listManualIDRW[listManualIDRW.Count - 1].Id;
            }
        }

        return string.Empty;
    }

    private RewardedAd _rewardedAd;
    private bool isLoadingRWAdmob;


    private double valueRWAdmob;

    public void LoadRewardedAdMob(double x)
    {
        if (!isInit || isLoadingRWAdmob)
        {
            return;
        }

        string id = GetIDRW(x);
        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        isLoadingRWAdmob = true;
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(id, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    isLoadingRWAdmob = false;
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                isLoadingRWAdmob = false;
                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
            });
    }

    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            FirebaseEvent.LogEventAdsImpresstion(
                  AdjustConfig.AdjustAdRevenueSourceAdMob,
                  string.Empty,
                  string.Empty,
                  string.Empty,
                  adValue.Value / 1000000f,
                  string.Empty);
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            AdsControl.Instance.isShowingAds = false;
            StartCoroutine(CalLoadRW());
            AdsControl.Instance.ActiveBlockFaAds(false);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            AdsControl.Instance.isShowingAds = false;
            StartCoroutine(CalLoadRW());
            AdsControl.Instance.ActiveBlockFaAds(false);
        };
    }

    public void ShowAdRW()
    {
        if (AdsControl.Instance.isShowingAds)
        {
            return;
        }
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            Debug.Log("Showing Rw ad.");
            AdsControl.Instance.isShowingAds = true;
            _rewardedAd.Show((Reward rw) =>
            {
                AdsControl.Instance.isGetReward = true;
            }
            );
        }
        else
        {
            Debug.LogError("RW ad is not ready yet.");
        }
    }

    private IEnumerator CalLoadRW()
    {
        if (isInit && !isLoadingRWAdmob)
        {
            isLoadingRWAdmob = true;
            yield return new WaitForSecondsRealtime(0.3f);
            DestroyRWAdmob();
            yield return new WaitForSecondsRealtime(0.1f);
            isLoadingRWAdmob = false;
            LoadRewardedAdMob(valueFAAdmob);
        }
    }

    private void DestroyRWAdmob()
    {
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }
    }


    #endregion Rewarded Ad Methods----------------------------------------------------------------

    #region Banner Ad Methods-------------------------------------------------------
    private bool isShowingBanner = false;

    private void InitializeBannerAds()
    {
        MaxSdkCallbacks.Banner.OnAdLoadedEvent += OnBannerAdLoadedEvent;
        MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += OnBannerAdFailedEvent;
        MaxSdkCallbacks.Banner.OnAdClickedEvent += OnBannerAdClickedEvent;
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += OnBannerAdRevenuePaidEvent;

        MaxSdk.CreateBanner(idBNMax, MaxSdkBase.BannerPosition.BottomCenter);
        MaxSdk.SetBannerExtraParameter(idBNMax, "adaptive_baner", "true");

        MaxSdk.HideBanner(idBNMax);
        isShowingBanner = false;
    }

    private void OnBannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {

    }

    private void OnBannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
    }

    private void OnBannerAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
    }

    private void OnBannerAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        FirebaseEvent.LogEventAdsImpresstion(
                       AdjustConfig.AdjustAdRevenueSourceAppLovinMAX,
                       adInfo.AdUnitIdentifier,
                       adInfo.Placement,
                       adInfo.AdFormat,
                       adInfo.Revenue,
                       adInfo.NetworkName);
    }

    public void ShowBanner()
    {
        if (!isShowingBanner)
        {
            isShowingBanner = true;
            if (AdsControl.Instance.Banner_type == 0)
            {
                MaxSdk.ShowBanner(idBNMax);
            }
            else
            {
                AdsControl.Instance.AdmobBanner.ShowAdmobBanner();
            }
        }
    }

    public void HiddenBanner()
    {
        if (isShowingBanner)
        {
            isShowingBanner = false;
            if (AdsControl.Instance.Banner_type == 0)
            {
                MaxSdk.HideBanner(idBNMax);
            }
            else
            {
                AdsControl.Instance.AdmobBanner.HiddenAdmobBanner();
            }
        }
    }
    #endregion Banner Ad Methods-------------------------------------------------------
}
