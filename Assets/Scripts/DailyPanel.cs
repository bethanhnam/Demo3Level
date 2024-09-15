using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using Unity.VisualScripting;
using Sirenix.OdinInspector;

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

    }

    private bool checkDay()
    {
        bool result = false;
        // Lấy thời gian lần cuối claim
        string lastTime = PlayerPrefs.GetString("LastClaimTime");
        DateTime lastclaimTime;
        if (!string.IsNullOrEmpty(lastTime))
        {
            lastclaimTime = DateTime.Parse(lastTime); // Đảm bảo thời gian UTC
        }
        else
        {
            lastclaimTime = DateTime.MinValue;
        }

        // Kiểm tra với thời gian UTC hiện tại
        if (DateTime.Today > lastclaimTime)
        {
            UIManagerNew.Instance.ButtonMennuManager.ShowNoticeIcon(0, true);
            if (lastDate < dayRewards.Length)
            {
                result = true;
                dayRewards[lastDate].isActive = true;
                claimX2.interactable = true;
            }
            else
            {
                claimX2.interactable = false;
                lastDate = 0;
                SetupNewWeek();
            }
        }
        else
        {
            claimX2.interactable = false;
            UIManagerNew.Instance.ButtonMennuManager.ShowNoticeIcon(0, false);
        }
        return result;
    }

    private void OnEnable()
    {
        if (lastDate < 7)
        {
            for (int i = 0; i < lastDate; i++)
            {
                dayRewards[i].isClaim = true;
            }
        }
        else
        {
            for (int i = 0; i < 7; i++)
            {
                dayRewards[i].isClaim = true;
            }
        }
        startValue = SaveSystem.instance.coin;
        checkDay();
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
            reciveRewardPanel.Open();
            Claim();
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

    [Button("ResetDataReward")]
    public void ResetDataReward()
    {
        for (int i = 0; i < dayRewards.Length; i++)
        {
            for (int j = 0; j < dayRewards[i].rewardImg.Length; j++)
            {
                dayRewards[i].isClaim = false;
                dayRewards[i].rewardImg[j].sprite = dayRewards[i].rewardSprite[j];
                dayRewards[i].rewardImg[j].GetComponent<Image>().SetNativeSize();
                dayRewards[i].rewardImg[j].enabled = true;
            }
            for (int k = 0; k < dayRewards[i].tickImg.Length; k++)
            {
                dayRewards[i].tickImg[k].transform.gameObject.SetActive(true);
            }
            dayRewards[i].dayText.color = Color.white;
            dayRewards[i].panelImg.sprite = dayRewards[i].panelDefaultSprite;
            dayRewards[i].button.enabled = true;
            dayRewards[i].button.interactable = true;
        }
        SaveSystem.instance.days = 0;
        SaveSystem.instance.SaveData();
    }

    [Button("SetupNewWeek")]
    public void SetupNewWeek()
    {
        if (SaveSystem.instance.days >= 7)
        {
            ResetDataReward();
            checkDay();
        }
    }
}
