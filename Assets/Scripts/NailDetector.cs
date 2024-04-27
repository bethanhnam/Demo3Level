using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NailDetector : MonoBehaviour
{
    public HingeJoint2D hingeJoint2D;

	public NailControl Nail;
	void Start()
    {
		Nail = hingeJoint2D.connectedBody.GetComponent<NailControl>();
	}

	// Update is called once per frame
	void Update()
	{
	}
	public void setHinge(HingeJoint2D hingeJoint2D)
	{
		this.hingeJoint2D = hingeJoint2D;
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Nail")
		{
			if (hingeJoint2D.enabled == false)
			{
				hingeJoint2D.enabled = true;
				hingeJoint2D.connectedBody = collision.GetComponent<Rigidbody2D>();
				hingeJoint2D.connectedAnchor = Vector2.zero;
				Nail = hingeJoint2D.connectedBody.GetComponent<NailControl>();
			}
		}
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
	public void RemoveNail()
	{
		if (Nail != null)
		{
			Nail = null;
		}
	}
}
