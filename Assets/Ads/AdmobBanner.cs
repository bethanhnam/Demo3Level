using System;
using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using GoogleMobileAds.Api;
using UnityEngine;

public class AdmobBanner : MonoBehaviour
{
    string _adUnitId;
    private bool isShowing = false;

    public void Init(string id)
    {
        _adUnitId = id;
    }

    private BannerView _bannerView;

    private int bannerRetryAttempt;

    private void CreateBannerView()
    {
        // If we already have a banner, destroy the old one.
        if (_bannerView != null)
        {
            DestroyBannerView();
        }

        // Create a 320x50 banner at top of the screen
        _bannerView = new BannerView(_adUnitId, AdSize.Banner, AdPosition.Bottom);
        ListenToAdEvents();
    }

    public void LoadBannerAdMobAgain()
    {
        var adRequest = new AdRequest();

        // send the request to load the ad.
        _bannerView.LoadAd(adRequest);
    }

    public void LoadBannerAdMob()
    {
        // create an instance of a banner view first.
        if (_bannerView == null)
        {
            CreateBannerView();
        }

        HiddenAdmobBanner();

        // create our request used to load the ad.
        var adRequest = new AdRequest();
        if (AdsControl.Instance.Banner_collab == 0)
        {
            adRequest.Extras.Add("collapsible", "bottom");
        }
        // send the request to load the ad.
        _bannerView.LoadAd(adRequest);
    }

    private void ListenToAdEvents()
    {
        // Raised when an ad is loaded into the banner view.
        _bannerView.OnBannerAdLoaded += () =>
        {
            bannerRetryAttempt = 0;
        };
        // Raised when an ad fails to load into the banner view.
        _bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            bannerRetryAttempt++;
            double retryDelay = Math.Pow(2, Math.Min(6, bannerRetryAttempt));
            Invoke("LoadBannerAdMobAgain", (float)retryDelay);
        };
        // Raised when the ad is estimated to have earned money.
        _bannerView.OnAdPaid += (AdValue adValue) =>
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
        _bannerView.OnAdImpressionRecorded += () =>
        {
        };
        // Raised when a click is recorded for an ad.
        _bannerView.OnAdClicked += () =>
        {
        };
        // Raised when an ad opened full screen content.
        _bannerView.OnAdFullScreenContentOpened += () =>
        {
        };
        // Raised when the ad closed full screen content.
        _bannerView.OnAdFullScreenContentClosed += () =>
        {
        };
    }

    public void DestroyBannerView()
    {
        if (_bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            _bannerView.Destroy();
            _bannerView = null;
        }
    }

    public void ShowAdmobBanner()
    {
        if (!isShowing && _bannerView != null)
        {
            _bannerView.Show();
            isShowing = true;
        }
    }

    public void HiddenAdmobBanner()
    {
        if (isShowing)
        {
            isShowing = false;
        }
        if (_bannerView != null)
        {
            _bannerView.Hide();
        }
    }
}
