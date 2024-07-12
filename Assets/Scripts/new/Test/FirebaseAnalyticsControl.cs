﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Unity.VisualScripting;
//using MyManager.Abstract;
public enum LevelStatus : short
{
    start = 0,
    win = 1,
    fail = 2,
    Break = 3,
    retry = 4,
    revive = 5,
}
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

    //Screen_Home
    public void Screen_Home()
    {
        FirebaseAnalytics.LogEvent("return_home");
    }
    
    public void click_dailyRw()
    {
        FirebaseAnalytics.LogEvent("click_dailyRw");
    }

    // Level status
    public void LogEventLevelStatus(int level, LevelStatus status)
    {
        //if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
        //    return;
        FirebaseAnalytics.LogEvent(FireBaseEventName.Level_status, new Parameter[]
        {
            new Parameter(FireBaseEventName.level, level+1),
            new Parameter(FireBaseEventName.Status, status.ToString()),
        });
    }
    public void LogEventLevelStory(int level)
    {
        //if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
        //    return;
        FirebaseAnalytics.LogEvent(FireBaseEventName.Level_story, new Parameter[]
        {
            new Parameter(FireBaseEventName.level, level+1)
        });
    }


    private void OnApplicationQuit()
    {
        if (GamePlayPanelUIManager.Instance.gameObject.activeSelf)
            LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.Break);
    }

    // Map 1

    public void LogEventFixItem(int itemIndext) {
        FirebaseAnalytics.LogEvent(FireBaseEventName.Map_1, new Parameter[]
       {
            new Parameter(FireBaseEventName.fix_done, itemIndext+1),
       });
    }
    // Map 2 - Map 3 
    public void LogEventFixStageMap(string Piclevel,int stageIndext)
    {
        FirebaseAnalytics.LogEvent(FireBaseEventName.Map_+Piclevel+1, new Parameter[]
       {
            new Parameter(FireBaseEventName.fix_done_stage, stageIndext),
       });
    }
    //Shop 
    public void visit_session()
    {
        FirebaseAnalytics.LogEvent("visit_session");
    }
    public void visit_total()
    {
        FirebaseAnalytics.SetUserProperty("visit_total", PlayerPrefs.GetInt("visit_total").ToString());
    }
    public void BuyByAds(int packId )
    {
        string packName = "";
        switch (packId)
        {
            case 1:
                packName = "buy_combo1";
            break;
            case 2:
                packName = "buy_combo2";
                break;
            case 3:
                packName = "buy_gold";
                break;
            case 4:
                packName = "buy_drill";
                break;
            case 5:
                packName = "buy_unscrew";
                break;
            case 6:
                packName = "buy_undo";
                break;
        }
        FirebaseAnalytics.LogEvent(packName);
    }
    //booster
    public void Gameplay_Item_Unscrew(int level)
    {
        FirebaseAnalytics.LogEvent("unscrew_" + "_Level" + level+1);
    }
    public void Gameplay_Item_Undo(int level)
    {
        FirebaseAnalytics.LogEvent("undo" + "_Level" + level+1);
    }
    public void Gameplay_Item_Drill(int level)
    {
        FirebaseAnalytics.LogEvent("drill" + "_Level" + level+1);
    }
    //Offer
    public void impr_session_noads_1()
    {
        FirebaseAnalytics.LogEvent("impr_session_noads_1");
    }
    public void impr_total_noads_1()
    {
        FirebaseAnalytics.SetUserProperty("impr_total_noads_1", PlayerPrefs.GetInt("impr_total_noads_1").ToString());
    }

    //Tutorial
    public void startTutor()
    {
        FirebaseAnalytics.LogEvent("startTutor");
    }
    public void completeTutor()
    {
        FirebaseAnalytics.LogEvent("completeTutor");
    }

    //////////////////////////////////// New Tracking ///////////////////////////


    public void LogEventFirebase(string str)
    {
        FirebaseAnalytics.LogEvent(str);
    }

    //Screen_Home
    //public void LogEventReturn_home()
    //{
    //    FirebaseAnalytics.LogEvent(FireBaseEventName.return_home, new Parameter(FireBaseEventName.Screen_Home);
    //}

    //public void LogEventShopDaily(string name)
    //{
    //    FirebaseAnalytics.LogEvent(FireBaseEventName.Daily, new Parameter(FireBaseEventName.OfferType, name));
}
public class FireBaseEventName
{
    // key
    public static string Status = "Status";
    public static string fix_done = "Fix_Done_";
    public static string fix_done_stage = "fix_done_stage_";

