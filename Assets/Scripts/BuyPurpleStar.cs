using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyPurpleStar : MonoBehaviour
{

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
	public void ExchangeStar()
	{
		if(SaveSystem.instance.goldenStar >= 2) {
			SaveSystem.instance.goldenStar -= 2;
			SaveSystem.instance.purpleStar += 1;
		}
	}
}
