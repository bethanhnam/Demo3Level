using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class welcomeLevel : MonoBehaviour
{
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			UIManager.instance.DeactiveTime();
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
}
