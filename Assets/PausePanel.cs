using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : MonoBehaviour
{

	public void Home()
	{
		this.Close();
		UIManager.instance.gamePlayPanel.Close();
		UIManager.instance.menuPanel.Open();
	}
	public void Hint()
	{

	}
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
			this.gameObject.SetActive(false);
		}
	}
}
