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



	private void Start()
	{
		holes = GetAllChildObjects(transform);
		centerPoints = new Vector3[holes.Length];
		hingeJoint2Ds = GetComponents<HingeJoint2D>();
		SetHingeJoint();

	}
	private void Update()
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
		radius = holes[0].GetComponent<CircleCollider2D>().radius / 32;
		float reference = radius;
		for (int i = 0; i < centerPoints.Length; i++)
		{
			float distance = Vector2.Distance(holePosition, centerPoints[i]);
			if (distance < reference)
			{
				selectedHinge = i;
				//hingeJoint2Ds[selectedHinge].anchor = new Vector2(holePosition.x, holePosition.y);
				result = true;
			}
		}
		return result;
	}
	private void OnDrawGizmos()
	{
		foreach (var centerPoint in centerPoints)
		{
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(centerPoint, radius);
		}
	}

	//public bool checkNail( Vector2 holePosition)
	//{
	//	result = false;
	//	radius = holes[0].GetComponent<CircleCollider2D>().radius / 2;
	//	float reference = radius;
	//	for (int i = 0; i < centerPoints.Length; i++)
	//	{
	//		float distance = Vector2.Distance(holePosition, centerPoints[i]);
	//		if (distance < reference)
	//		{
	//			selectedhole = i;
	//			result = true;
	//		}
	//	}
	//	return result;
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
				hinge.autoConfigureConnectedAnchor = true;
				if (!joints.Contains(hinge))
				{
					joints.Add(hinge);
				}
				
			}
		}
		if (hingeJoint2Ds.Length >= 3)
		{
			if (joints.Count >= hingeJoint2Ds.Length - 1)
			{
				this.GetComponent<Rigidbody2D>().freezeRotation = true;
			}
			else
			{
				this.GetComponent<Rigidbody2D>().freezeRotation = false;
			}
		}
	}
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
