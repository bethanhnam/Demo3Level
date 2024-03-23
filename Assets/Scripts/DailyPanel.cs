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
	public Button claim;
	public Button claimX2;
	public bool isClaim;
	public bool isClaimX2;
	public reciveRewardPanel reciveRewardPanel;

	private void Start()
	{
		checkDay();

	}

	private void checkDay()
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
		for (int i = 0; i < SaveSystem.instance.days; i++)
		{
			dayRewards[i].isClaim = true;
		}
		checkDay();


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
		
		checkDay();
	}
	public void OnClaimButtinPressedX2()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
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
		
		checkDay();
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
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.alpha = 1;
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.DOFade(0, .3f).OnComplete(() =>
			{
				if (reciveRewardPanel.gameObject.activeSelf)
				{
					reciveRewardPanel.gameObject.SetActive(false);
				}
				GameManager.instance.hasUI = false;
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				Blockpanel.gameObject.SetActive(false);
			});
			panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
		}

	}
}
