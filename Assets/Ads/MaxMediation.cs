using System;
using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using Unity.Advertisement.IosSupport;
using UnityEngine;

public class MaxMediation : MonoBehaviour
{
    private string maxSDK = "";
    private string idFAMax = "";
    private string idRWMax = "";
    private string idBNMax = "";
    private string idOpenAds = "";

    private bool initMax;


    public void Init(string _maxSDK, string _idFaMax, string _idRWMax, string _idBnMax, string _idOpenAds)
    {
        maxSDK = _maxSDK;
        idFAMax = _idFaMax;
        idRWMax = _idRWMax;
        idBNMax = _idBnMax;
        idOpenAds = _idOpenAds;
#if UNITY_IOS
		Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");

		ATTrackingStatusBinding.RequestAuthorizationTracking();
#endif
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
        AdsControl.Instance.ActiveBlockFaAds(false);
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
        AdsControl.Instance.ActiveBlockFaAds(false);
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

    private double faValue;
    public double FaValue { get => faValue; }


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
        faValue = adInfo.Revenue;
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
            return;
        }

        if (MaxSdk.IsInterstitialReady(idFAMax))
        {
            AdsControl.Instance.isShowingAds = true;
            AdsControl.Instance.ActiveBlockFaAds(true, () =>
            {
                MaxSdk.ShowInterstitial(idFAMax);
            });
        }
        else
        {
            AdsControl.Instance.ActiveBlockFaAds(false);
        }
    }

    public double GetFA()
    {
        if (initMax && MaxSdk.IsInterstitialReady(idFAMax))
        {
            return faValue;
        }

        return -1;
    }
    #endregion Interstitial Ad Methods---------------------------------------------------------------------

    #region Rewarded Ad Methods----------------------------------------------------------------
    private int rewardedRetryAttempt;

    private double rwValue;
    public double RwValue { get => rwValue; }

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
        rwValue = adInfo.Revenue;
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
            AdsControl.Instance.ActiveBlockFaAds(false);
        }
    }

    public bool CheckRewardReady()
    {
        return MaxSdk.IsRewardedAdReady(idRWMax);
    }

    public double GetRW()
    {
        if (initMax && MaxSdk.IsInterstitialReady(idRWMax))
        {
            return rwValue;
        }

        return -1;
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
            MaxSdk.ShowBanner(idBNMax);
        }
    }

    public void HiddenBanner()
    {
        if (isShowingBanner)
        {
            isShowingBanner = false;
            MaxSdk.HideBanner(idBNMax);
        }
    }
    #endregion Banner Ad Methods-------------------------------------------------------
}
