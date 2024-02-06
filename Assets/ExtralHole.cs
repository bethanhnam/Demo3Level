using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtralHole : MonoBehaviour
{
	public string layerName = "Hole";
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			Level.instance.ChangeLayer();
		}
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
		}
	}
	

}
