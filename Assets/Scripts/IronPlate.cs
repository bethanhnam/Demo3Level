using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
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
	public bool isFrezze;
	public bool hasAddForce;
	public List<HingeJoint2D> joints = new List<HingeJoint2D>();
	public List<NailControl> nailControls = new List<NailControl>();
	public Rigidbody2D rigidbody2D1;
	private Collider2D collider2D;
	[SerializeField] private Vector3 centerOfMass = new Vector3(-0.00177252f, -0.001291171f, 0f);


	private void Start()
	{
		holes = GetAllChildObjects(transform);
		centerPoints = new Vector3[holes.Length];
		hingeJoint2Ds = GetComponents<HingeJoint2D>();
		SetHingeJoint();
		rigidbody2D1 = this.GetComponent<Rigidbody2D>();
		collider2D = GetComponent<Collider2D>();


	}
	private void Update()
	{
		checkHinge();
		setPoint();
		//CheckPosition();
	}
	private void FixedUpdate()
	{
		rigidbody2D1.centerOfMass = centerOfMass;
	}

	public void SetNai()
	{
		for(int i = 0; i < holes.Length; i++)
		{
			if(holes[i].GetComponent<NailDetector>().hingeJoint2D != null)
			{
				if(holes[i].GetComponent<NailDetector>().Nail !=null)
				AddNail(holes[i].GetComponent<NailDetector>().Nail);
			}
		}
	}
	public void AddNail(NailControl nailControl)
	{
		if (!nailControls.Contains(nailControl))
		{
				nailControls.Add(nailControl);
		}
	}
	public void ClearNailControl()
	{
		nailControls.Clear();
	}
	public void RemoveHoleInIron(NailControl nailControl)
	{
		if (nailControls.Contains(nailControl))
		{
			nailControls.Remove(nailControl);
		}
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
	public void checkHinge()
	{
		
		foreach (var hinge in hingeJoint2Ds)
		{
			if (hinge.connectedBody == null)
			{
				hinge.enabled = false;
				joints.Remove(hinge);
			}
			else
			{
				hinge.enabled = true;
				hinge.connectedAnchor = Vector2.zero;
				//hinge.anchor = this.transform.InverseTransformPoint(hinge.connectedBody.transform.position);
				if (!joints.Contains(hinge))
				{
					joints.Add(hinge);
				}

			}
		}
		if (hingeJoint2Ds.Length > 0)
		{
			if (joints.Count >= 2)
			{
				if (!isFrezze)
					StartCoroutine(Freeze());
			}
			else
			{
				StartCoroutine(unFreeze());
			}
		}
		if (joints.IsNullOrEmpty())
		{
			if (rigidbody2D1.isKinematic == true)
			{
				rigidbody2D1.isKinematic = false;
			}
			else
			{
					if (!hasAddForce)
					{
						Vector3 movementDirection = this.rigidbody2D1.velocity.normalized;

						// Tính toán lực cần thêm vào dựa trên hướng di chuyển và forceMagnitude
						Vector3 forceToAdd = movementDirection * 0.2f;

						// Thêm lực vào Rigidbody
						rigidbody2D1.AddForce(forceToAdd, ForceMode2D.Impulse);
						hasAddForce = true;
					} 
			}
		}
	}
	IEnumerator Freeze()
	{
		yield return new WaitForSeconds(0.1f);
		rigidbody2D1.freezeRotation = true;
		rigidbody2D1.constraints = RigidbodyConstraints2D.FreezeAll;
		rigidbody2D1.gravityScale = 1f;
		isFrezze = true;
	}
	IEnumerator unFreeze()
	{
		yield return new WaitForSeconds(0.05f);
		isFrezze = false;
		rigidbody2D1.constraints = RigidbodyConstraints2D.None;
		rigidbody2D1.freezeRotation = false;
		if (rigidbody2D1.angularDrag < 2)
		{
			rigidbody2D1.angularDrag += 0.05f;
		}
		rigidbody2D1.gravityScale = 1.1f;

	}
	public void SetHingeJoint()
	{
		for (int i = 0; i < holes.Length; i++)
		{
			holes[i].GetComponent<NailDetector>().hingeJoint2D = hingeJoint2Ds[i];
		}
	}
	public HingeJoint2D GetHinge()
	{
		return hingeJoint2Ds[selectedHinge];
	}
	public void SetTrigger(bool set)
	{
		collider2D.isTrigger = set;
	}
	public void ResetHingeJoint()
	{
		checkHinge();
	}
	//public void CheckPosition()
	//{
	//	if (this.gameObject.activeSelf == true)
	//	{
	//		var endline = GameObject.FindFirstObjectByType<EndLine>();
	//		if (this.transform.position.y < endline.transform.position.y)
	//		{
	//			//Destroy(collision.gameObject);
	//			this.gameObject.SetActive(false);
	//			//InputManager.instance.numOfIronPlate--;
	//			var partical1 = Instantiate(endline.GetComponent<EndLine>().partical, this.transform.position, Quaternion.identity);
	//			Destroy(partical1, 1f);
	//			for (int i = 0; i < InputManager.instance.ignoreIronCollider.Count; i++)
	//			{
	//				if (this.GetComponent<IronPlate>() == InputManager.instance.ignoreIronCollider[i])
	//				{
	//					endline.GetComponent<EndLine>().ignoreIronCollider = true;
	//					goto ignoreCollider;
	//				}
	//			}
	//			if (endline.GetComponent<EndLine>().ignoreIronCollider == false)
	//			{
	//				InputManager.instance.numOfIronPlateCollider--;
	//			}
	//		}
	//		ignoreCollider:
	//		endline.GetComponent<EndLine>().ignoreIronCollider = false;
	//		//AudioManager.instance.PlaySFX("DropIron");
	//	}
	//}

}
