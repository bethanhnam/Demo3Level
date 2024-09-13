using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyEventPrefab : MonoBehaviour
{
    public TextMeshProUGUI title;

    public TextMeshProUGUI numOfDayLeft;

    private void Update()
    {

    }
    public void SetValue(WeeklyEventController weeklyEventController)
    {
        title.text = weeklyEventController.eventType1.ToString();
        numOfDayLeft.text = weeklyEventController.numberOfDaysExistence.ToString();
    }
    public void ChangeData(string eventName, WeeklyEventController weeklyEventController)
    {
        weeklyEventController.numberOfDaysExistence -= 1;
        EventController.instance.SaveData(eventName, weeklyEventController);
        numOfDayLeft.text = weeklyEventController.numberOfDaysExistence.ToString();
    }
}
