using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtralHole : MonoBehaviour
{
	public string layerName = "Hole";
	public ExtraHoleButton extraHoleButton;
	private void Update()
	{
		try
		{
			extraHoleButton = FindFirstObjectByType<ExtraHoleButton>();
		}
		catch { }
	}
	public void UseTicket()
	{
		if (GameManager.instance.goldenStar > 0)
		{
			this.Close();
			GameManager.instance.goldenStar--;
			SaveSystem.instance.SetTiket(GameManager.instance.goldenStar, GameManager.instance.purpleStar);
			Level.instance.ChangeLayer();
			extraHoleButton.gameObject.SetActive(false);
		}
	}
	public void WatchAd()
	{
		// load ad 
		this.Close();
		Level.instance.ChangeLayer();
		extraHoleButton.gameObject.SetActive(false);
		
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
			GameManager.instance.hasUI = false;
			UIManager.instance.gamePlayPanel.timer.TimerOn = true;
		}
	}


}
