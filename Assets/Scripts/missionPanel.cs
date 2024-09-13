using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class missionPanel : MonoBehaviour
{
	bool canShow = true;
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void ShopDialog()
	{
		if (canShow)
		{
			canShow = false;
			this.gameObject.SetActive(true);
			Invoke("OffDialog", 3f);
				
		}

	}
	public void OffDialog()
	{
		this.gameObject.SetActive(false);
		canShow = true;
	}
}
