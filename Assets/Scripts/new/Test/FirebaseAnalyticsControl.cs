using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
//using MyManager.Abstract;

public class FirebaseAnalyticsControl : MonoBehaviour
{
    private bool isReady = false;

    public static FirebaseAnalyticsControl Instance;
	private void Awake()
	{
        Instance = this;
        DontDestroyOnLoad(gameObject);
	}


	#region properties
	public void LogEventMenuPanelAccessSuccessfully(int a)
	{
		FirebaseAnalytics.LogEvent("Accessed_menupanel_successfully" + a);
	}
	public void LogEventShopPanelAccessSuccessfully(int a)
	{
		FirebaseAnalytics.LogEvent("Accessed_shoppanel_successfully" + a);
	}

	public void LogEventGamePlayAccessSuccessfully(int a)
	{
		FirebaseAnalytics.LogEvent("Gameplay_Accessed_Level_" + a);
	}

	public void LogEventGamePlayWin(int a)
    {
        if (a > 30)
            return;
        FirebaseAnalytics.LogEvent("Gameplay_Win_Level_" + a);
    }

    public void LogEventGamePlayLose(int a)
    {
        if (a > 30)
            return;
        FirebaseAnalytics.LogEvent("Gameplay_Lose_Level_" + a);
    }

	public void LogEventUnscrewUsed(int a)
	{
		FirebaseAnalytics.LogEvent("UnscrewUsed" + a);
	}

	public void LogEventUndoUsed(int a)
	{
		FirebaseAnalytics.LogEvent("UndoUsed" + a);
	}

	public void LogEventX2Reward(int a)
	{
		FirebaseAnalytics.LogEvent("X2Reward" + a);
	}

	public void LogEventUnscrewByAds(int a)
	{
		FirebaseAnalytics.LogEvent("UnscrewUsedByAds" + a);
	}

	public void LogEventUndoReplayByAds(int a)
	{
		FirebaseAnalytics.LogEvent("UndoReplayByAds" + a);
	}

	public void LogEventMoreTime(int a)
	{
		FirebaseAnalytics.LogEvent("MoreTime" + a);
	}

	public void LogEventSreen(string a)
    {
        FirebaseAnalytics.LogEvent("Screen_" + a);
    }

    #endregion
    #region event_REV_Ads
    private string countRevAdsName = "count_REV_Ads_total";

    private string isLogRevAds1 = "count_REV_Ads_1";
    private string isLogRevAds5 = "count_REV_Ads_5";
    private string isLogRevAds10 = "count_REV_Ads_10";

    public void CallREVAds(double value)
    {
        float a = PlayerPrefs.GetFloat(countRevAdsName);

        a += (float)value * 1000f;

        PlayerPrefs.SetFloat(countRevAdsName, a);

        if (PlayerPrefs.GetInt(isLogRevAds1) == 0)
        {
            if (a >= 1)
            {
                if (!isReady)
                {
                    return;
                }
                PlayerPrefs.SetInt(isLogRevAds1, 1);
                FirebaseAnalytics.LogEvent("fs_rev_ads_1_cent");
            }
        }

        if (PlayerPrefs.GetInt(isLogRevAds5) == 0)
        {
            if (a >= 5)
            {
                if (!isReady)
                {
                    return;
                }
                PlayerPrefs.SetInt(isLogRevAds5, 1);
                FirebaseAnalytics.LogEvent("fs_rev_ads_5_cent");
            }
        }

        if (PlayerPrefs.GetInt(isLogRevAds10) == 0)
        {
            if (a >= 10)
            {
                if (!isReady)
                {
                    return;
                }
                PlayerPrefs.SetInt(isLogRevAds10, 1);
                FirebaseAnalytics.LogEvent("fs_rev_ads_10_cent");
            }
        }
    }
    #endregion

    #region Ads_impress_value
    private string nameEventAdsImprss = "ad_impression_value";
    public void LogEvenAdsImpresssion(Parameter[] p)
    {
        if (!isReady)
        {
            return;
        }
        FirebaseAnalytics.LogEvent(nameEventAdsImprss, p);

        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, p);
    }
    #endregion
}
