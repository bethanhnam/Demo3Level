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
			LevelManager.instance.LoadLevel(GameManager.instance.currentLevel);
		}
	}
	public void WatchAd()
	{
		// load ad 
		LevelManager.instance.LoadLevel(GameManager.instance.currentLevel);
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
