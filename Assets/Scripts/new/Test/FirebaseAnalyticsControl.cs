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
	public void Screen_Home(int a)
	{
		FirebaseAnalytics.LogEvent("Screen_Home");
	}
	public void Screen_Shop(int a)
	{
		FirebaseAnalytics.LogEvent("Screen_Shop");
	}

	public void Gameplay_Level(int level)
    {
        string temp = "Gameplay_Level_" + (level +1).ToString() ;
        FirebaseAnalytics.LogEvent(temp);
	}

	public void Gameplay_Win(int a)
    {
        if (a > 30)
            return;
        FirebaseAnalytics.LogEvent("Gameplay_Win" + "_Level" + a);
    }

    public void Gameplay_Lose(int a)
    {
        if (a > 30)
            return;
        FirebaseAnalytics.LogEvent("Gameplay_Lose" + "_Level" + a);
    }

    public void Gameplay_Item_Unscrew(int a, int level)
	{
		FirebaseAnalytics.LogEvent("Gameplay_Item_Unscrew" + "_Level" + level);
	}
	public void Gameplay_Item_Undo(int a,int level)
	{
		FirebaseAnalytics.LogEvent("Gameplay_Item_Undo" + "_Level" + level );
	}
    public void LogEventUndo_RW_Change(int a)
    {
        FirebaseAnalytics.LogEvent("Undo_RW_Change");
    }
    public void Daily_RW_x2(int a)
	{
		FirebaseAnalytics.LogEvent("Daily_RW_x2");
	}
	public void Unscrew_RW_Change(int a)
	{
		FirebaseAnalytics.LogEvent("UnscrewUsedByAds");
	}
    public void RemoveAds_Click(int a)
    {
        FirebaseAnalytics.LogEvent("RemoveAds_Click");
    }
    public void Revive_Rw(int a)
    {
        FirebaseAnalytics.LogEvent("Revive_Rw");
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
