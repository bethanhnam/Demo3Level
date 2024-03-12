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
		dayRewards[lastDate].rewardImg.rectTransform.DOAnchorPos(UIManager.instance.menuPanel.dailyPanel.transform.InverseTransformPoint(dayRewards[lastDate].reciveRewardpoint.position), 1f).OnComplete(() =>
		{
			dayRewards[lastDate].isClaim = true;
			SaveSystem.instance.days = lastDate + 1;
			SaveSystem.instance.purpleStar += dayRewards[lastDate].purpleStar;
			SaveSystem.instance.goldenStar += dayRewards[lastDate].GoldenStar;
			SaveSystem.instance.SaveData();
		});
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = true;
			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
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
		}

	}
}
