using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HalloWeenTreat : MonoBehaviour
{
    public Animator animator;
    public CanvasGroup canvasGroup;
    public Animator[] items;

    public Image presentImage;
    public Sprite[] itemSprites;
    public Sprite[] presentSprites;
    public Sprite[] buttonSprites;
    public TMP_FontAsset[] fontAsset;

    private int appear = Animator.StringToHash("PackSaleHalloWeen");
    private int disappear = Animator.StringToHash("PackSaleHalloWeenDisappear");

    public bool timeOn;
    public float timeDefault = 300;
    public float timeRemaining = 0;


    public Button claimButton;
    public TextMeshProUGUI timeText;

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
        claimButton.interactable = false;
    }
    public void DeactiveObjects()
    {
        timeRemaining = 0;
        timeOn = false;
    }
    [Button("Appear")]
    public void Appear()
    {
        if(this.gameObject.activeSelf == false)
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
                claimButton.interactable = true;
                timeText.text = "Claim";
            }
            else
            {
                hasClaimed = true;
                DisplayForClaimed();
                timeText.text = "Next Day";
            }
            
        }
        animator.Play(appear);
        DOVirtual.DelayedCall(1, () =>
        {
            if (hasClaimed == false)
            {
                PlayItemIdleAnim(0);
                presentImage.GetComponent<Animator>().enabled = true;
            }
            else
            {
                claimButton.interactable = false;
                for (int i = 0; i < items.Length; i++)
                {
                    items[i].enabled = false;
                }
                PlayItemIdleClaimed(0);
                presentImage.GetComponent<Animator>().enabled = false;
            }
        });
    }
    
    public void Disappear()
    {
        for (int i = 0; i < items.Length; i++)
        {
            items[i].enabled = false;
        }
        DeactiveCanvasGroup();
        animator.Play(disappear);
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
        claimButton.GetComponent<Image>().sprite = buttonSprites[1];
        timeText.font = fontAsset[1];
        claimButton.interactable = false;
        presentImage.sprite = presentSprites[1];
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
    public void PlayItemIdleAnim(int i)
    {
        if (hasClaimed)
        {
            return;
        }
        if (i < items.Length)
        {
            items[i].enabled = true;
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (hasClaimed)
                {
                    return;
                }
                //items[i].enabled = false;
                i++;
                PlayItemIdleAnim(i);
            });
        }
        else
        {
            if (hasClaimed)
            {
                return;
            }
            i = 0;
            PlayItemIdleAnim(i);
        }
    }
    public void PlayItemIdleClaimed(int i)
    {
        if (i < items.Length)
        {
            items[i].enabled = true;
            items[i].Play("HalloWeenPackItemClaimed");
            DOVirtual.DelayedCall(0.5f, () =>
            {
                i++;
                PlayItemIdleClaimed(i);
            });
        }
        else
        {
            i = 0;
            PlayItemIdleClaimed(i);
        }
    }
    public void ClaimReward()
    {
        hasClaimed = true;
        SaveSystem.instance.addCoin(50);
        SaveSystem.instance.AddBooster(1, 2, 0);
        SaveSystem.instance.SaveData();
        SaveSystem.instance.SavePlayerPrefsInt("HasClaimtTreat", 1);
        DisplayClaim(() =>
        {
            Disappear();
        });
    }
    public void DisplayClaim(Action action)
    {
        hasClaimed = true;
        claimButton.interactable = false;
        presentImage.GetComponent<Animator>().Play("PresentOpen");
        for (int i = 0; i < items.Length; i++)
        {
            items[i].enabled = false;
        }
        ChangeImageItem(0, () =>
        {
            action();
        });
        
    }
    public void ChangeImageItem(int index, Action action)
    {
        if (index < items.Length)
        {
            items[index].enabled = true;
            items[index].Play("HalloWeenPackItemClaim");
            DOVirtual.DelayedCall(1f, () =>
            {
                index++;
                
                    ChangeImageItem(index, action);
            });
        }
        else
        {
            action.Invoke();
        }
    }

    public void ChangePresentSprite()
    {
        presentImage.sprite = presentSprites[1];
    }

    public void SetDefaultDisplay()
    {

    }
}
