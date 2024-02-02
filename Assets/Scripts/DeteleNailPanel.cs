using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeteleNailPanel : MonoBehaviour
{
	public void UseTicket()
	{
		if (GameManager.instance.numOfTicket > 0)
		{
			this.Close();
			GameManager.instance.numOfTicket--;
		}
	}
	public void WatchAd()
	{
		//xem qu?ng cáo 
		this.Close();
		//xoá nail(Đồng hồ đếm giờ dừng lại)
		GameManager.instance.deleting = true;
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
