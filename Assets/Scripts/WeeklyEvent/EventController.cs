using Facebook.Unity;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public static EventController instance;
    public WeeklyEventController[] weeklyEventControllers;

    public WeeklyEventController weeklyEventTreasureClimb;
    public WeeklyEventController weeklyEventHauntedTreasure;
    public WeeklyEventController weeklyEvent;

    public WeeklyEventPrefab[] WeeklyEventPrefabs;
    public WeeklyEventPrefab currentWeeklyEventPrefab;
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
            if (weeklyEvent != null)
            {
                if (weeklyEventTreasureClimb.eventStaus == WeeklyEventController.EventStaus.running)
                {
                    CheckForEndTime("WeeklyEvent", weeklyEvent, weeklyEventControllers[2]);
                }
                else
                if (CheckForDate("WeeklyEvent", weeklyEvent, weeklyEventControllers[2]))
                {
                    weeklyEvent.ResetData();
                    weeklyEvent.eventStaus = WeeklyEventController.EventStaus.running;
                    CretaWeeklyEvent(weeklyEvent);
                }
            }
            else
            {
                SetNewData("WeeklyEvent", weeklyEvent);
                CretaWeeklyEvent(weeklyEvent);
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
        weeklyEventHauntedTreasure = JsonConvert.DeserializeObject<WeeklyEventController>("HauntedTreasure");
        weeklyEvent = JsonConvert.DeserializeObject<WeeklyEventController>("WeeklyEvent");

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
        UIManagerNew.Instance.TreasureClimbPanel.TresureClimbButton.gameObject.SetActive(true);
        currentWeeklyEventPrefab.SetValue(weeklyEventController);
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
            weeklyEventHauntedTreasure = targetWeeklyEventController;
        }
    }
}
