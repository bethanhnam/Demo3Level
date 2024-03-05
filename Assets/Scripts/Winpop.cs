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
			SaveSystem.instance.playingHard = false;
			int minutes = Mathf.FloorToInt(SaveSystem.instance.playHardTime / 60);
			int seconds = Mathf.FloorToInt(SaveSystem.instance.playHardTime % 60);
			PlayTimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
			SaveSystem.instance.playHardTime = 0;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.chestPanel.Open();
		}
	}
}
