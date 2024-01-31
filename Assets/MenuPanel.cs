using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class MenuPanel : MonoBehaviour
{
	public SettingPanel settingPanel;
	public DailyPanel dailyPanel;
	public BackGroundPanel backGroundPanel;
	public SkinPanel skinPanel;
	public LevelManager levelManager;
	public GameManager gameManager;

	public void PlayGame()
	{
		Close();
		// Load level from prefab
		UIManager.instance.gamePlayPanel.Open();
		levelManager.LoadLevel(gameManager.currentLevel);

	}
	public void OpenSettingPanel()
	{
		settingPanel.Open();
	}
	public void OpenDailyPanel()
	{
		dailyPanel.Open();
	}
	public void OpenBackGroundPanel()
	{
		backGroundPanel.Open();
	}
	public void OpenSkinPanel()
	{
		skinPanel.Open();
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
