using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardLevel : MonoBehaviour
{
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			UIManager.instance.DeactiveTime();
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
