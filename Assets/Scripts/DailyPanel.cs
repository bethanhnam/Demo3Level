using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DailyPanel : MonoBehaviour
{
	public Reward[] dayRewards;
	public int lastDate;
	[SerializeField]private int selectReward = 0;
	[SerializeField] private Button claimButton;
	[SerializeField] private TextMeshProUGUI timeLeft;

	private void Start()
	{
		//get last claim time
		lastDate = PlayerPrefs.GetInt("Days");
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
			claimButton.interactable = true;
		}
		else
		{
			claimButton.interactable = false;
			timeLeft.text = GetTimeToNextClaim();
		}
	}
	private void Update()
	{
		for (int i = 0; i < lastDate; i++)
		{
			dayRewards[i].isClaim = true;
		}
	}
	private string GetTimeToNextClaim()
	{
		int hours = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalHours);
		int minutes = Mathf.FloorToInt((float)(DateTime.Today.AddDays(1) - DateTime.Now).TotalMinutes) %60;
		return (hours + " hours and " + minutes + " minutes left to claim next prize!");
	}
	public void OnClaimButtinPressed()
	{
		dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		ClaimGift();
		dayRewards[lastDate].isClaim = true;
		PlayerPrefs.SetInt("Days", lastDate+1);
		SaveSystem.instance.purpleStar += dayRewards[lastDate].purpleStar;
		SaveSystem.instance.goldenStar += dayRewards[lastDate].GoldenStar;
		SaveSystem.instance.SaveData();
	}
	public void ClaimGift()
	{
		claimButton.interactable = false;
		timeLeft.text = GetTimeToNextClaim();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
		}
	}
}
