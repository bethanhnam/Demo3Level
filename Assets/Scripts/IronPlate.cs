using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class IronPlate : MonoBehaviour
{
	public HingeJoint2D[] hingeJoint2Ds;
	public GameObject[] holes;
	public Vector3[] centerPoints;
	public int selectedhole;
	[SerializeField] private float radius;
	public int selectedHinge = 0;
	public bool result;
	public List<HingeJoint2D> joints = new List<HingeJoint2D>();
	private Rigidbody2D rigidbody2D;
	[SerializeField] private Vector3 centerOfMass = new Vector3(-0.00177252f, 0.001291171f,0f);
	[SerializeField] private Vector3 centerOfMass1;
	//-0.02177252
	//0.004291171


	private void Start()
	{
		holes = GetAllChildObjects(transform);
		centerPoints = new Vector3[holes.Length];
		hingeJoint2Ds = GetComponents<HingeJoint2D>();
		SetHingeJoint();
		rigidbody2D  = this.GetComponent<Rigidbody2D>();
		centerOfMass1 = rigidbody2D.centerOfMass;
		rigidbody2D.centerOfMass = centerOfMass;

	}
	private async void Update()
	{
		setPoint();
		checkHinge();
	}

	
	public void setPoint()
	{
		for (int i = 0; i < holes.Length; i++)
		{
			centerPoints[i] = holes[i].GetComponent<CircleCollider2D>().bounds.center;
		}
	}
	private GameObject[] GetAllChildObjects(Transform parent)
	{
		GameObject[] childObjects = new GameObject[parent.childCount];

		for (int i = 0; i < parent.childCount; i++)
		{
			childObjects[i] = parent.GetChild(i).gameObject;
		}

		return childObjects;
	}
	public bool checkHitPoint(Vector2 holePosition)
	{
		result = false;
		radius = 0.05f;
		float reference = radius;
		for (int i = 0; i < centerPoints.Length; i++)
		{
			float distance = Vector2.Distance(holePosition, centerPoints[i]);
			if (distance < reference)
			{
				selectedHinge = i;
				result = true;
			}
		}
		return result;
	}
	//private void OnDrawGizmos()
	//{
	//	if (this.isActiveAndEnabled)
	//	{
	//		foreach (var centerPoint in centerPoints)
	//		{
	//			Gizmos.color = Color.black;
	//			Gizmos.DrawWireSphere(centerPoint, radius);
	//		}
	//		Gizmos.color = Color.red;
	//		Gizmos.DrawSphere(transform.position + transform.rotation * centerOfMass, 0.1f);
	//	}
	//}
	private void checkHinge()
	{
		foreach(var hinge in hingeJoint2Ds)
		{
			if(hinge.connectedBody == null)
			{
				hinge.enabled = false;
				joints.Remove(hinge);
			}
			else
			{
				hinge.enabled = true;
				hinge.connectedAnchor = Vector2.zero;
				//hinge.autoConfigureConnectedAnchor = true;
				if (!joints.Contains(hinge))
				{
					joints.Add(hinge);
				}
				
			}
		}
		if (hingeJoint2Ds.Length >= 2)
		{
			if (joints.Count >= 2)
			{
				this.GetComponent<Rigidbody2D>().freezeRotation = true;
				this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
			}
			else
			{
				this.GetComponent<Rigidbody2D>().freezeRotation = false;
				this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
				//StartCoroutine(UnFreeze());
			}
		}
	}
	//IEnumerator UnFreeze()
	//{
	//	yield return new WaitForSeconds(0.1f);
	//	this.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
	//	//rigidbody2D.AddTorque(10f);
	//}
	public void SetHingeJoint()
	{
		for(int i = 0;i<holes.Length;i++)
		{
			holes[i].GetComponent<NailDetector>().hingeJoint2D  = hingeJoint2Ds[i];
		}
	}
	public HingeJoint2D GetHinge()
	{
		return hingeJoint2Ds[selectedHinge];
	}

}
