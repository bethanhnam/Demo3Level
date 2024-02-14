using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Winpop : MonoBehaviour
{
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			UIManager.instance.gamePlayPanel.Close();
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			SaveSystem.instance.level = 0;
			SaveSystem.instance.LoadData();
			GameManager.instance.currentLevel = 0;
			UIManager.instance.menuPanel.Open();
			UIManager.instance.chestPanel.slider.value = 0;
		}
	}
}
