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
		Close();
		UIManager.instance.gamePlayPanel.Open();
	}
	public void OpenSettingPanel()
	{
		settingPanel.Open();

	}
	public void OpenDailyPanel()
	{

		dailyPanel.Open();

	}
	public void OpenNonAdsPanel()
	{
		nonAdsPanel.Open();

	}
	public void OpenShopPanel()
	{
		UIManager.instance.shopPanel.Open();
		
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
