using System.Collections;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using Newtonsoft.Json;
using UnityEngine;

public class NativeAdmobManager : MonoBehaviour
{
    private int index;
    private int indexContinue = 0;
    private bool isLoadingNativeAds;
    private bool isWaitingLoad;
    private NativeConfigRemote nativeConfigRemote;
    public NativeAd nativeAd;
    public float lastTimeLoadedNativeAds;

    public void InitNativeData()
    {
        try
        {
            if (!string.IsNullOrEmpty(RemoteConfigController.instance.Ads_config_native))
            {
                nativeConfigRemote = JsonConvert.DeserializeObject<NativeConfigRemote>(RemoteConfigController.instance.Ads_config_native);
            }
            else
            {
                nativeConfigRemote = null;
            }
        }
        catch
        {
            nativeConfigRemote = null;
        }
    }

    public void RequestNativeAds()
    {
        RequestNativeAd(index);
    }

    private void RequestNativeAd(int _index)
    {
        if (nativeConfigRemote == null)
        {
            return;
        }

        if (nativeAd != null)
        {
            return;
        }

        if (isLoadingNativeAds)
        {
            return;
        }

        if (isWaitingLoad)
        {
            return;
        }

        isLoadingNativeAds = true;

        string id = string.Empty;
        if (_index <= nativeConfigRemote.ListId.Count)
        {
            id = nativeConfigRemote.ListId[_index];
            _index += 1;
        }

        if (string.IsNullOrEmpty(id))
        {
            return;
        }

        index = _index;
        AdLoader adLoader = new AdLoader.Builder(id)
            .ForNativeAd()
            .Build();
        adLoader.OnNativeAdLoaded += AdLoader_OnNativeAdLoaded;
        adLoader.OnAdFailedToLoad += AdLoader_OnAdFailedToLoad;
        adLoader.LoadAd(new AdRequest());
    }

    private void AdLoader_OnNativeAdLoaded(object sender, NativeAdEventArgs e)
    {
        isLoadingNativeAds = false;
        nativeAd = e.nativeAd;
        index = 0;
        indexContinue = 0;
        lastTimeLoadedNativeAds = Time.time;
    }

    public void CallWhenDestroyNative(bool isCheck)
    {
        if (!isCheck)
        {
            return;
        }

        if (nativeConfigRemote == null)
        {
            return;
        }

        if (lastTimeLoadedNativeAds + nativeConfigRemote.TimeDestroy > Time.time)
        {
            return;
        }

        if (isLoadingNativeAds)
        {
            return;
        }

        if (isWaitingLoad)
        {
            return;
        }

        if (nativeAd != null)
        {
            nativeAd.Destroy();
            nativeAd = null;
        }

        isWaitingLoad = false;
        isLoadingNativeAds = false;
        index = 0;
        RequestNativeAd(index);
    }

    private void AdLoader_OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e)
    {
        isLoadingNativeAds = false;
        if (isWaitingLoad)
        {
            return;
        }
        index += 1;
        if (index >= nativeConfigRemote.ListId.Count)
        {
            indexContinue += 1;
            if (indexContinue <= 3)
            {
                index = 0;
                RequestNativeAd(index);
            }
            else
            {
                isWaitingLoad = true;
                Invoke("RequestNativeDelay", 180);
            }
        }
        else
        {
            RequestNativeAd(index);
        }
    }

    private void RequestNativeDelay()
    {
        isWaitingLoad = false;
        isLoadingNativeAds = false;
        index = 0;
        RequestNativeAd(index);
    }

    public void ClearNativeAds()
    {
        if (nativeAd != null)
        {
            nativeAd.Destroy();
        }
        if (nativeAd != null)
        {
            nativeAd = null;
        }

    }

    public bool CheckNative()
    {
        if (nativeAd != null && nativeConfigRemote.IsEnable)
        {
            return true;
        }
        else
        {
            RequestNativeAds();
        }

        return false;
    }
}
