using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinPanel : MonoBehaviour
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
			this.gameObject.SetActive(false);
		}
	}
}
