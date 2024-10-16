using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;

public class DailyPanel : MonoBehaviour
{
    public RewardDaily[] dayRewards;
    public int lastDate = 0;
    [SerializeField] private int selectReward = 0;
    public Button claim;
    public Button claimX2;
    public bool isClaim;
    public bool isClaimX2;
    public reciveRewardPanel reciveRewardPanel;
    public reciveRewardDaily reciveRewardDaily;
    public CanvasGroup canvasGroup;

    public int startValue;

    private void Awake()
    {
        lastDate = SaveSystem.instance.days;
    }
    private void Start()
    {
        checkDay();


    }
    private bool checkDay()
    {
        bool result = false;
        //get last claim time");
        string lastTime = PlayerPrefs.GetString("LastClaimTime");
        DateTime lastclaimTime;
        if (!string.IsNullOrEmpty(lastTime))
        {
            lastclaimTime = DateTime.Parse(lastTime);
        }
        else
        {
            lastclaimTime = DateTime.MinValue;
        }
        //enable / disable claim button
        if (DateTime.Today > lastclaimTime)
        {
            if (lastDate < dayRewards.Length)
            {
                if (SaveSystem.instance.days < 7)
                {
                    result = true;
                    dayRewards[SaveSystem.instance.days].isActive = true;
                    claimX2.interactable = true;
                }
                else
                {
                    claimX2.interactable = false;
                }
            }
        }
        else
        {
            claimX2.interactable = false;
        }
        return result;
    }
    private void Update()
    {
        checkDay();
    }
    private void OnEnable()
    {
        for (int i = 0; i < SaveSystem.instance.days; i++)
        {
            dayRewards[i].isClaim = true;
        }
        startValue = SaveSystem.instance.coin;
    }
    public void OnClaimButtinPressed()
    {
        PlayerPrefs.SetString("LastClaimTime", DateTime.Now.ToString("yyyy-MM-dd"));
        isClaim = true;
        reciveRewardPanel.Open();
        Claim();
        SaveSystem.instance.SaveData();

    }
    public void OnClaimButtinPressedX2()
    {
        AdsManager.instance.ShowRewardVideo(() =>
        {
            PlayerPrefs.SetString("LastClaimTime", DateTime.Now.ToString("yyyy-MM-dd"));
            Debug.Log(DateTime.Now.ToString());
            isClaimX2 = true;
            //if (lastDate == 6)
            //{
            reciveRewardPanel.Open();
            Claim();
            //}
            //else
            //{
            //  reciveRewardDaily.gameObject.SetActive(true);
            //	reciveRewardDaily.claim();
            //         }
            //SaveSystem.instance.SaveData();
        });
    }
    public void Claim()
    {
        if (isClaim)
        {
            reciveRewardPanel.TakeReward(() =>
            {
                StartCoroutine(ClosePanel());
            });
        }
        if (isClaimX2)
        {
            reciveRewardPanel.TakeReward(() =>
            {
                StartCoroutine(ClosePanel());
            });
        }
        SaveSystem.instance.SaveData();
    }
    IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(0.5f);
        UIManagerNew.Instance.DailyRWUI.Close();
        reciveRewardPanel.Close();
        reciveRewardDaily.gameObject.SetActive(true);
        //reciveRewardDaily.claim();
        reciveRewardDaily.SetValue();
        reciveRewardDaily.SpawnObjects(dayRewards[lastDate].gold, dayRewards[lastDate].magicTiket, dayRewards[lastDate].powerTicket, reciveRewardDaily.rewardImg.gameObject);
        isClaimX2 = false;
        AudioManager.instance.PlaySFX("ClosePopUp");

        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        {
            UIManagerNew.Instance.ButtonMennuManager.Appear();
            DOVirtual.DelayedCall(1.5f, () =>
            {
                if (LevelManagerNew.Instance.stage >= 8)
                {
                    if (!EventController.instance.FirstWeeklyEvent())
                    {
                        UIManagerNew.Instance.StartWeeklyEvent.Appear();
                        PlayerPrefs.SetString("FirstWeeklyEvent", "true");
                    }
                }
            });
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            if (reciveRewardPanel.gameObject.activeSelf)
            {
                reciveRewardPanel.gameObject.SetActive(false);
            }
            //this.gameObject.SetActive(false);
        }

    }
    public void CheckForClose()
    {
        if (lastDate < dayRewards.Length)
        {
            if (dayRewards[lastDate] != null)
            {
                if (!dayRewards[lastDate].isClaim && checkDay())
                {
                    OnClaimButtinPressed();
                }
                else
                {
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                    UIManagerNew.Instance.ButtonMennuManager.DisappearDailyRW();
                    DOVirtual.DelayedCall(1, () =>
                    {
                        if (LevelManagerNew.Instance.stage >= 8)
                        {
                            if (!EventController.instance.FirstWeeklyEvent())
                            {
                                UIManagerNew.Instance.StartWeeklyEvent.Appear();
                                PlayerPrefs.SetString("FirstWeeklyEvent", "true");
                            }
                        }
                    });
                }
            }
        }
        else
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
            UIManagerNew.Instance.ButtonMennuManager.DisappearDailyRW();
            DOVirtual.DelayedCall(1, () =>
            {
                if (LevelManagerNew.Instance.stage >= 8)
                {
                    if (!EventController.instance.FirstWeeklyEvent())
                    {
                        UIManagerNew.Instance.StartWeeklyEvent.Appear();
                        PlayerPrefs.SetString("FirstWeeklyEvent", "true");
                    }
                }
            });
        }
    }
}
