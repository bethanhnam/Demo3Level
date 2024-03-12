using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hole : MonoBehaviour
{
    public GameObject Nail;
    public bool collider;
	public List<Collider2D> colliders1 = new List<Collider2D>();
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
	private void Update()
	{
		if (this.CheckNail() == false)
		{
			Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(this.transform.position.x, this.transform.position.y), this.GetComponent<CircleCollider2D>().radius);
			bool hasCollider = false;
			foreach (Collider2D collider1 in colliders)
			{
				if (collider1.gameObject.tag == "Iron")
				{
					hasCollider = true;
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
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Nail")
		{
			Nail = collision.gameObject;
		}
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
