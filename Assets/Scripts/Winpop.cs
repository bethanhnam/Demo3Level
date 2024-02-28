using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Winpop : MonoBehaviour
{
	public TextMeshProUGUI PlayTimeText;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			SaveSystem.instance.playing = false;
			int minutes = Mathf.FloorToInt(SaveSystem.instance.playTime / 60);
			int seconds = Mathf.FloorToInt(SaveSystem.instance.playTime % 60);
			PlayTimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
			UIManager.instance.gamePlayPanel.Close();
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			SaveSystem.instance.level = 0;
			SaveSystem.instance.LoadData();
			GameManager.instance.currentLevel = 0;
			UIManager.instance.menuPanel.Open();
			UIManager.instance.chestPanel.slider.value = 0;
			SaveSystem.instance.playing = true;
			SaveSystem.instance.playTime = 0;
		}
	}
}
