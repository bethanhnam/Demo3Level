using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CongratPanel : MonoBehaviour
{
	public RectTransform rewardImg;
	public RectTransform rewardLight;
	public RectTransform rewardOpen;
	public RectTransform rewardStart;
	public RectTransform claimPanel;
	public RectTransform item;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			var reward = Random.Range(0, 2);
			if (reward == 0)
			{
				SaveSystem.instance.addTiket(1, 0);
				item.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/congrat/ticket_blue_big");
			}
			else
			{

				item.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/congrat/ticket_purple_big");
				SaveSystem.instance.addTiket(0, 1);
			}
			AudioManager.instance.PlaySFX("OpenPopUp");
			rewardLight.GetComponent<CanvasGroup>().alpha = 0f;
			claimPanel.gameObject.SetActive(false);
			rewardImg.gameObject.SetActive(true);
			rewardLight.gameObject.SetActive(false);
			rewardOpen.gameObject.SetActive(false);
			rewardOpen.localScale = Vector3.one;
			rewardLight.localScale = Vector3.one;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			SaveSystem.instance.SaveData();
			rewardOpen.gameObject.SetActive(false);
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
			rewardLight.GetComponent<CanvasGroup>().DOFade(1, 0.6f);
			rewardLight.DOScale(1.3f, 0.5f).OnComplete(() =>
			{
				rewardImg.gameObject.SetActive(false);
				rewardOpen.gameObject.SetActive(true);
				rewardOpen.DOScale(1.2f, 1f).OnComplete(() =>
				{
					AudioManager.instance.PlaySFX("GetReward");
					rewardOpen.gameObject.SetActive(false);
					rewardLight.gameObject.SetActive(false);
					claimPanel.gameObject.SetActive(true);
				});
			});
		});

	}
}
