﻿using DG.Tweening;
using Facebook.Unity;
using GoogleMobileAds.Ump.Api;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using static WeeklyEventController;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    public WeeklyEventController[] weeklyEventControllers;

    public WeeklyEventController weeklyEventTreasureClimb;
    public WeeklyEventController weeklyEventHauntedTreasure;

    public WeeklyEventController weeklyEvent;
    public Sprite weeklyEventItemSprite;

    public WeeklyEventPrefab[] WeeklyEventPrefabs;
    public WeeklyEventPrefab currentWeeklyEventPrefab;

    // config
    WeeklyEventDataConfig weeklyEventConfig;
    public bool needResetData = false;

    public List<int> weeklyEventItemColors = new List<int>();
    public int selectedColorIndex = 0;

    private void Start()
    {
        instance = this;
    }
    [Button("CheckForWeeklyEvent")]
    public void CheckForWeeklyEvent()
    {

        // treasure climb
        //if (LevelManagerNew.Instance.stage >= 10)
        //{
        //    if (weeklyEventTreasureClimb != null)
        //    {
        //        if (weeklyEventTreasureClimb.eventStaus == WeeklyEventController.EventStaus.running)
        //        {
        //            CheckForEndTime("TreasureClimb", weeklyEventTreasureClimb, weeklyEventControllers[0]);
        //        }
        //        else
        //        if (CheckForDate("TreasureClimb", weeklyEventTreasureClimb, weeklyEventControllers[0]))
        //        {
        //            weeklyEventTreasureClimb.ResetData();
        //            weeklyEventTreasureClimb.eventStaus = WeeklyEventController.EventStaus.running;
        //            CretaWeeklyEvent(weeklyEventTreasureClimb);
        //        }
        //    }
        //    else
        //    {
        //        SetNewData("TreasureClimb", weeklyEventTreasureClimb);
        //        CretaWeeklyEvent(weeklyEventTreasureClimb);
        //        SaveData("TreasureClimb", weeklyEventTreasureClimb);
        //    }
        //}

        // weekly event
        if (LevelManagerNew.Instance.stage >= 8)
        {
            if (weeklyEvent != null)
            {
                if (needResetData == true)
                {
                    needResetData = false;
                    UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                    weeklyEvent.ResetData();
                    weeklyEvent.eventStaus = EventStaus.running;
                    UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                    UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(true);
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                    UIManagerNew.Instance.WeeklyEventPanel.LoadData();
                    RandomItemColor();
                    UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                    UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                    SaveData("WeeklyEvent", weeklyEvent);
                    UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                    return;
                }
                Debug.LogError("chaay vao weeklyEvent != null");

                weeklyEventItemSprite = weeklyEventControllers[0].weeklyEventItemColor[weeklyEvent.colorIndex].weeklyEventBarColor;
                UIManagerNew.Instance.ButtonMennuManager.collectImage.sprite = weeklyEventItemSprite;
                Debug.LogError("gan xong sprite");
                if (weeklyEvent.eventStaus == WeeklyEventController.EventStaus.running)
                {
                    Debug.LogError("dang running");
                    FirebaseAnalyticsControl.Instance.LogEventevent_weekly();
                    Debug.LogError("chay qua ban firebase");
                    DateTime endTime;
                    try
                    {
                        Debug.LogError("weeklyEvent.endEventDate " + weeklyEvent.endEventDate);
                        var x = JsonConvert.DeserializeObject<long>(weeklyEvent.endEventDate);
                        Debug.LogError("x " + x);
                        endTime = new DateTime(x);
                        //endTime.AddDays(1);
                        Debug.LogError("end " + endTime);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("weeklyEvent.endEventDate " + weeklyEvent.endEventDate);
                        var x = DateTime.Parse(weeklyEvent.endEventDate);
                        endTime = x;
                        Debug.LogError("x " + x);
                        //endTime.AddDays(1);
                        Debug.LogError("end " + endTime);
                    }
                    Debug.LogError("end " + endTime);
                    Debug.LogError("chuyen doi duoc time cua weeklyEvent");
                    if (HasTimeExpired(endTime))
                    {
                        Debug.LogError("het time");
                        PlayerPrefs.SetString("FirstWeeklyEvent", "false");
                        UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                        weeklyEvent.ResetData();
                        weeklyEvent.eventStaus = EventStaus.running;
                        UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                        UIManagerNew.Instance.WeeklyEventPanel.LoadData();
                        RandomItemColor();
                        UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                        UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                        SaveData("WeeklyEvent", weeklyEvent);
                        UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        //UIManagerNew.Instance.StartWeeklyEvent.Appear();
                    }
                    else
                    {
                        Debug.LogError("chua het time");
                        FirebaseAnalyticsControl.Instance.LogEventevent_weekly();
                        PlayerPrefs.GetString("FirstWeeklyEvent", "true");
                        if (weeklyEvent.levelIndex == weeklyEventControllers[0].weeklyEventPack.Count - 1 && weeklyEvent.numOfCollection == weeklyEvent.numToLevelUp)
                        {
                            Debug.LogError("pack cuoi ");
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = true;
                            weeklyEvent.eventStaus = EventStaus.end;
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                            UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                            UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                            UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(false);
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(false);
                            SaveData("WeeklyEvent", weeklyEvent);
                            UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        }
                        else
                        {
                            Debug.LogError("chua phai pack cuoi");
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(true);
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                            UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                            UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                            SaveData("WeeklyEvent", weeklyEvent);
                            UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        }
                    }
                }
                else
                 if (weeklyEvent.eventStaus == WeeklyEventController.EventStaus.end)
                {
                    Debug.LogError("dang end");
                    FirebaseAnalyticsControl.Instance.LogEventevent_weekly();
                    DateTime endTime;
                    try
                    {
                        Debug.LogError("weeklyEvent.endEventDate " + weeklyEvent.endEventDate);
                        var x = JsonConvert.DeserializeObject<long>(weeklyEvent.endEventDate);
                        Debug.LogError("x " + x);
                        endTime = new DateTime(x);
                        //endTime.AddDays(1);
                        Debug.LogError("end " + endTime);
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError("weeklyEvent.endEventDate " + weeklyEvent.endEventDate);
                        var x = DateTime.Parse(weeklyEvent.endEventDate);
                        endTime = x;
                        Debug.LogError("x " + x);
                        //endTime.AddDays(1);
                        Debug.LogError("end " + endTime);
                    }
                    if (HasTimeExpired(endTime))
                    {
                        Debug.LogError(" het time dang end");
                        PlayerPrefs.SetString("FirstWeeklyEvent", "false");
                        UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                        weeklyEvent.ResetData();
                        weeklyEvent.eventStaus = EventStaus.running;
                        UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                        UIManagerNew.Instance.WeeklyEventPanel.LoadData();
                        RandomItemColor();
                        UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                        UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                        SaveData("WeeklyEvent", weeklyEvent);
                        UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        //UIManagerNew.Instance.StartWeeklyEvent.Appear();
                    }
                    else
                    {
                        if (weeklyEvent.levelIndex == weeklyEventControllers[0].weeklyEventPack.Count - 1 && weeklyEvent.numOfCollection == weeklyEvent.numToLevelUp)
                        {
                            Debug.LogError("pack cuoi dang end");
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = true;
                            weeklyEvent.eventStaus = EventStaus.end;
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                            UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                            UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                            UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(false);
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(false);
                            SaveData("WeeklyEvent", weeklyEvent);
                            UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        }
                        else
                        {
                            Debug.LogError("chua phai pack cuoi");
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(true);
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                            UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                            UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                            SaveData("WeeklyEvent", weeklyEvent);
                            UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                        }
                    }
                }
                else
                if (weeklyEvent.eventStaus == WeeklyEventController.EventStaus.NotEnable)
                {
                    Debug.LogError("dang NotEnable");
                    PlayerPrefs.GetString("FirstWeeklyEvent", "true");
                    FirebaseAnalyticsControl.Instance.LogEventevent_weekly();
                    SetNewData("WeeklyEvent", weeklyEvent);
                    UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                    RandomItemColor();
                    UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                    UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                    SaveData("WeeklyEvent", weeklyEvent);
                    UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                    //UIManagerNew.Instance.StartWeeklyEvent.Appear();
                }
            }
            else
            {
                Debug.LogError("tao moi");
                PlayerPrefs.GetString("FirstWeeklyEvent", "true");
                FirebaseAnalyticsControl.Instance.LogEventevent_weekly();
                SetNewData("WeeklyEvent", weeklyEvent);
                UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                RandomItemColor();
                UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                SaveData("WeeklyEvent", weeklyEvent);
                UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
            }
        }
        else
        {
            if (weeklyEvent != null)
            {
                weeklyEvent = null;
                SaveData("WeeklyEvent", weeklyEvent);
            }
        }


        //treasure Haunt
        //if (DateTime.Now.Date.Subtract(new DateTime(long.Parse(PlayerPrefs.GetString(DataInit.instance.FirstOpenDate)))).TotalDays >= 2)
        //{
        //    if (weeklyEventHauntedTreasure != null)
        //    {
        //        if (weeklyEventHauntedTreasure.eventStaus == WeeklyEventController.EventStaus.running)
        //        {
        //            CheckForEndTime("HauntedTreasure", weeklyEventHauntedTreasure, weeklyEventControllers[1]);
        //        }
        //        else
        //        if (CheckForDate("HauntedTreasure", weeklyEventHauntedTreasure, weeklyEventControllers[1]))
        //        {
        //            weeklyEventHauntedTreasure.ResetData();
        //            weeklyEventHauntedTreasure.eventStaus = WeeklyEventController.EventStaus.running;
        //            CretaWeeklyEvent(weeklyEventHauntedTreasure);
        //        }
        //    }
        //    else
        //    {
        //        SetNewData("HauntedTreasure", weeklyEventHauntedTreasure);
        //        CretaWeeklyEvent(weeklyEventHauntedTreasure);
        //        SaveData("HauntedTreasure", weeklyEventHauntedTreasure);
        //    }
        //}
    }
    public bool CheckForDate(string eventName, WeeklyEventController weeklyEventController, WeeklyEventController targetWeeklyEventController)
    {
        bool status = false;
        if (string.IsNullOrEmpty(weeklyEventController.startEventDate))
        {
            Debug.LogError("create new data");
            SetNewData(eventName, weeklyEventController);
            status = true;
            SaveData(eventName, weeklyEventController);
        }
        else
        {
            DateTime startTime;
            try
            {
                Debug.LogError("weeklyEvent.startEventDate " + weeklyEvent.startEventDate);
                var x = JsonConvert.DeserializeObject<long>(weeklyEvent.startEventDate);
                Debug.LogError("x " + x);
                startTime = new DateTime(x);
                //endTime.AddDays(1);
                Debug.LogError("start " + startTime);
            }
            catch (Exception ex)
            {
                Debug.LogError("weeklyEvent.startEventDate " + weeklyEvent.startEventDate);
                var x = DateTime.Parse(weeklyEvent.startEventDate);
                startTime = x;
                Debug.LogError("x " + x);
                //endTime.AddDays(1);
                Debug.LogError("start " + startTime);
            }
            if (DateTime.Now.Date.Subtract(startTime).TotalDays >= targetWeeklyEventController.numberOfDaysExistence + targetWeeklyEventController.numberBeforeStart)
            {
                status = true;
            }
            else
            {
                status = false;
            }
        }
        return status;
    }
    [Button("SaveData")]
    public void SaveData(string eventName, WeeklyEventController weeklyEventController)
    {
        PlayerPrefs.SetString(eventName, JsonConvert.SerializeObject(weeklyEventController));
        Debug.Log("data " + PlayerPrefs.GetString(eventName));
    }
    [Button("LoadData")]
    public void LoadData()
    {
        //LoadConfigDataWeeklyEvent();
        //if (CheckTimeForWeeklyEvent())
        //{
        //string dataString = PlayerPrefs.GetString("TreasureClimb");
        //string dataString1 = PlayerPrefs.GetString("HauntedTreasure");
        string dataString2 = PlayerPrefs.GetString("WeeklyEvent");


        //weeklyEventTreasureClimb = JsonConvert.DeserializeObject<WeeklyEventController>(dataString);
        //weeklyEventHauntedTreasure = JsonConvert.DeserializeObject<WeeklyEventController>(dataString1);
        weeklyEvent = JsonConvert.DeserializeObject<WeeklyEventController>(dataString2);

        weeklyEventItemColors = LoadList("weeklyEventItemColors");

        //Debug.Log("weeklyEventTreasureClimb " + weeklyEventTreasureClimb);
        //Debug.Log("weeklyEventHauntedTreasure " + weeklyEventHauntedTreasure);
        Debug.Log("WeeklyEvent " + weeklyEvent);

        //Debug.Log(PlayerPrefs.GetString(dataString));
        //Debug.Log(PlayerPrefs.GetString(dataString1));
        Debug.Log(PlayerPrefs.GetString(dataString2));
    }
    //else
    //{
    //    // do nothing

    [Button("CretaWeeklyEvent")]
    public void CretaWeeklyEvent(WeeklyEventController weeklyEventController)
    {
        //UIManagerNew.Instance.TreasureClimbPanel.TresureClimbButton.gameObject.SetActive(true);
        //currentWeeklyEventPrefab.SetValue(weeklyEventController);
    }
    public void CheckForEndTime(String eventName, WeeklyEventController weeklyEventController, WeeklyEventController targetWeeklyEventController)
    {
        DateTime startTime;
        try
        {
            Debug.LogError("weeklyEvent.startEventDate " + weeklyEvent.startEventDate);
            var x = JsonConvert.DeserializeObject<long>(weeklyEvent.startEventDate);
            Debug.LogError("x " + x);
            startTime = new DateTime(x);
            //endTime.AddDays(1);
            Debug.LogError("start " + startTime);
        }
        catch (Exception ex)
        {
            Debug.LogError("weeklyEvent.startEventDate " + weeklyEvent.startEventDate);
            var x = DateTime.Parse(weeklyEvent.startEventDate);
            startTime = x;
            Debug.LogError("x " + x);
            //endTime.AddDays(1);
            Debug.LogError("start " + startTime);
        }
        if (DateTime.Now.Date.Subtract(startTime).TotalDays >= targetWeeklyEventController.numberOfDaysExistence)
        {
            //if (eventName == "TreasureClimb")
            //{
            //    UIManagerNew.Instance.TreasureClimbPanel.TresureClimbButton.gameObject.SetActive(false);
            //    weeklyEventController.eventStaus = WeeklyEventController.EventStaus.end;
            //    SaveData(weeklyEventController.eventType1.ToString(), weeklyEventController);
            //}
            //if (eventName == "HauntedTreasure")
            //{
            //    UIManagerNew.Instance.TreasureClimbPanel.TresureClimbButton.gameObject.SetActive(false);
            //    weeklyEventController.eventStaus = WeeklyEventController.EventStaus.end;
            //    SaveData(weeklyEventController.eventType1.ToString(), weeklyEventController);
            //}
        }
        else
        {
            CretaWeeklyEvent(weeklyEventController);
        }
    }
    [Button("TestChangeData")]
    public void TestChangeData()
    {
        if (currentWeeklyEventPrefab != null)
        {
            currentWeeklyEventPrefab.ChangeData("TreasureClimb", weeklyEventTreasureClimb);
            SaveData("TreasureClimb", weeklyEventTreasureClimb);
        }
        if (weeklyEvent != null)
        {
            if (weeklyEvent.levelIndex < weeklyEventControllers[0].weeklyEventPack.Count)
            {
                //NextStageWeeklyEvent();
            }
            NextStageWeeklyEvent(1);
            //RandomItemColor();
            //UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
        }
    }
    [Button("TestChangeData1")]
    public void TestChangeData1()
    {
        weeklyEvent.numOfCollection = weeklyEvent.numToLevelUp;
        //NextStageWeeklyEvent();
    }


    public void SetNewData(string EventName, WeeklyEventController targetWeeklyEventController)
    {
        if (EventName == WeeklyEventController.EventType.TreasureClimb.ToString())
        {
            targetWeeklyEventController = new WeeklyEventController();
            targetWeeklyEventController.eventType1 = WeeklyEventController.EventType.TreasureClimb;
            targetWeeklyEventController.SetAllData(targetWeeklyEventController);
            targetWeeklyEventController.eventStaus = WeeklyEventController.EventStaus.running;
            //targetWeeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            weeklyEventTreasureClimb = targetWeeklyEventController;
        }
        if (EventName == WeeklyEventController.EventType.HauntedTreasure.ToString())
        {
            targetWeeklyEventController = new WeeklyEventController();
            targetWeeklyEventController.eventType1 = WeeklyEventController.EventType.HauntedTreasure;
            targetWeeklyEventController.SetAllData(targetWeeklyEventController);
            targetWeeklyEventController.eventStaus = WeeklyEventController.EventStaus.running;
            //targetWeeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            weeklyEventHauntedTreasure = targetWeeklyEventController;
        }
        if (EventName == WeeklyEventController.EventType.WeeklyEvent.ToString())
        {
            targetWeeklyEventController = new WeeklyEventController();
            targetWeeklyEventController.eventType1 = WeeklyEventController.EventType.WeeklyEvent;
            targetWeeklyEventController.SetAllData(targetWeeklyEventController);
            targetWeeklyEventController.eventStaus = WeeklyEventController.EventStaus.running;
            weeklyEvent = targetWeeklyEventController;
        }
    }

    public void ChangeCollectSlider(int addnum)
    {
        if (weeklyEvent.numOfCollection < weeklyEvent.numToLevelUp)
        {
            weeklyEvent.numOfCollection += addnum;
            //UIManagerNew.Instance.WeeklyEventPanel.collectSlider.DOValue(weeklyEvent.numOfCollection, 0.3f);
            //SaveData("WeeklyEvent", weeklyEvent);
        }
    }

    public void NextStageWeeklyEvent(int addnum = 0, bool checkAgain = false)
    {
        if (weeklyEvent.levelIndex < weeklyEventControllers[0].weeklyEventPack.Count - 1)
        {
            if (weeklyEvent.numOfCollection + addnum >= weeklyEvent.numToLevelUp)
            {
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEvent.levelIndex);
                SaveSystem.instance.SaveData();

                weeklyEvent.levelIndex++;
                if (checkAgain)
                {
                    weeklyEvent.numOfCollection = 0;
                }
                weeklyEvent.numOfCollection = weeklyEvent.numOfCollection + addnum - weeklyEvent.numToLevelUp;
                weeklyEvent.numToLevelUp = weeklyEventControllers[0].weeklyEventPack[weeklyEvent.levelIndex].numToUpLevel;
                UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue = weeklyEvent.numToLevelUp;
                UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = weeklyEvent.numOfCollection;


            }
            else
            {
                ChangeCollectSlider(addnum);
                if (weeklyEvent.numOfCollection == weeklyEvent.numToLevelUp)
                {
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEvent.levelIndex);
                    SaveSystem.instance.SaveData();

                    weeklyEvent.levelIndex++;
                    weeklyEvent.numOfCollection = weeklyEvent.numOfCollection + addnum - weeklyEvent.numToLevelUp;
                    weeklyEvent.numToLevelUp = weeklyEventControllers[0].weeklyEventPack[weeklyEvent.levelIndex].numToUpLevel;
                    UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue = weeklyEvent.numToLevelUp;
                    UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = weeklyEvent.numOfCollection;

                }
            }
        }
        else
        {
            if (weeklyEvent.numOfCollection + addnum >= weeklyEvent.numToLevelUp)
            {
                weeklyEvent.numOfCollection = weeklyEvent.numToLevelUp;
                UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue;
                UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = true;
                weeklyEvent.eventStaus = EventStaus.end;
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEventControllers[0].weeklyEventPack.Count - 1);
                SaveSystem.instance.SaveData();

            }
            else
            {
                ChangeCollectSlider(addnum);
                if (weeklyEvent.numOfCollection + addnum >= weeklyEvent.numToLevelUp)
                {
                    weeklyEvent.numOfCollection = weeklyEvent.numToLevelUp;
                    UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue;
                    UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = true;
                    weeklyEvent.eventStaus = EventStaus.end;
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEventControllers[0].weeklyEventPack.Count - 1);
                    SaveSystem.instance.SaveData();

                }
            }
        }
        EventController.instance.SaveData("WeeklyEvent", weeklyEvent);
    }

    public void RandomItemColor()
    {
        if (weeklyEventItemColors.IsNullOrEmpty())
        {
            var x = 2;
            weeklyEventItemColors.Add(x);
            weeklyEvent.colorIndex = x;
            selectedColorIndex = x;
            weeklyEventItemSprite = weeklyEventControllers[0].weeklyEventItemColor[x].weeklyEventBarColor;
            SaveList("weeklyEventItemColors", weeklyEventItemColors);
        }
        else
        {
            var x = UnityEngine.Random.Range(0, weeklyEventControllers[0].weeklyEventItemColor.Count);
            {
                if (weeklyEventItemColors.Count != 7)
                {
                    if (!weeklyEventItemColors.Contains(x))
                    {
                        weeklyEventItemColors.Add(x);
                        weeklyEvent.colorIndex = x;
                        selectedColorIndex = x;
                        weeklyEventItemSprite = weeklyEventControllers[0].weeklyEventItemColor[x].weeklyEventBarColor;
                        SaveList("weeklyEventItemColors", weeklyEventItemColors);
                    }
                    else
                    {
                        RandomItemColor();
                    }
                }
                else
                {
                    weeklyEventItemColors.Clear();
                    SaveList("weeklyEventItemColors", weeklyEventItemColors);
                    RandomItemColor();
                }
            }
        }
    }

    public void SaveList(string key, List<int> list)
    {
        string json = JsonConvert.SerializeObject(list);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public DateTime CauculateEndTime()
    {
        DateTime dateTime;
        try
        {
            var x = JsonConvert.DeserializeObject<long>(weeklyEvent.startEventDate);
            dateTime = new DateTime(x);
            dateTime = dateTime.AddDays(8); // Gán lại giá trị sau khi cộng
            return dateTime;
        }
        catch (Exception ex)
        {
            dateTime = DateTime.Today;
        }
        return dateTime;
    }

    // Tải danh sách
    public List<int> LoadList(string key)
    {
        string json = PlayerPrefs.GetString(key, string.Empty);
        if (!string.IsNullOrEmpty(json))
        {
            return JsonConvert.DeserializeObject<List<int>>(json);
        }
        return new List<int>();
    }

    public bool FirstWeeklyEvent()
    {
        var data = PlayerPrefs.GetString("FirstWeeklyEvent", "false");
        bool HasFinishedStory = JsonConvert.DeserializeObject<bool>(data);
        return HasFinishedStory;
    }

    public bool HasTimeExpired(DateTime sunday)
    {
        DateTime today = DateTime.Now;
        // Kiểm tra nếu thời gian hiện tại đã vượt quá thời gian Chủ nhật lúc 12h đêm

        return DateTime.Now.Subtract(sunday).TotalHours >= 24;
    }

    public void LoadConfigDataWeeklyEvent()
    {
        string datastring3 = PlayerPrefs.GetString("WeeklyEventConfig");
        weeklyEventConfig = JsonConvert.DeserializeObject<WeeklyEventDataConfig>(datastring3);
        Debug.LogError("data new :" + weeklyEventConfig.EndTime);
    }

    public bool CheckTimeForWeeklyEvent()
    {
        bool result = false;
        if (weeklyEventConfig != null)
        {
            if (weeklyEventConfig.StartTime.Subtract(DateTime.Now).TotalDays > 0)
            {
                Debug.Log(" chưa đến weekly event");
                result = false;
                return result;
            }
            else if (weeklyEventConfig.EndTime.Subtract(DateTime.Now).TotalDays < 0)
            {
                Debug.Log(" đã hết time weekly event");
                if (weeklyEventConfig.NextEvent.Subtract(DateTime.Now).TotalDays < 0)
                {
                    Debug.Log(weeklyEventConfig.NextEvent.Subtract(DateTime.Now).TotalDays);
                    Debug.Log(" đã đến next weekly event");
                    needResetData = true;
                    result = true;
                    return result;
                }
                else
                {
                    Debug.Log(" chưa đến next weekly event");
                    result = false;
                    return result;
                }
            }
            else
            {
                Debug.Log(" chưa hết time weekly event");
                result = true;
                return result;
            }
        }
        return result;
    }
}
[System.Serializable]
public class WeeklyEventDataConfig
{
    [JsonProperty("NameEvent")]
    public string NameEvent;

    [JsonProperty("TimeStart")]
    public DateTime StartTime;

    [JsonProperty("TimeEnd")]
    public DateTime EndTime;

    [JsonProperty("NextEvent")]
    public DateTime NextEvent;

    public WeeklyEventDataConfig(string nameEvent, DateTime startTime, DateTime endTime, DateTime nextEvent)
    {
        NameEvent = nameEvent;
        StartTime = startTime;
        EndTime = endTime;
        NextEvent = nextEvent;
    }
}
