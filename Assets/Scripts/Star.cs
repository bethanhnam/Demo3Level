using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{
	public bool isActive;
	private void OnMouseOver()
	{
		if(Ratting.instance.isRated == false) { 
		isActive = true;
		}
	}
	private void OnMouseExit()
	{
		if (Ratting.instance.isRated == false)
		{
			isActive = false;
		}
	}
}
