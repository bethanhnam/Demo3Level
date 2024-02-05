using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopPanel : MonoBehaviour
{
	public GameObject panel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			panel.transform.localPosition = new Vector2(0, -816.35f);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
		}
	}
	public void BackToMenu()
	{
		Close();
		UIManager.instance.menuPanel.Open();
	}
}
