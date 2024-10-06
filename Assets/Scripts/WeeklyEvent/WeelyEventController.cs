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
    public int numOfChances;
    [ShowIf("IsTreasureClimb")]
    public int numOfStage;
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
    public List<WeeklyEventPack> weeklyEventPack;
    [ShowIf("IsWeeklyEvent")]
    public int levelIndex;
    [ShowIf("IsWeeklyEvent")]
    public int numToLevelUp;
    [ShowIf("IsWeeklyEvent")]
    public int numOfCollection;
    [ShowIf("IsWeeklyEvent")]
    public Sprite[] weeklyEventPacksprite;
    [ShowIf("IsWeeklyEvent")]
    public List<WeeklyEventItemColor> weeklyEventItemColor;
    [ShowIf("IsWeeklyEvent")]
    public int colorIndex;


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
            numOfStage = 10;
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
            numOfCollection = 0;
            levelIndex = 0;
            numToLevelUp = 2;
            numberOfDaysExistence = 7;
            eventStaus = EventStaus.NotEnable;
            startEventDate = GetMondayDate().Ticks.ToString();
            endEventDate = GetSundayDate().Ticks.ToString();
            colorIndex = 0;
        }
    }

    public void SetAllData(WeeklyEventController weeklyEventController)
    {
        ResetData();
        if (weeklyEventController.eventType1 == EventType.TreasureClimb)
        {
            weeklyEventController.numOfChances = numOfChances;
            weeklyEventController.numOfStage = numOfStage;
            weeklyEventController.numOfWinStage = numOfWinStage;
            weeklyEventController.topIndex = topIndex;
            weeklyEventController.numberOfDaysExistence = numberOfDaysExistence;
            weeklyEventController.numberBeforeStart = numberBeforeStart;
            weeklyEventController.eventStaus = eventStaus;

            if (startEventDate != null)
            {
 //               weeklyEventController.startEventDate = startEventDate;
            }
            else
            {
   //             weeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            }
    //        weeklyEventController.endEventDate = weeklyEventController.startEventDate + weeklyEventController.numberOfDaysExistence;
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
   //             weeklyEventController.startEventDate = startEventDate;
            }
            else
            {
      //          weeklyEventController.startEventDate = System.DateTime.Now.Ticks.ToString();
            }
      //      weeklyEventController.endEventDate = weeklyEventController.startEventDate + weeklyEventController.numberOfDaysExistence;
        }
        if (weeklyEventController.eventType1 == EventType.WeeklyEvent)
        {
            weeklyEventController.colorIndex = colorIndex;
            weeklyEventController.levelIndex = levelIndex;
            weeklyEventController.numToLevelUp = numToLevelUp;
            weeklyEventController.numOfCollection = numOfCollection;

            weeklyEventController.startEventDate = GetMondayDate().Date.Ticks.ToString();
            weeklyEventController.endEventDate = GetSundayDate().Date.Ticks.ToString();
        }
    }

    public DateTime GetMondayDate()
    {
        DateTime currentDate = DateTime.Now;
        int daysToSubtract = (int)currentDate.DayOfWeek + 1; // +1 để lùi về chủ nhật tuần trước
        DateTime lastSunday = currentDate.AddDays(-daysToSubtract);

        // Trả về giá trị 12h đêm (00:00:00) của chủ nhật tuần trước
        return new DateTime(lastSunday.Year, lastSunday.Month, lastSunday.Day, 0, 0, 0, DateTimeKind.Utc);
    }

    public DateTime GetSundayDate()
    {
        DateTime currentDate = DateTime.Now;
        int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)currentDate.DayOfWeek + 7) % 7;
        DateTime sundayDate = currentDate.Date.AddDays(daysUntilSunday).AddDays(1).AddSeconds(-1); ;
        return new DateTime(sundayDate.Year, sundayDate.Month, sundayDate.Day, 0, 0, 0, DateTimeKind.Utc);
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

    [System.Serializable]
    public class WeeklyEventPack
    {
        public int numOfGold;
        public int numOfUnscrew;
        public int numOfUndo;
        public int numOfDrill;

        public int numToUpLevel;
    }

    [System.Serializable]
    public class WeeklyEventItemColor
    {
        public Sprite weeklyEventBarColor;
        public bool hasSelected;
    }
}
