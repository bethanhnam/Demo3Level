using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
	public SettingPanel settingPanel;
	public DailyPanel dailyPanel;
	public NonAdsPanel nonAdsPanel;
	public LevelManager levelManager;
	public GameManager gameManager;

	public void PlayGame()
	{
		Close();
		// Load level from prefab
		UIManager.instance.gamePlayPanel.Open();

	}
	public void OpenSettingPanel()
	{
		this.Close();
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
		//Close();
		UIManager.instance.shopPanel.Open();
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
