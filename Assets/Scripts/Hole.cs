using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject Nail;
    public GameObject getNail()
    {
        return Nail;
    }
	public bool CheckNail()
	{
		if(Nail != null)
		{
			return true;
		}
		else return false;

	}
	public void setNail(GameObject nail)
	{
		Nail = nail;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.tag == "Nail")
		{
			Nail = collision.gameObject;
		}
	}
}
