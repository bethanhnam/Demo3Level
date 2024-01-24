using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hode : MonoBehaviour
{
	[SerializeField]private bool hitable;

	public bool Hitable { get => hitable; }
	public void setHitable(bool value)
	{
		hitable = value;
	}
}
