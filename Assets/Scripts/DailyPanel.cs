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
	public CanvasGroup canvasGroup;

	private void Start()
	{
		checkDay();

	}

	private void checkDay()
	{
		//get last claim time");
		lastDate = DailyRWManager.instance.days;
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
			if (DailyRWManager.instance.days < 7)
			{
				dayRewards[DailyRWManager.instance.days].isActive = true;
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
		for (int i = 0; i < DailyRWManager.instance.days; i++)
		{
			dayRewards[i].isClaim = true;
		}
	}
	public void OnClaimButtinPressed()
	{
		//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
		PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
		isClaim = true;
		if (dayRewards[lastDate].rewardImg.Length <= 1)
		{
			reciveRewardPanel.Show(lastDate, 1,() => {
			});
		}
		else
		{
			reciveRewardPanel.Show(lastDate, 2, () => {
			});
		}
	}
	public void OnClaimButtinPressedX2()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
			FirebaseAnalyticsControl.Instance.LogEventX2Reward(1);
			//dayRewards[lastDate].Active.gameObject.SetActive(true) ;
			PlayerPrefs.SetString("LastClaimTime", DateTime.Today.ToString());
			isClaimX2 = true;
			if (dayRewards[lastDate].rewardImg.Length <= 1)
			{
				reciveRewardPanel.Show(lastDate,1, () => {
				});
			}
			else
			{
				reciveRewardPanel.Show(lastDate,2, () => {
				});
			}
		});
	}
	public void Claim()
	{
		if (isClaim)
		{
			reciveRewardPanel.Claim(lastDate, () => {
				StartCoroutine(ClosePanel());
			});
		}
		if (isClaimX2)
		{
			reciveRewardPanel.ClaimX2(lastDate, () =>
			{
				StartCoroutine(ClosePanel());
			});
		}
	}
	IEnumerator ClosePanel()
	{
		yield return new WaitForSeconds(1f);
		reciveRewardPanel.Close();
		isClaimX2 = false;
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
				AudioManager.instance.PlaySFX("ClosePopUp");
		}

	}
}
