using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rankpanel : MonoBehaviour
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
