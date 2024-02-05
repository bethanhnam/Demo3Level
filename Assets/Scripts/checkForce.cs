using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForce : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "HoleInIron")
		{
			collision.GetComponent<NailDetector>().hingeJoint2D.enabled = true;
			collision.GetComponent<NailDetector>().hingeJoint2D.connectedBody = this.GetComponent<Rigidbody2D>();
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.tag == "HoleInIron")
		{
			collision.GetComponent<NailDetector>().hingeJoint2D.connectedBody = null;
			collision.GetComponent<NailDetector>().hingeJoint2D.enabled = false;
		}
	}
}
