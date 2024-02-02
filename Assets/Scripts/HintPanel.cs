using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanel : MonoBehaviour
{
	public void UseTicket()
	{
		if (GameManager.instance.numOfTicket > 0)
		{
			this.Close();
			GameManager.instance.numOfTicket--;
			UIManager.instance.gamePlayPanel.ShowHint();
		}
	}
	public void WatchAd()
	{
		//loadAd
		this.Close();
		UIManager.instance.gamePlayPanel.ShowHint();


	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.gamePlayPanel.timer.TimerOn = true;
			GameManager.instance.hasUI = false;
		}
	}
}
