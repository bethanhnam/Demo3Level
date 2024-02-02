using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailDetector : MonoBehaviour
{
    public HingeJoint2D hingeJoint2D;
    void Start()
    {
    }

	// Update is called once per frame
	void Update()
	{
		//// Lấy vị trí trung tâm của hai hình tròn
		//if (hingeJoint2D.connectedBody == null)
		//{
		//	Vector3 center1 = this.transform.position;
		//	foreach (GameObject hole in InputManager.instance.holeObjects)
		//	{
		//		Vector3 center2 = hole.transform.position;

		//		// Tính khoảng cách Euclidean giữa hai trung tâm
		//		float distance = Vector3.Distance(center1, center2);
		//		Debug.Log(distance);

		//		// Tính tổng bán kính của hai hình tròn
		//		float totalRadius = 0.2f;

		//		// Kiểm tra độ trùng
		//		if (distance <= totalRadius)
		//		{
		//			if (hole.GetComponent<Hole>().CheckNail() == true)
		//			{
		//				this.hingeJoint2D.connectedBody = hole.GetComponent<Hole>().getNail().GetComponent<Rigidbody2D>();
		//			}
		//		}
		//	}
		//}
	}
	public void setHinge(HingeJoint2D hingeJoint2D)
	{
		this.hingeJoint2D = hingeJoint2D;
	}
	////public void OffHinge()
	////{
	////	if (hingeJoint2D.enabled == true)
	////	{
	////		hingeJoint2D.connectedBody = null;
	////		hingeJoint2D.enabled = false;
	////	}
	////}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Nail")
		{
			Debug.Log(collision.name);
			if (hingeJoint2D.enabled == false)
			{
				hingeJoint2D.enabled = true;
				hingeJoint2D.connectedBody = collision.GetComponent<Rigidbody2D>();
				hingeJoint2D.connectedAnchor = Vector2.zero;
			}
		}
	}
}
