using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using DG.Tweening;

public class MenuPanel : MonoBehaviour
{
	[Header("Panel")]
	public SettingPanel settingPanel;
	public DailyPanel dailyPanel;
	public NonAdsPanel nonAdsPanel;
	public LevelManager levelManager;
	public GameManager gameManager;

	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;
	public void PlayGame()
	{
		AudioManager.instance.PlaySFX("Button");
		Close();
		UIManager.instance.gamePlayPanel.Open();
	}
	public void OpenSettingPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_setting, () =>
		{
			settingPanel.Open();
		});
	}
	public void OpenDailyPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_daily, () =>
		{
			dailyPanel.Open();
		});

	}
	public void OpenNonAdsPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		nonAdsPanel.Open();

	}
	public void OpenShopPanel()
	{
		AudioManager.instance.PlaySFX("Button");
		AdsManager.instance.ShowInterstial(AdsManager.PositionAds.menu_shop, () =>
		{
			UIManager.instance.shopPanel.Open();
		});
	}

	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .7f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{

			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
			canvasGroup.alpha = 1;
			canvasGroup.DOFade(0, .7f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
				this.gameObject.SetActive(false);
			});
		}
	}
}
