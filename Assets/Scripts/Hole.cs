using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hole : MonoBehaviour
{
	public NailControl Nail;
	public bool extraHole;
	public bool collider;
	public bool isOsccupied;
	private LayerMask IronLayer;
	private void Start()
	{
		IronLayer = LayerMask.GetMask("IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "IronLayer9", "BothLayer", "layer1vs2", "layer1vs2vs3", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
	}
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
	public void RemoveNail() {
		if (Nail!=null)
		{
			Nail = null;
		}
	}

	private void Update()
	{
	
		if (!extraHole)
		{
			if (this.CheckNail() == false)
			{
				Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), 0.13f,IronLayer);
				bool hasCollider = false;
				if (colliders.IsNullOrEmpty())
				{
					isOsccupied = false;
				}
				else
				{
					hasCollider = true;
					if (checkOsccupied(colliders))
					{
						isOsccupied = true;
					}
					else
					{
						isOsccupied = false;
					}
				}
				if (hasCollider == false)
				{
					collider = false;
				}
				else
				{
					collider = true;
				}
			}
			else
			{
				if (!getNail().gameObject.activeSelf)
				{
					Nail = null;
					isOsccupied = false;
				}
				else
				{
					isOsccupied = true;
				}
			}
		}
	}
	public bool checkOsccupied(Collider2D[] colliders)
	{
		bool x = false;
		foreach (Collider2D collider1 in colliders)
		{
			if (collider1.gameObject.tag == "Iron")
			{
				if (collider1.GetComponent<IronPlate>().checkHitPoint(this.transform.position) == true)
				{
                    x = false;
                }
				else
				{
                    x = true;
                    return true;
                }
			}
		}
		return x;
	}
	
	//public void CheckIron()
	//{
	//	Collider2D[] colliders = Physics2D.OverlapCircleAll(this.transform.localPosition, .2f);
	//	foreach (Collider2D collider1 in colliders)
	//	{
	//		if (collider1.gameObject.tag == "Iron")
	//		{
	//			if (!colliders1.Contains(collider1)){
	//				colliders1.Add(collider1);
	//			}
	//		}
	//	}
	//}
}
