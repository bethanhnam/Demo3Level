using System;
using System.Collections;
using System.Collections.Generic;
using com.adjust.sdk;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "WeelyEventData", menuName = "ScriptableObjects/WeelyEvent", order = 1)]
public class WeeklyEventController : ScriptableObject
{

    public enum EventType
    {
        TreasureClimb,
        HauntedTreasure,
        WeeklyEvent
    }

    public enum EventStaus
    {
        NotEnable,
        running,
        end
    }
    public EventType eventType1;

    public EventStaus eventStaus;

    [ShowIf("IsTreasureClimb")]
    public TreasureClimbPack[] treasureClimbPacks;
    [ShowIf("IsTreasureClimb")]
    public int numOfChances = 3;
    [ShowIf("IsTreasureClimb")]
    public int numOfStage = 10;
    [ShowIf("IsTreasureClimb")]
    public int numOfWinStage;
    [ShowIf("IsTreasureClimb")]
    public int timeToShowPass;
    [ShowIf("IsTreasureClimb")]
    public int topIndex;

    [ShowIf("IsHauntedTreasure")]
    public HauntedTreasurePack[] hauntedTreasurePack;
    [ShowIf("IsHauntedTreasure")]
    public int packIndex;


    [ShowIf("IsWeeklyEvent")]
    public TreasureClimbPack[] weeklyEventPack;
    [ShowIf("IsWeeklyEvent")]
    public int levelIndex;
    [ShowIf("IsWeeklyEvent")]
    public int Itemuplv;



    // Phương thức kiểm tra điều kiện hiển thị
    private bool IsTreasureClimb()
    {
        return eventType1 == EventType.TreasureClimb;
    }
    private bool IsHauntedTreasure()
    {
        return eventType1 == EventType.HauntedTreasure;
    }
    private bool IsWeeklyEvent()
    {
        return eventType1 == EventType.WeeklyEvent;
    }

    //properties chung 
    public int numberOfDaysExistence;
    public int numberBeforeStart;
    public string startEventDate;
    public string endEventDate;

    [Button("resetData")]
    public void ResetData()
    {
        if (eventType1 == EventType.TreasureClimb)
        {
            numOfChances = 3;
            numOfWinStage = 0;
            topIndex = 0;
            numberOfDaysExistence = 5;
            numberBeforeStart = 3;
            eventStaus = EventStaus.NotEnable;
            startEventDate = null;
            endEventDate = null;
        }
        if (eventType1 == EventType.HauntedTreasure)
        {
            packIndex = 0;
            numberOfDaysExistence = 5;
            numberBeforeStart = 2;
            eventStaus = EventStaus.NotEnable;
            startEventDate = null;
            endEventDate = null;
        }
        if (eventType1 == EventType.WeeklyEvent)
        {
            levelIndex = 0;
            Itemuplv = 2;
            numberOfDaysExistence = 7;
            eventStaus = EventStaus.NotEnable;
            startEventDate = null;
            endEventDate = null;
        }
    }

    public void SetAllData(WeeklyEventController weeklyEventController)
    {
        ResetData();
        if (weeklyEventController.eventType1 == EventType.TreasureClimb)
        {
            weeklyEventController.numOfChances = numOfChances;
            weeklyEventController.numOfWinStage = numOfWinStage;
            weeklyEventController.topIndex = topIndex;
            weeklyEventController.numberOfDaysExistence = numberOfDaysExistence;
            weeklyEventController.numberBeforeStart = numberBeforeStart;
            weeklyEventController.eventStaus = eventStaus;

            if (startEventDate != null)
            {
                weeklyEventController.startEventDate = startEventDate;
            }
            else
            {
                weeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            }
            weeklyEventController.endEventDate = weeklyEventController.startEventDate + weeklyEventController.numberOfDaysExistence;
            //weeklyEventController.weeklyEventPrefab = weeklyEventPrefab;
        }

        if (weeklyEventController.eventType1 == EventType.HauntedTreasure)
        {
            weeklyEventController.packIndex = packIndex;
            weeklyEventController.numberOfDaysExistence = numberOfDaysExistence;
            weeklyEventController.numberBeforeStart = numberBeforeStart;
            weeklyEventController.eventStaus = eventStaus;

            if (startEventDate != null)
            {
                weeklyEventController.startEventDate = startEventDate;
            }
            else
            {
                weeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            }
            weeklyEventController.endEventDate = weeklyEventController.startEventDate + weeklyEventController.numberOfDaysExistence;
        }
        if (weeklyEventController.eventType1 == EventType.WeeklyEvent)
        {
            weeklyEventController.levelIndex = levelIndex;
            weeklyEventController.Itemuplv = Itemuplv;

            weeklyEventController.startEventDate = GetMondayDate().ToString();

            weeklyEventController.endEventDate = weeklyEventController.startEventDate + weeklyEventController.numberOfDaysExistence;
        }
    }

    public DateTime GetMondayDate()
    {
        // Lấy ngày hiện tại
        DateTime currentDate = DateTime.Now;

        // Tính khoảng cách từ thứ Hai (Monday) gần nhất
        int daysUntilMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;

        // Nếu hôm nay là thứ Hai, trả về ngày hôm nay, nếu không thì tìm ngày thứ Hai gần nhất
        DateTime mondayDate = currentDate.AddDays(-((int)currentDate.DayOfWeek - (int)DayOfWeek.Monday));

        return mondayDate;
    }


    [System.Serializable]
    public class HauntedTreasurePack
    {
        public int numOfGold;
        public int numOfUnscrew;
        public int numOfUndo;
        public int numOfDrill;
        public double Price;
    }
    [System.Serializable]
    public class TreasureClimbPack
    {
        public int numOfGold;
        public int numOfUnscrew;
        public int numOfUndo;
        public int numOfDrill;
    }


}