    //event
    public static string Screen_Home = "Screen_Home";
    public static string Level_status = "Level_status";
    public static string Level_story = "Level_story";
    public static string Endgames = "Endgames";
    public static string Map_1 = "Map_1";
    public static string Map_ = "Map_";
    public static string Screen_Shop = "Screen_Shop";
    public static string Offer = "Offer";
    public static string Tutorial = "Tutorial";

    ////////////////////////////////////////////////////////name param

    //Screen_Home
    public static string return_home = "return_home";
    public static string click_shop = "click_shop";
    public static string click_dailyRw = "click_dailyRw";

    //level_status
    public static string level = "level";
    public static string win = "win";
    public static string fail = "fail";
    public static string Break = "break";
    public static string retry = "retry";
    public static string unscrew = "unscrew";
    public static string undo = "undo";
    public static string drill = "drill";

    //EndGame
    public static string revive = "revive";

    //Map_1
    public static string fix_done_1 = "fix_done_1";
    public static string fix_done_2 = "fix_done_2";
    public static string fix_done_3 = "fix_done_3";
    public static string fix_done_4 = "fix_done_4";

    //Map_2
    public static string fix_done_stage_1 = "fix_done_stage_1_map2";
    public static string fix_done_stage_2 = "fix_done_stage_2_map2";

    //Map_3
    public static string fix_done_stage_1_map3 = "fix_done_stage_1_map3";
    public static string fix_done_stage_2_map3 = "fix_done_stage_2_map3";
    public static string fix_done_stage_3_map3 = "fix_done_stage_3_map3";

    //Screen_Shop
    public static string visit_session = "visit_session";
    public static string visit_total = "visit_total";
    public static string buy_gold = "buy_gold";
    public static string buy_combo1 = "buy_combo1";
    public static string buy_combo2 = "buy_combo2";
    public static string buy_drill = "buy_drill";
    public static string buy_unscrew = "buy_unscrew";
    public static string buy_undo = "buy_undo";

    //Offer
    public static string impr_session_noads_1 = "impr_session_noads_1";
    public static string impr_total_noads_1 = "impr_total_noads_1";

    //Tutorial
    public static string start = "start";
    public static string complete = "complete";

    //value param
    public static string ItemDaily = "{0}_Daily";
    #endregion
    #region event_REV_Ads
    private string countRevAdsName = "count_REV_Ads_total";

    private string isLogRevAds1 = "count_REV_Ads_1";
    private string isLogRevAds5 = "count_REV_Ads_5";
    private string isLogRevAds10 = "count_REV_Ads_10";

    //public void CallREVAds(double value)
    //{
    //    float a = PlayerPrefs.GetFloat(countRevAdsName);

    //    a += (float)value * 1000f;

    //    PlayerPrefs.SetFloat(countRevAdsName, a);

    //    if (PlayerPrefs.GetInt(isLogRevAds1) == 0)
    //    {
    //        if (a >= 1)
    //        {
    //            if (!isReady)
    //            {
    //                return;
    //            }
    //            PlayerPrefs.SetInt(isLogRevAds1, 1);
    //            FirebaseAnalytics.LogEvent("fs_rev_ads_1_cent");
    //        }
    //    }

    //    if (PlayerPrefs.GetInt(isLogRevAds5) == 0)
    //    {
    //        if (a >= 5)
    //        {
    //            if (!isReady)
    //            {
    //                return;
    //            }
    //            PlayerPrefs.SetInt(isLogRevAds5, 1);
    //            FirebaseAnalytics.LogEvent("fs_rev_ads_5_cent");
    //        }
    //    }

    //    if (PlayerPrefs.GetInt(isLogRevAds10) == 0)
    //    {
    //        if (a >= 10)
    //        {
    //            if (!isReady)
    //            {
    //                return;
    //            }
    //            PlayerPrefs.SetInt(isLogRevAds10, 1);
    //            FirebaseAnalytics.LogEvent("fs_rev_ads_10_cent");
    //        }
    //    }
    //}
    #endregion

    //#region Ads_impress_value
    //private string nameEventAdsImprss = "ad_impression_value";
    //public void LogEvenAdsImpresssion(Parameter[] p)
    //{
    //    //if (!isReady)
    //    //{
    //    //    return;
    //    //}
    //    FirebaseAnalytics.LogEvent(nameEventAdsImprss, p);

    //    FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, p);
    //}
    //#endregion
}

