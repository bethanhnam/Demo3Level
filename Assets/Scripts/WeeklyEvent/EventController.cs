using DG.Tweening;
using Facebook.Unity;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
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

    public List<int> weeklyEventItemColors = new List<int>();
    public int selectedColorIndex = 0;

    private void Start()
    {
        instance = this;
        LoadData();
        CheckForWeeklyEvent();
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
            UIManagerNew.Instance.WeeklyEventPanel.WeeklyEventButton.gameObject.SetActive(true);
            if (weeklyEvent != null)
            {
                weeklyEventItemSprite = weeklyEventControllers[0].weeklyEventItemColor[weeklyEvent.colorIndex].weeklyEventBarColor;
                if (weeklyEvent.eventStaus == WeeklyEventController.EventStaus.running)
                {
                    DateTime startDate = DateTime.Parse(weeklyEvent.startEventDate);
                    if (DateTime.Now.Date.Subtract(startDate).TotalDays >= 7)
                    {
                        weeklyEvent.eventStaus = EventStaus.running;
                        PlayerPrefs.SetString("FirstWeeklyEvent", "false");
                        UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                        weeklyEvent.ResetData();
                        UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                        UIManagerNew.Instance.WeeklyEventPanel.LoadData();
                        RandomItemColor();
                        UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                        UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                    }
                    else
                    {
                        PlayerPrefs.GetString("FirstWeeklyEvent", "true");
                        if (weeklyEvent.levelIndex == weeklyEventControllers[0].weeklyEventPack.Count && weeklyEvent.numOfCollection == weeklyEvent.numToLevelUp)
                        {
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = true;
                            weeklyEvent.eventStaus = EventStaus.end;
                            UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(false);
                        }
                        else
                        {
                            UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                            UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                            UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                            UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                            UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                        }
                    }
                }
                else
                {
                    DateTime startDate = DateTime.Parse(weeklyEvent.startEventDate);
                    if (DateTime.Now.Date.Subtract(startDate).TotalDays >= 7)
                    {
                        weeklyEvent.eventStaus = EventStaus.running;
                        PlayerPrefs.SetString("FirstWeeklyEvent", "false");
                        UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                        weeklyEvent.ResetData();
                        UIManagerNew.Instance.WeeklyEventPanel.rewardImage.gameObject.SetActive(true);
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                        UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                        UIManagerNew.Instance.WeeklyEventPanel.LoadData();
                        RandomItemColor();
                        UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                        UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                    }
                }
            }
            else
            {
                PlayerPrefs.GetString("FirstWeeklyEvent", "true");

                SetNewData("WeeklyEvent", weeklyEvent);
                UIManagerNew.Instance.WeeklyEventPanel.hasCompletedEvent = false;
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.CreateRewardList();
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.AddData();
                RandomItemColor();
                UIManagerNew.Instance.WeeklyEventPanel.changeCollectItem(weeklyEventItemSprite);
                UIManagerNew.Instance.StartWeeklyEvent.SetCollectImg(weeklyEventItemSprite);
                SaveData("WeeklyEvent", weeklyEvent);
            }
        }
        else
        {
            UIManagerNew.Instance.WeeklyEventPanel.WeeklyEventButton.gameObject.SetActive(false);
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
            if (DateTime.Now.Date.Subtract(new DateTime(long.Parse(weeklyEventController.startEventDate))).TotalDays >= targetWeeklyEventController.numberOfDaysExistence + targetWeeklyEventController.numberBeforeStart)
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
        string dataString = PlayerPrefs.GetString("TreasureClimb");
        string dataString1 = PlayerPrefs.GetString("HauntedTreasure");
        string dataString2 = PlayerPrefs.GetString("WeeklyEvent");

        weeklyEventTreasureClimb = JsonConvert.DeserializeObject<WeeklyEventController>(dataString);
        weeklyEventHauntedTreasure = JsonConvert.DeserializeObject<WeeklyEventController>(dataString1);
        weeklyEvent = JsonConvert.DeserializeObject<WeeklyEventController>(dataString2);

        weeklyEventItemColors = LoadList("weeklyEventItemColors");

        Debug.Log("weeklyEventTreasureClimb " + weeklyEventTreasureClimb);
        Debug.Log("weeklyEventHauntedTreasure " + weeklyEventHauntedTreasure);
        Debug.Log("WeeklyEvent " + weeklyEvent);

        Debug.Log(PlayerPrefs.GetString(dataString));
        Debug.Log(PlayerPrefs.GetString(dataString1));
        Debug.Log(PlayerPrefs.GetString(dataString2));

    }
    [Button("CretaWeeklyEvent")]
    public void CretaWeeklyEvent(WeeklyEventController weeklyEventController)
    {
        //UIManagerNew.Instance.TreasureClimbPanel.TresureClimbButton.gameObject.SetActive(true);
        //currentWeeklyEventPrefab.SetValue(weeklyEventController);
    }
    public void CheckForEndTime(String eventName, WeeklyEventController weeklyEventController, WeeklyEventController targetWeeklyEventController)
    {
        if (DateTime.Now.Date.Subtract(new DateTime(long.Parse(weeklyEventController.startEventDate))).TotalDays >= targetWeeklyEventController.numberOfDaysExistence)
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
            targetWeeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            weeklyEventTreasureClimb = targetWeeklyEventController;
        }
        if (EventName == WeeklyEventController.EventType.HauntedTreasure.ToString())
        {
            targetWeeklyEventController = new WeeklyEventController();
            targetWeeklyEventController.eventType1 = WeeklyEventController.EventType.HauntedTreasure;
            targetWeeklyEventController.SetAllData(targetWeeklyEventController);
            targetWeeklyEventController.eventStaus = WeeklyEventController.EventStaus.running;
            targetWeeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
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

    public void NextStageWeeklyEvent(int addnum = 0)
    {
        if (weeklyEvent.levelIndex < weeklyEventControllers[0].weeklyEventPack.Count - 1)
        {
            if (weeklyEvent.numOfCollection + addnum >= weeklyEvent.numToLevelUp)
            {
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEvent.levelIndex);
                SaveSystem.instance.SaveData();

                weeklyEvent.levelIndex++;
                weeklyEvent.numOfCollection = weeklyEvent.numOfCollection + addnum - weeklyEvent.numToLevelUp;
                weeklyEvent.numToLevelUp = weeklyEventControllers[0].weeklyEventPack[weeklyEvent.levelIndex].numToUpLevel;
                UIManagerNew.Instance.WeeklyEventPanel.collectSlider.maxValue = weeklyEvent.numToLevelUp;
                UIManagerNew.Instance.WeeklyEventPanel.collectSlider.value = weeklyEvent.numOfCollection;

                SaveData("WeeklyEvent", weeklyEvent);
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

                    SaveData("WeeklyEvent", weeklyEvent);
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
                UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEventControllers[0].weeklyEventPack.Count);
                SaveSystem.instance.SaveData();

                SaveData("WeeklyEvent", weeklyEvent);
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
                    UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.RewardClaim(weeklyEventControllers[0].weeklyEventPack.Count);
                    SaveSystem.instance.SaveData();

                    SaveData("WeeklyEvent", weeklyEvent);
                }
            }
        }
    }

    public void RandomItemColor()
    {
        var x = UnityEngine.Random.Range(0, weeklyEventControllers[0].weeklyEventItemColor.Count);
        {
            if (weeklyEventItemColors.Count != 7)
            {
                if (!weeklyEventItemColors.Contains(x))
                {
                    SaveList("weeklyEventItemColors", weeklyEventItemColors);
                    weeklyEventItemColors.Add(x);
                    weeklyEvent.colorIndex = x;
                    selectedColorIndex = x;
                    weeklyEventItemSprite = weeklyEventControllers[0].weeklyEventItemColor[x].weeklyEventBarColor;
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

    public void SaveList(string key, List<int> list)
    {
        string json = JsonConvert.SerializeObject(list);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
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
}
