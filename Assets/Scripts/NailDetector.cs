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
	}
	public void setHinge(HingeJoint2D hingeJoint2D)
	{
		this.hingeJoint2D = hingeJoint2D;
	}
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
