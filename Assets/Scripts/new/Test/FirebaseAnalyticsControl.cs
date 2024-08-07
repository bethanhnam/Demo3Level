using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Analytics;
using Unity.VisualScripting;
using Firebase;
using Firebase.Extensions;
using System;
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
public enum TutorialStatus : short
{
    startTutor_1 = 0,
    completeTutor_1 = 1,
    tut_unscrew_start = 2,
    tut_unscrew_done = 3,
    tut_drill_start = 4,
    tut_drill_done = 5,
}
public enum LevelItem: short
{
    unscrew = 0,
    undo = 1,
    drill = 2
}
public class FirebaseAnalyticsControl : MonoBehaviour
{
    private bool isReady = false;

    public static FirebaseAnalyticsControl Instance;
    private void Awake()
    {
        Instance = this;
        CheckFireBaseAvailabe();
        DontDestroyOnLoad(gameObject);
    }


    #region properties

    //Screen_Home
    public void Screen_Home()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("return_home");
    }
    
    public void click_dailyRw()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("click_dailyRw");
    }

    public void CheckFireBaseAvailabe()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            Firebase.DependencyStatus dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Các phụ thuộc đã sẵn sàng, bạn có thể gọi các chức năng của Firebase tại đây
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }

    void InitializeFirebase()
    {
        // Các hàm khởi tạo Firebase
        FirebaseApp app = FirebaseApp.DefaultInstance;
        LoadingScreen.instance.isFirebaseInitialized = true;
        Debug.Log("Firebase is ready to use!");
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
    // tutorial status
    public void LogEventTutorialStatus(int level, TutorialStatus status)
    {
        //if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
        //    return;
        FirebaseAnalytics.LogEvent(FireBaseEventName.Tutorial, new Parameter[]
        {
            new Parameter(FireBaseEventName.level, level+1),
            new Parameter(FireBaseEventName.Tutorial_Status, status.ToString()),
        });
    }
    //LevelItem
    public void LogEventLevelItem(int level, LevelItem status)
    {
        FirebaseAnalytics.LogEvent(FireBaseEventName.Level_item, new Parameter[]
        {
            new Parameter(FireBaseEventName.level, level+1),
            new Parameter(FireBaseEventName.LevelItem, status.ToString()),
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
        {
            FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
            LogEventLevelStatus(LevelManagerNew.Instance.stage, LevelStatus.Break);
        }
    }

    // Map 1
    public void LogEventFixItem(int itemIndext) {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("Map_1_fix_done_" + itemIndext+1);
    }
    // Map 2 - Map 3 
    public void LogEventFixStageMap(int map,int stageIndext)
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("Map_"+map+"_fix_done_stage_" + (stageIndext + 1));
    }
    //Shop 
    public void visit_session()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("visit_session");
    }
    public void visit_total()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
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
    //Offer
    public void impr_session_noads_1()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("impr_session_noads_1");
    }
    public void impr_total_noads_1()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.SetUserProperty("impr_total_noads_1", PlayerPrefs.GetInt("impr_total_noads_1").ToString());
    }
    //Tutorial
    public void startTutor()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("startTutorVideo");
    }
    public void completeTutor()
    {
        FirebaseAnalytics.SetUserProperty(FireBaseEventName.level, LevelManagerNew.Instance.stage.ToString());
        FirebaseAnalytics.LogEvent("completeTutorVideo");
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
    //name
    public static string Level;
    // key
    public static string Status = "Status";
    public static string LevelItem = "LevelItem";
    public static string fix_done = "Fix_Done_";
    public static string fix_done_stage = "fix_done_stage_";
    public static string Tutorial_Status = "tutorial_status";

    //event
    public static string Screen_Home = "Screen_Home";
    public static string Level_status = "Level_status";
    public static string Level_item = "Level_item";
    public static string Level_story = "Level_story";
    public static string Endgames = "Endgames";
    public static string Map_1 = "Map_1_fix_item_";
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

    //tutorial_status
    public static string startTutor_1 = "startTutor_1";
    public static string completeTutor_1 = "completeTutor_1";
    public static string tut_unscrew_start = "tut_unscrew_start";
    public static string tut_unscrew_done = "tut_unscrew_done";
    public static string tut_drill_start = "tut_drill_start";
    public static string tut_drill_done = "tut_drill_done";


    //LevelItem
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

