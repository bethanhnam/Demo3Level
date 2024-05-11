using DG.Tweening;
using DG.Tweening.Core.Easing;
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
	public RectTransform tapToOpen;

	public int typeOfReward;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			typeOfReward = Random.Range(0, 2);
			if (typeOfReward == 0)
			{
				item.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/congrat/ticket_blue_big");
			}
			else
			{
				item.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/UI Nut/export/congrat/ticket_purple_big");
			}
			AudioManager.instance.PlaySFX("OpenPopUp");
			rewardLight.GetComponent<CanvasGroup>().alpha = 0f;
			claimPanel.gameObject.SetActive(false);
			rewardImg.gameObject.SetActive(true);
			
			tapToOpen.gameObject.SetActive(true);
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
			SaveSystem.instance.SaveData();
			//if (SaveSystem.instance.menuLevel == MenuLevelManager.instance.levelCount)
			//{
			//	UIManager.instance.completePanel.Open();
			//	MenuLevelManager.instance.RemoveLevel(SaveSystem.instance.menuLevel);
				
			//}
			//else
			//{
			//	PlayerPrefs.SetInt("hasFlip", 0);
			//	UIManager.instance.gamePlayPanel.pausePanel.Home();
			//	MenuLevelManager.instance.RemoveLevel(SaveSystem.instance.menuLevel);
			//	PlayerPrefs.SetInt("HasComplete", 0);
			//	MenuLevelManager.instance.LoadLevel(SaveSystem.instance.menuLevel);
			//}
		}
	}
	public void OpenTakeRewardPanel()
	{
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_bonus, () =>
		{
			tapToOpen.gameObject.SetActive(false );
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
		},null);

	}
	public void TakeReward()
	{
		if (typeOfReward == 0)
		{
			SaveSystem.instance.addTiket(1, 0);
		}
		else
		{
			SaveSystem.instance.addTiket(0, 1);
		}
		SaveSystem.instance.SaveData();
	}
	public void ComPleteImgViaButton()
	{
		GameManagerNew.Instance.CompleteImgAppearViaButton();
		Close();
	}
}
