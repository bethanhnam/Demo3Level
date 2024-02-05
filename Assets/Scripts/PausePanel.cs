using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{

	public void Home()
	{
		this.Close();
		UIManager.instance.gamePlayPanel.backFromPause =false;
		UIManager.instance.gamePlayPanel.Close();
		UIManager.instance.menuPanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.Close();
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.gamePlayPanel.timer.TimerOn = true;
			GameManager.instance.hasUI = false;
			UIManager.instance.gamePlayPanel.backFromPause = true;
			UIManager.instance.gamePlayPanel.Open();


		}
	}
	public void Retry()
	{
		Close();
		GameManager.instance.Replay();
		UIManager.instance.gamePlayPanel.backFromPause = false;
	}
}
