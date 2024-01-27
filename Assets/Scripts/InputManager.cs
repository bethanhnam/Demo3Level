using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;
	[SerializeField] private GameObject nailPrefab;
	[SerializeField] private GameObject selectedNail;
	public GameObject selectedHole;
	public GameObject selectedIron;
	public List<HingeJoint2D> preHingeJoint2D = new List<HingeJoint2D>();
	[SerializeField] private GameObject preHole;
	[SerializeField] private LayerMask iNSelectionLayer;
	[SerializeField] private LayerMask IronLayer;
	[SerializeField] private LayerMask placeLayer;
	public GameObject[] ironObjects;
	public GameObject[] holeObjects;

	//public RaycastHit2D[] cubeHit;

	public int numOfIronPlate;

	private void Awake()
	{
		iNSelectionLayer = LayerMask.GetMask("Hole");
		placeLayer = LayerMask.GetMask("Hole", "IronLayer1", "IronLayer2", "IronLayer3");
		IronLayer = LayerMask.GetMask("IronLayer1", "IronLayer2", "IronLayer3");
		
	}
	// Start is called before the first frame update
	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		holeObjects = GameObject.FindGameObjectsWithTag("Hole");
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
		setHinge();
	}

	// Update is called once per frame
	void Update()
	{
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
		
			selectHole();

	}
	private void checkNails()
	{
		for (int i = 0; i < holeObjects.Length; i++) {
			foreach (GameObject obj in ironObjects)
			{
				if (obj.GetComponent<IronPlate>().checkNail(holeObjects[i].transform.position)){
					Debug.Log("hố trong " + obj.name + " đã trùng với " + holeObjects[i].name);
					if (holeObjects[i].GetComponent<Hole>().CheckNail()){
						Debug.Log("đã phát hiện ra nail ở " + holeObjects[i].GetComponent<Hole>().name);
						obj.GetComponent<IronPlate>().holes[obj.GetComponent<IronPlate>().selectedhole].GetComponent<HingeJoint2D>().connectedBody = holeObjects[i].GetComponent<Hole>().getNail().GetComponent<Rigidbody2D>();
						Debug.Log("đã gắn rigit của " + holeObjects[i].GetComponent<Hole>().getNail().name + "vào rigit của hố " + obj.GetComponent<IronPlate>().holes[obj.GetComponent<IronPlate>().selectedhole]);
					}
				}
			}
		}
	}
	private void selectHole()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(cubeRay, 0.1f, Vector3.forward, iNSelectionLayer);
			RaycastHit2D[] HitIron = Physics2D.CircleCastAll(cubeRay, 0.1f, Vector3.forward, IronLayer);
				if (cubeHit.Length > 0){
				foreach (var cube in cubeHit)
				{
					Debug.Log(cube.collider.name);
				}
				foreach (RaycastHit2D ray in cubeHit)
				{
					if (ray.collider.tag == "Hole")
					{
						Debug.Log("sau khi lọc : " + ray.collider.name);
						selectedHole = ray.collider.gameObject;
						if (ray.collider.GetComponent<Hole>().CheckNail() == true)
						{
							Debug.Log("We select " + ray.collider.name);
							if (selectedNail != null)
							{
								selectedNail.GetComponent<SpriteRenderer>().color = Color.red;
							}
							selectedNail = ray.collider.GetComponent<Hole>().getNail();
							if(preHole == null)
							{
								preHole = selectedHole;
							}
							if(selectedNail != null)
							{
								preHole = selectedHole;
							}
							selectedNail.GetComponent<SpriteRenderer>().color = Color.green;
						}
					}

				}
				if (selectedNail != null)
				{
					if (HitIron.Length > 0)
					{
						foreach (RaycastHit2D ray in HitIron)
						{
							if (ray.collider.tag == "Iron")
							{
								selectedIron = ray.collider.gameObject;
								Debug.Log(selectedIron.name);
								for (int i = 0; i < selectedIron.GetComponent<IronPlate>().holes.Length; i++)
								{
									var hole = selectedIron.GetComponent<IronPlate>().hingeJoint2Ds[i];
									if (hole.connectedBody == selectedNail.GetComponent<Rigidbody2D>())
									{
										preHingeJoint2D.Add(hole);
									}
								}
										foreach(var preHinge in preHingeJoint2D)
										Debug.Log(preHinge.name);
							}
						}
					}
					placeNail();
				}
				
			}
		}
	}
	private void placeNail()
	{
		bool hasHole = false;
		bool hasIron = false;
		int selectedIronPlate = 0;
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] Hit = Physics2D.CircleCastAll(Ray, 0.15f, Vector3.forward, placeLayer);

			if (Hit.Length > 0)
			{
				for (int i = 0; i < Hit.Length; i++)
				{
					if (Hit[i].collider.tag == "Hole")
					{
						hasHole = true;
					}
					if (Hit[i].collider.tag == "Iron")
					{
						hasIron = true;
						selectedIronPlate = i;
					}
				}
				if (hasIron)
				{
					if (hasHole)
					{
						Debug.Log("có cả 2 đấy");
						if (Hit[selectedIronPlate].collider.GetComponent<IronPlate>().checkHitPoint(selectedHole.transform.position))
						{

							if (createNailInIron())
							{
								NailManager.instance.DestroyNail(selectedNail);
								selectedNail = null;
							}

						}
					}
				}
				else
				{
					if (hasHole)
					{

						if (createNail())
						{
							NailManager.instance.DestroyNail(selectedNail);
							selectedNail = null;
						}
					}
				}
			}
		}
	}
	public bool createNailInIron()
	{
		if (selectedHole.GetComponent<Hole>().CheckNail() == false)
		{
			if (preHingeJoint2D != null)
			{
				foreach (var hinge in preHingeJoint2D)
				{
					if (hinge != null)
					{
						hinge.connectedBody = null;
						hinge.enabled = false;
						hinge.connectedAnchor = Vector2.zero;
					}
				}
						preHingeJoint2D.Clear();
			}
			GameObject newNail = NailManager.instance.PoolNail(selectedHole.transform.position);
			selectedNail.GetComponent<SpriteRenderer>().color = Color.red;
			newNail.GetComponent<CircleCollider2D>().isTrigger = true;
			selectedHole.GetComponent<Hole>().setNail(selectedNail);
			preHole.GetComponent<Hole>().setNail(null);
			
			return true;
		}
		return false;
	}
	public bool createNail()
	{
		if (selectedHole.GetComponent<Hole>().CheckNail() == false)
		{
			if(preHingeJoint2D != null) {
				foreach (var preHinge in preHingeJoint2D)
				{
					if (preHinge != null)
					{
						preHinge.connectedBody = null;
						preHinge.enabled = false;
						preHinge.connectedAnchor = Vector2.zero;
					}
				}
						preHingeJoint2D.Clear();
			}
			GameObject newNail = NailManager.instance.PoolNail(selectedHole.transform.position);
			selectedNail.GetComponent<SpriteRenderer>().color = Color.red;
			newNail.GetComponent<CircleCollider2D>().isTrigger = false;
			selectedHole.GetComponent<Hole>().setNail(selectedNail);
			preHole.GetComponent<Hole>().setNail(null);
			return true;
		}
		return false;
	}
	public void setHinge()
	{
		//checkNails();
		foreach (var iron in ironObjects)
		{
			for (int i = 0; i < iron.GetComponent<IronPlate>().holes.Length; i++)
			{
				iron.GetComponent<IronPlate>().holes[i].GetComponent<NailDetector>().setHinge(iron.GetComponent<IronPlate>().hingeJoint2Ds[i]);
			}
			for(int i = 0; i < iron.GetComponent<IronPlate>().hingeJoint2Ds.Length; i++)
			{
				Debug.Log(iron.GetComponent<IronPlate>().holes[i].transform.localPosition.x + " +" + iron.GetComponent<IronPlate>().holes[i].transform.localPosition.y);
				iron.GetComponent<IronPlate>().hingeJoint2Ds[i].anchor = new Vector2(iron.GetComponent<IronPlate>().holes[i].transform.localPosition.x, iron.GetComponent<IronPlate>().holes[i].transform.localPosition.y);
				iron.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedAnchor = Vector2.zero;
			}
		}
	}
	public void setResource()
	{
		holeObjects = GameObject.FindGameObjectsWithTag("Hole");
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
	}
	private void OnDrawGizmos()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(mousePosition, 0.1f);
	}
}
