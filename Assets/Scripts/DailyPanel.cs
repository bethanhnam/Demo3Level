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
	public CanvasGroup canvasGroup;
	public GameObject panelBoard;
	public RectTransform Blockpanel;

	private void Start()
	{
		//get last claim time");
		lastDate = SaveSystem.instance.days;
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
			}
		}

	}
	private void Update()
	{
		for (int i = 0; i < SaveSystem.instance.days; i++)
		{
			dayRewards[i].isClaim = true;
			dayRewards[i].GetComponent<Button>().interactable = false;
		}
		
	}
	public void OnClaimButtinPressed()
	{
		//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		if (dayRewards[lastDate].rewardImg.Length <= 1)
		{
			dayRewards[lastDate].rewardImg[0].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f).OnComplete(() =>
			{
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket, dayRewards[lastDate].magicTiket);
				SaveSystem.instance.SaveData();
				//Invoke("Close", 1f);
			});
		}
		else
		{
			dayRewards[lastDate].rewardImg[1].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f);
			dayRewards[lastDate].rewardImg[0].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f).OnComplete(() =>
			{
				dayRewards[lastDate].isClaim = true;
				SaveSystem.instance.days = lastDate + 1;
				SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket, dayRewards[lastDate].magicTiket);
				SaveSystem.instance.SaveData();
				//Invoke("Close", 1f);
			});
		}
	}
	public void OnClaimButtinPressedX2()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
			//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
			PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
			if (dayRewards[lastDate].rewardImg.Length <= 1)
			{
				dayRewards[lastDate].rewardImg[0].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f).OnComplete(() =>
				{
					dayRewards[lastDate].isClaim = true;
					SaveSystem.instance.days = lastDate + 1;
					SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket * 2, dayRewards[lastDate].magicTiket * 2);
					SaveSystem.instance.SaveData();
					//Invoke("Close", 1f);
				});
			}
			else
			{
				dayRewards[lastDate].rewardImg[1].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f);
				dayRewards[lastDate].rewardImg[0].rectTransform.DOAnchorPos(new Vector3(431.5f, 874f, 0), 0.5f).OnComplete(() =>
				{
					dayRewards[lastDate].isClaim = true;
					SaveSystem.instance.days = lastDate + 1;
					SaveSystem.instance.addTiket(dayRewards[lastDate].powerTicket * 2, dayRewards[lastDate].magicTiket * 2);
					SaveSystem.instance.SaveData();
					//Invoke("Close", 1f);
				});
			}
		});
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = true;
			this.gameObject.SetActive(true);
			panelBoard.gameObject.SetActive(false);
			panelBoard.transform.localScale = new Vector3(.9f, .9f, 1f);
			Blockpanel.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f);
			panelBoard.gameObject.SetActive(true);
			panelBoard.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f).OnComplete(() =>
			{
				panelBoard.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
				{
					Blockpanel.gameObject.SetActive(false);
				});
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				GameManager.instance.hasUI = false;
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				Blockpanel.gameObject.SetActive(false);
			});
			panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
		}

	}
}
