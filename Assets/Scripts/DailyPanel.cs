using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DailyPanel : MonoBehaviour
{
	public RewardDaily[] dayRewards;
	public int lastDate=0;
	[SerializeField]private int selectReward = 0;
	public Button claim;
	public Button claimX2;
	public bool isClaim;
	public bool isClaimX2;
	public reciveRewardPanel reciveRewardPanel;
	public reciveRewardDaily reciveRewardDaily;
	public CanvasGroup canvasGroup;

    private void Awake()
    {
        lastDate = SaveSystem.instance.days;
    }
    private void Start()
	{
		checkDay();
	}

	private void checkDay()
	{
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
			if (SaveSystem.instance.days < 7)
			{
				dayRewards[SaveSystem.instance.days].isActive = true;
				claim.interactable = true;
				claim.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;
				claimX2.interactable = true;
			}
			else
			{
				claim.interactable = false;
				Color color = new Color(140f / 255f, 140f / 255f, 140f / 255f);
				claim.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
				claimX2.interactable = false;
			}
			
		}
		else
		{
			claim.interactable = false;
			Color color = new Color(140f / 255f, 140f / 255f, 140f / 255f);
			claim.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = color;
			claimX2.interactable = false;
		}
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
	}
	public void OnClaimButtinPressed()
	{
		//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		isClaim = true;
		//if (lastDate == 6)
		//{
			if (dayRewards[lastDate].rewardImg.Length <= 1)
			{
				reciveRewardPanel.Open();
			}
			else
			{
				reciveRewardPanel.Open();
			}
		//else
		//{
  //          reciveRewardDaily.gameObject.SetActive(true);
  //          reciveRewardDaily.claim();
  //      }
		SaveSystem.instance.SaveData();
		
    }
	public void OnClaimButtinPressedX2()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
			FirebaseAnalyticsControl.Instance.Daily_RW_x2(1);
			//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
			PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
			isClaimX2 = true;
			//if (lastDate == 6)
			//{
				if (dayRewards[lastDate].rewardImg.Length <= 1)
				{
					reciveRewardPanel.Open();
				}
				else
				{
					reciveRewardPanel.Open();
				}
			//}
			//else
			//{
   //             reciveRewardDaily.gameObject.SetActive(true);
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
		yield return new WaitForSeconds(1.7f);
		UIManagerNew.Instance.DailyRWUI.Close();
		reciveRewardPanel.Close();
		reciveRewardDaily.gameObject.SetActive(true);
		//reciveRewardDaily.claim();
		reciveRewardDaily.SpawnObjects(dayRewards[lastDate].gold,dayRewards[lastDate].magicTiket,dayRewards[lastDate].powerTicket, reciveRewardDaily.rewardImg.gameObject);
        isClaimX2 = false;
		AudioManager.instance.PlaySFX("ClosePopUp");

        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        {
            UIManagerNew.Instance.ButtonMennuManager.Appear();
        }
    }
	public void Open()
	{
		AudioManager.instance.PlaySFX("OpenPopUp");

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
}
