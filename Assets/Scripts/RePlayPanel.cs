using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePlayPanel : MonoBehaviour
{
	public void UseTicket()
	{
		if(GameManager.instance.numOfTicket > 0)
		{
			this.Close();
			GameManager.instance.numOfTicket--;
			GameManager.instance.Replay();
		}
	}
	public void WatchAd()
	{
		// load ad 
		this.Close();
		GameManager.instance.Replay();
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
			GameManager.instance.hasUI = false;
			UIManager.instance.gamePlayPanel.timer.TimerOn = true;
		}
	}
}
