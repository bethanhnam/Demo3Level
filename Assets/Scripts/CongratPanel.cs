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
			AudioManager.instance.PlaySFX("OpenPopUp");
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			var reward = Random.Range(0,1);
			if (reward == 0)
			{
				SaveSystem.instance.goldenStar++;
			}
			else
			{
				SaveSystem.instance.purpleStar++ ;
			}
			SaveSystem.instance.SaveData();
			this.gameObject.SetActive(false);
			AudioManager.instance.PlaySFX("ClosePopUp");
			UIManager.instance.gamePlayPanel.backFromChestPanel = true;
			UIManager.instance.gamePlayPanel.backFromPause = false;
			UIManager.instance.gamePlayPanel.Open();
		}
	}
}
