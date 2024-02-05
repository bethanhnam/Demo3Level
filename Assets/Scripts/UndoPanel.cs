﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndoPanel : MonoBehaviour
{
	public bool hasUse;
	public void UseTicket()
	{
		if (GameManager.instance.numOfSilverTicket > 0)
		{
			GameManager.instance.numOfSilverTicket--;
			hasUse = true;
			SaveSystem.instance.SetTiket(GameManager.instance.numOfSilverTicket, GameManager.instance.numOfSilverTicket);
			InputManager.instance.Undo();
			this.Close();
		}
	}
	public void WatchAd()
	{
		//xem qu?ng cáo 
		InputManager.instance.Undo();
		hasUse = true;
		this.Close();
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
