using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CongratPanel : MonoBehaviour
{
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
			SaveSystem.instance.purpleStar++;
			this.gameObject.SetActive(false);
			UIManager.instance.gamePlayPanel.backFromChestPanel = true;
			UIManager.instance.gamePlayPanel.backFromPause = false;
			UIManager.instance.gamePlayPanel.Open();
		}
	}
}
