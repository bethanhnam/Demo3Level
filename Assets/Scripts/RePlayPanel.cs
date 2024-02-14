using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RePlayPanel : MonoBehaviour
{
	public void UseTicket()
	{
		if(GameManager.instance.goldenStar > 0)
		{
			this.Close();
			GameManager.instance.goldenStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
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
