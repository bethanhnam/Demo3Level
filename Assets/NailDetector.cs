using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailDetector : MonoBehaviour
{
    [SerializeField] private HingeJoint2D hingeJointInParent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(hingeJointInParent.connectedBody == null)
        {
            hingeJointInParent.enabled = false;
        }
    }
	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.tag == "Nail")
        {
			hingeJointInParent.enabled = true;
			hingeJointInParent.connectedBody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }
	
}
