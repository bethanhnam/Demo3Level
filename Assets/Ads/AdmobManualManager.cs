using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobManualManager : MonoBehaviour
{
    private bool isInit = false;
    private bool initAdmob = false;

    private List<StructID> listManualIDFA;
    private List<StructID> listManualIDRW;
    private List<StructID> listManualIDBN;
    private float delayTime;

    public void Init(List<StructID> _dataFA, List<StructID> _dataRW, List<StructID> _dataBN, float _delayTime)
    {
        delayTime = _delayTime;

        listManualIDFA = _dataFA;
        if (listManualIDFA == null || listManualIDFA == null || listManualIDFA.Count == 0)
        {
        }
        else
        {
            listManualIDFA.Sort();
        }

        listManualIDRW = _dataRW;
        if (listManualIDRW == null || listManualIDRW == null || listManualIDRW.Count == 0)
        {
        }
        else
        {
            listManualIDRW.Sort();
        }

        //listManualIDBN = _structIDsBN;
        //if (listManualIDBN == null || listManualIDBN.Count == 0)
        //{
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
                CallLoadFA();
                CallLoadRW();
                if (AdsControl.Instance.Banner_type == 0)
                {
                    AdsControl.Instance.AdmobBanner.LoadBannerAdMob();
                    AdsControl.Instance.isCanShowBanner = true;
                }
            });
        }
    }


    #region FA Admob---------------------------------------------------------------
    private InterstitialAd interstitialAd;
    private bool isLoadingFAAdmob;

    private double valueFA;

    private int indexInList;

    public void CallLoadFA()
    {
        if (listManualIDFA == null || listManualIDFA == null || listManualIDFA.Count == 0)
        {
            return;
        }

        if (indexInList >= listManualIDFA.Count)
        {
            indexInList = 0;
        }

        valueFA = listManualIDFA[listManualIDFA.Count - indexInList - 1].Value / 1000f;
        LoadInterstitialAdAdmob(listManualIDFA[listManualIDFA.Count - indexInList - 1].Id);
    }

    public void LoadInterstitialAdAdmob(string _idAds)
    {
        if (!isInit || isLoadingFAAdmob)
        {
            return;
        }

        isLoadingFAAdmob = true;
        // Clean up the old ad before loading a new one.
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
                    indexInList += 1;
                    Invoke("CallLoadFA", delayTime);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                interstitialAd = ad;
                RegisterEventHandlers(interstitialAd);
                indexInList = 0;
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
                    adValue.Value / 1000f,
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
            AdsManager.instance.CallCloseFA();
        };
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Interstitial ad full screen content closed.");
            AdsControl.Instance.isShowingAds = false;
            CallLoadFA();
            AdsManager.instance.CallCloseFA();
            AdsControl.Instance.ActiveBlockFaAds(false);
            AdsControl.Instance.CallActionFa();

        };
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);
            AdsControl.Instance.isShowingAds = false;
            CallLoadFA();
            AdsControl.Instance.ActiveBlockFaAds(false);
            AdsControl.Instance.CallActionFa();
        };
    }

    public void ShowFAAd()
    {
        if (AdsControl.Instance.isShowingAds)
        {
            return;
        }
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            AdsControl.Instance.isShowingAds = true;
            AdsControl.Instance.ActiveBlockFaAds(true, () =>
            {
                interstitialAd.Show();
            });
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public double GetFA()
    {
        if (initAdmob && isInit && interstitialAd != null && interstitialAd.CanShowAd())
        {
            return valueFA;
        }

        return -1;
    }
    #endregion FA Admob---------------------------------------------------------------

    #region RW Admob---------------------------------------------------------------
    private RewardedAd _rewardedAd;
    private bool isLoadingRWAdmob;


    private double valueRW;

    private int indexInListRW;

    public void CallLoadRW()
    {
        if (listManualIDRW == null || listManualIDRW == null || listManualIDRW.Count == 0)
        {
            return;
        }
        if (indexInListRW >= listManualIDRW.Count)
        {
            indexInListRW = 0;
        }

        valueRW = listManualIDRW[listManualIDRW.Count - indexInListRW - 1].Value / 1000f;
        LoadRewardedAd(listManualIDRW[listManualIDRW.Count - indexInListRW - 1].Id);
    }


    public void LoadRewardedAd(string id)
    {
        if (!isInit || isLoadingRWAdmob)
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
                    indexInListRW += 1;
                    isLoadingRWAdmob = false;
                    Invoke("CallLoadRW", delayTime);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                isLoadingRWAdmob = false;
                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);
                indexInListRW = 0;
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
                  adValue.Value / 1000f,
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
            CallLoadRW();
            AdsControl.Instance.ActiveBlockFaAds(false);
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
            AdsControl.Instance.isShowingAds = false;
            CallLoadRW();
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
            AdsControl.Instance.ActiveBlockFaAds(true, () =>
            {
                _rewardedAd.Show((Reward rw) =>
                {
                    AdsControl.Instance.isGetReward = true;
                }
            );
            });  
        }
        else
        {
            Debug.LogError("RW ad is not ready yet.");
        }
    }

    public double GetRW()
    {
        if (initAdmob && isInit && interstitialAd != null && interstitialAd.CanShowAd())
        {
            return valueRW;
        }

        return -1;
    }
    #endregion RW Admob---------------------------------------------------------------

    //#region BN Admob---------------------------------------------------------------

    //private BannerView _bannerView;

    //private void CreateBannerView(string id)
    //{
    //    DestroyBannerView();

    //    // Create a 320x50 banner at top of the screen
    //    _bannerView = new BannerView(id, AdSize.Banner, AdPosition.Bottom);
    //}


    //private void DestroyBannerView()
    //{
    //    if (_bannerView != null)
    //    {
    //        Debug.Log("Destroying banner view.");
    //        _bannerView.Destroy();
    //        _bannerView = null;
    //    }
    //}
    //#endregion BN Admob---------------------------------------------------------------
}
