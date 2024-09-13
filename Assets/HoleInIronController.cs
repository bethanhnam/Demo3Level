using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleInIronController : MonoBehaviour
{
	public NailControl Nail;
	public NailControl getNail()
	{
		return Nail;
	}
	public bool CheckNail()
	{
		if (Nail != null)
		{
			return true;
		}
		else return false;

	}
	public void setNail(NailControl nail)
	{
		Nail = nail;
	}
	public void RemoveNail()
	{
		if (Nail != null)
		{
			Nail = null;
		}
	}
}
