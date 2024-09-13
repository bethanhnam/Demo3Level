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
    //public Button WeeklyEventButton;

    //public RaceTrack raceTrack;
    //// Start is called before the first frame update
    //public void Appear()
    //{
    //    GameManagerNew.Instance.Bg.sprite = GameManagerNew.Instance.sprites[0];
    //    GameManagerNew.Instance.Bg.gameObject.SetActive(true);
    //    if (!this.gameObject.activeSelf)
    //    {
    //        this.gameObject.SetActive(true);
    //    }
    //    DOVirtual.DelayedCall(1, () =>
    //    {
    //        raceTrack.LoadPos();
    //    });
    //}
    //public void Disappear()
    //{
    //    this.gameObject.SetActive(false);
    //    GameManagerNew.Instance.Bg.gameObject.SetActive(false);
    //}

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
        DateTime today = DateTime.Now;
        int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;

        // Ngày Chủ Nhật lúc 12h đêm (23:59:59)
        DateTime sundayMidnight = today.Date.AddDays(daysUntilSunday).AddDays(1).AddSeconds(-1);

        TimeSpan timeUntilSundayMidnight = sundayMidnight - today;

        int days = timeUntilSundayMidnight.Days;
        int hours = timeUntilSundayMidnight.Hours;
        int minutes = timeUntilSundayMidnight.Minutes;

        return $"{days}d {hours}h";
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
    }
}
