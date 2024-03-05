using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = true;
			this.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			GameManager.instance.hasUI = false;
			this.gameObject.SetActive(false);
		}
	}
	public void BackToMenu()
	{
		Close();
		//UIManager.instance.menuPanel.Open();
	}
}
