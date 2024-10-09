using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyEventPanel : MonoBehaviour
{
    public Button WeeklyEventButton;

    public weeklyRewardController weeklyRewardController;

    public Image ItemToCollect;

    public TextMeshProUGUI collectText;

    public Image rewardImage;

    public Image barCollectImage;

    public TextMeshProUGUI rewardText;

    public TextMeshProUGUI NumOfCollect;

    public TextMeshProUGUI timeRemaining;

    public Slider collectSlider;

    public Sprite[] rewardSprites;

    public bool hasCompletedEvent = false;

    public Animator animator;

    public WeeklyItemCollect WeeklyItemCollect;


    // halloween 
    public Image panel;
    public List<Sprite> panelSprites;

    private void Update()
    {
        timeRemaining.text = UIManagerNew.Instance.WeeklyEventPanel.CauculateTimeRemaining();
    }

    // Start is called before the first frame update
    public void Appear()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        this.gameObject.SetActive(true);
        EventController.instance.NextStageWeeklyEvent();
        LoadData();
        animator.enabled = true;
        animator.Play("Apear");
    }

    public void Active()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        animator.enabled = false;
    }


    public void Disappear()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        animator.enabled = true;
        animator.Play("Disapear");
    }

    public void Deactive()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
        this.gameObject.SetActive(false);
        animator.enabled = false;
    }

    public void LoadData()
    {
        if (EventController.instance != null) {
            if(EventController.instance.weeklyEvent != null)
            {
                if (hasCompletedEvent)
                {
                    collectSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                    collectSlider.value = EventController.instance.weeklyEvent.numOfCollection;
                    NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                    rewardImage.sprite = weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardImg.sprite;
                    rewardText.text = weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    collectText.text = "Collect " + EventController.instance.weeklyEvent.numToLevelUp.ToString() + "              " + "to win!";
                    timeRemaining.text = CauculateTimeRemaining();
                    ChangeRewardImage();
                    weeklyRewardController.CompleteEvent();
                }
                else
                {
                    collectSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                    collectSlider.value = EventController.instance.weeklyEvent.numOfCollection;
                    NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                    rewardImage.sprite = weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardImg.sprite;
                    rewardText.text = weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].numOfReward.text;
                    collectText.text = "Collect " + EventController.instance.weeklyEvent.numToLevelUp.ToString() + "              " + "to win!";
                    timeRemaining.text = CauculateTimeRemaining();
                    ChangeRewardImage();
                    weeklyRewardController.UpdateData();
                }
            }
        }
    }


    public string CauculateTimeRemaining()
    {
        string timeText;

        DateTime today = DateTime.Now;
        int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;

        // Ngày Chủ Nhật lúc 12h đêm (23:59:59)
        DateTime sundayMidnight = today.Date.AddDays(daysUntilSunday).AddDays(1).AddSeconds(-1);

        TimeSpan timeUntilSundayMidnight = sundayMidnight - today;

        //DateTime timeUntilNextEventAsDateTime = DateTime.MinValue.Add(timeUntilSundayMidnight);

        int days = (int)timeUntilSundayMidnight.Days;
        int hours = timeUntilSundayMidnight.Hours;
        int minutes = timeUntilSundayMidnight.Minutes;
        int seconds = timeUntilSundayMidnight.Seconds;

        if (days < 0 && hours < 0 && minutes < 0 && seconds < 0)
        {
            days = 0;
            hours = 0;
            minutes = 0;
            seconds = 0;
        }
        if (days > 0)
        {
            timeText = $"{(int)days:D2}d{(int)hours:D2}h";
        }
        else
        {
            if (hours > 0)
            {
                timeText = $"{(int)hours:D2}h{(int)minutes:D2}m";
            }
            else
            {
                timeText = $"{(int)minutes:D2}m{(int)seconds:D2}";
                if (seconds < 0)
                {
                    return null;
                }
            }
        }

        return timeText;
    }

    public void ChangeRewardImage()
    {
        if (weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.gold)
        {
            rewardImage.sprite = rewardSprites[0];
        }
        if (weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.unscrew)
        {
            rewardImage.sprite = rewardSprites[1];
        }
        if (weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.undo)
        {
            rewardImage.sprite = rewardSprites[2];
        }
        if (weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.drill)
        {
            rewardImage.sprite = rewardSprites[3];
        }
    }

    public void changeCollectItem(Sprite sprite)
    {
        barCollectImage.sprite = sprite;
        ItemToCollect.sprite = sprite;
        //UIManagerNew.Instance.ButtonMennuManager.collectImage.sprite = sprite;
    }
}
