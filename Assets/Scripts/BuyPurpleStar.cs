using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuymagicTiket : MonoBehaviour
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
			 UIManager.instance.ActiveTime();
			GameManager.instance.hasUI = false;
		}
	}
	public void ExchangeStar()
	{
		if(SaveSystem.instance.powerTicket >= 2) {
			SaveSystem.instance.powerTicket -= 2;
			SaveSystem.instance.magicTiket += 1;
		}
	}
}
