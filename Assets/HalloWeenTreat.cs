using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HalloWeenTreat : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup canvasGroup;
    public Animator[] items;

    public Image presentImage;
    public Sprite[] itemSprites;
    public Sprite[] buttonSprites;
    public TMP_FontAsset[] fontAsset;

    private int appear = Animator.StringToHash("Appear");
    private int disappear = Animator.StringToHash("Disappear");

    public bool timeOn;
    public float timeDefault = 300;
    public float timeRemaining = 0;


    public Button claimButton;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI buttonText;

    public DateTime claimedDate;

    public bool hasStart = false;
    public bool hasClaimed = false;

    private void Start()
    {

    }

    public void StartCountTime()
    {
        timeRemaining = timeDefault;
        timeOn = true;
        claimButton.transition = Selectable.Transition.None;
    }
    public void DeactiveObjects()
    {
        timeRemaining = 0;
        timeOn = false;
    }
    public void Appear()
    {
        if (this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }
        if (CheckForHalloWeenTreat())
        {
            if (hasStart == false)
            {
                SaveSystem.instance.SavePlayerPrefsInt("HasClaimtTreat", 0);
                StartCountTime();
                timeText.text = SetText(timeRemaining);
                buttonText.font = fontAsset[1];
                claimButton.GetComponent<Image>().sprite = buttonSprites[1];
            }
            else
            {

            }
        }
        else
        {
            DeactiveObjects();
            if (PlayerPrefs.GetInt("HasClaimtTreat") == 0)
            {
                UIManagerNew.Instance.HalloWeenTreat.timeText.text = $"The gift will be delivered in <size=55><color=green>{UIManagerNew.Instance.HalloWeenTreat.SetText(UIManagerNew.Instance.HalloWeenTreat.timeRemaining):D2}</color></size>";
                claimButton.interactable = true;
                buttonText.text = "Claim";
            }
            else
            {
                hasClaimed = true;
                DisplayForClaimed();
                buttonText.text = "Next Day";
            }

        }
        animator.Play(appear);
        DOVirtual.DelayedCall(1, () =>
        {
            if (hasClaimed == false)
            {
                for (int i = 0; i <= items.Length; i++)
                {
                    PlayItemIdleAnim(1.1f*i+1,i, "HalloWeenPackItem", null);
                }
                presentImage.GetComponent<Animator>().enabled = true;
            }
            else
            {
                claimButton.interactable = false;
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].enabled = false;
                }
                for (int i = 0; i <= items.Length; i++)
                {
                    PlayItemIdleAnim(1.1f * i + 1, i, "HalloWeenPackItemClaimed",null);
                }
                presentImage.GetComponent<Animator>().enabled = true;
            }
        });
    }

    public void Disappear()
    {
        DeactiveCanvasGroup();
        animator.Play(disappear);
        UIManagerNew.Instance.ButtonMennuManager.Appear();
    }

    private void Update()
    {

    }
    public bool CheckForHalloWeenTreat()
    {

        bool result = false;
        // Lấy thời gian lần cuối claim
        string lastTime = PlayerPrefs.GetString("LastClaimDay");
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
            result = true;
        }
        else
        {
            result = false;
        }
        return result;
    }

    public void DisplayForClaimed()
    {
        presentImage.GetComponent<Animator>().enabled = true;
        claimButton.GetComponent<Image>().sprite = buttonSprites[1];
        UIManagerNew.Instance.HalloWeenTreat.timeText.text = $"The gift will be delivered in <size=55><color=green>Next Day</color></size>";
        timeText.font = fontAsset[1];
        buttonText.font = fontAsset[1];
        claimButton.interactable = false;
        for (int i = 0; i < items.Length; i++)
        {
            items[i].GetComponent<Image>().sprite = itemSprites[1];
            items[i].transform.GetChild(1).gameObject.SetActive(false);
            items[i].transform.GetChild(2).gameObject.SetActive(true);
        }
    }

    public string SetText(float time)
    {
        string timeText = "";

        // If time is 0 or less, return "00:00"
        if (time <= 0)
        {
            timeText = "00:00";
        }
        else
        {
            float totalSeconds = time;
            float minutes = totalSeconds / 60;
            float seconds = totalSeconds % 60;

            timeText = $"{(int)minutes:D2}:{(int)seconds:D2}";
        }

        return timeText;
    }

    public void ActiveCanvasGroup()
    {
        canvasGroup.blocksRaycasts = true;
    }
    public void DeactiveCanvasGroup()
    {
        canvasGroup.blocksRaycasts = false;
    }
    public void Deactive()
    {
        this.gameObject.SetActive(false);
    }
    public void PlayItemIdleAnim(float delayTime, int i,String t,Action action)
    {
        if (i < items.Length)
        {
            DOVirtual.DelayedCall(delayTime, () =>
            {
                items[i].enabled = true;
                items[i].Play(t);
            });
        }
        else
        {
            if (action != null)
            {
                DOVirtual.DelayedCall(3, () =>
                {
                    action();
                });
            }
        }
    }
    public void ClaimReward()
    {
        if (timeRemaining > 0)
        {
            timeText.transform.DOScale(1.1f, 0.3f).OnComplete(() =>
            {
                timeText.transform.DOScale(1f, 0.3f);
            });
        }
        else
        {
            for (int i = 0; i < items.Length; i++)
            {
                items[i].enabled = false;
            }
            hasClaimed = true;
            SaveSystem.instance.addCoin(50);
            SaveSystem.instance.AddBooster(1, 2, 0);
            SaveSystem.instance.SaveData();
            SaveSystem.instance.SavePlayerPrefsInt("HasClaimtTreat", 1);
            UIManagerNew.Instance.HalloWeenTreat.timeText.text = $"The gift will be delivered in <size=55><color=green>Next Day</color></size>";
            DisplayClaim(() =>
            {
                Disappear();
            });
        }
    }
    public void DisplayClaim(Action action)
    {
        claimButton.GetComponent<Image>().sprite = buttonSprites[1];
        UIManagerNew.Instance.HalloWeenTreat.timeText.text = $"The gift will be delivered in <size=55><color=green>Next Day</color></size>";
        timeText.font = fontAsset[1];
        buttonText.font = fontAsset[1];
        claimButton.interactable = false;
        for (int i = 0; i <= items.Length; i++)
        {
            PlayItemIdleAnim(.8f * i + 1, i, "HalloWeenPackItemClaim", () =>
            {
                action();
            });
        } 
    }
}
