using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePanel : MonoBehaviour
{
	public RectTransform claimPanel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			PlayerPrefs.SetInt("HasComplete", 1);
			AudioManager.instance.PlaySFX("Winpop");
			claimPanel.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			claimPanel.gameObject.SetActive(true);
			this.gameObject.SetActive(false);
			GameManager.instance.currentLevel = LevelManager.instance.levelCount-1;
			SaveSystem.instance.SaveData();
			UIManager.instance.gamePlayPanel.pausePanel.Home();
		}
	}
}
