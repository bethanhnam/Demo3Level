using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CongratPanel : MonoBehaviour
{
	public RectTransform rewardImg;
	public RectTransform rewardLight;
	public RectTransform rewardOpen;
	public RectTransform rewardStart;
	public RectTransform claimPanel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);

			AudioManager.instance.PlaySFX("OpenPopUp");
			rewardLight.GetComponent<CanvasGroup>().alpha = 0f;
			claimPanel.gameObject.SetActive(false);
			rewardImg.gameObject.SetActive(true);
			rewardLight.gameObject.SetActive(false);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			var reward = Random.Range(0, 1);
			if (reward == 0)
			{
				SaveSystem.instance.goldenStar++;
			}
			else
			{
				SaveSystem.instance.purpleStar++;
			}
			SaveSystem.instance.SaveData();
			this.gameObject.SetActive(false);
			AudioManager.instance.PlaySFX("ClosePopUp");
			UIManager.instance.gamePlayPanel.backFromChestPanel = true;
			UIManager.instance.gamePlayPanel.backFromPause = false;
			UIManager.instance.gamePlayPanel.Open();
		}
	}
	public void TakeReward()
	{
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_bonus, () =>
		{
			rewardLight.gameObject.SetActive(true);
			rewardLight.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
			rewardLight.DOScale(1.3f, 0.5f).OnComplete(() =>
			{
				rewardImg.gameObject.SetActive(false);
				rewardLight.gameObject.SetActive(false);
				claimPanel.gameObject.SetActive(true);
			});
		});

	}
}
