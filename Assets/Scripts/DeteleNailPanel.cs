using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteleNailPanel : MonoBehaviour
{
	public bool hasUse;
	public void UseTicket()
	{
		if (GameManager.instance.purpleStar > 0)
		{
			this.Close();
			GameManager.instance.purpleStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
			//hasUse = true;
			GameManager.instance.deleting = true;
			UIManager.instance.gamePlayPanel.ButtonOff();
		}
	}
	public void WatchAd()
	{
		//xem qu?ng cáo 
		this.Close();
		//xoá nail(Đồng hồ đếm giờ dừng lại)
		GameManager.instance.deleting = true;
		//hasUse = true;
		UIManager.instance.gamePlayPanel.ButtonOff();
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
