using System;
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
	//khai bao tai nguyen
	[SerializeField] private GameObject nailPrefab;
	[SerializeField] private GameObject selectNailPrefab;
	[SerializeField] private GameObject selectNailPrefabAnimation;


	[SerializeField] private Sprite nailDefaulSprite;
	// lay nail truoc va dang chon
	[SerializeField] private GameObject selectedNail;
	[SerializeField] private GameObject preNail;
	GameObject newNail;
	// lay ho va ho truoc
	public GameObject selectedHole;
	[SerializeField] private GameObject preHole;
	//lay iron
	public GameObject selectedIron;
	//huy hingejoint
	public List<HingeJoint2D> preHingeJoint2D = new List<HingeJoint2D>();
	// cac layer mask
	[SerializeField] private LayerMask iNSelectionLayer;
	[SerializeField] private LayerMask IronLayer;
	[SerializeField] private LayerMask placeLayer;
	//lay tai nguyen
	public GameObject[] ironObjects;
	public GameObject[] holeObjects;
	//vi tri chuot
	Vector2 Ray;
	//boon

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

		Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
		if (GameManager.instance.deleting != true)
		{
			selectHole();
		}
		else
		{
			selectDeteleNail();
		}
	}
	//private void checkNails()
	//{
	//	for (int i = 0; i < holeObjects.Length; i++) {
	//		foreach (GameObject obj in ironObjects)
	//		{
	//			if (obj.GetComponent<IronPlate>().checkNail(holeObjects[i].transform.position)){
	//				Debug.Log("hố trong " + obj.name + " đã trùng với " + holeObjects[i].name);
	//				if (holeObjects[i].GetComponent<Hole>().CheckNail()){
	//					Debug.Log("đã phát hiện ra nail ở " + holeObjects[i].GetComponent<Hole>().name);
	//					obj.GetComponent<IronPlate>().holes[obj.GetComponent<IronPlate>().selectedhole].GetComponent<HingeJoint2D>().connectedBody = holeObjects[i].GetComponent<Hole>().getNail().GetComponent<Rigidbody2D>();
	//					Debug.Log("đã gắn rigit của " + holeObjects[i].GetComponent<Hole>().getNail().name + "vào rigit của hố " + obj.GetComponent<IronPlate>().holes[obj.GetComponent<IronPlate>().selectedhole]);
	//				}
	//			}
	//		}
	//	}
	//}
	private void selectHole()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(Ray, 0.2f, Vector3.forward, iNSelectionLayer);
			if (cubeHit.Length > 0)
			{
				foreach (RaycastHit2D ray in cubeHit)
				{
					if (ray.collider.tag == "Hole")
					{
						//chọn hố
						selectedHole = ray.collider.gameObject;
						if (ray.collider.GetComponent<Hole>().CheckNail() == true)
						{
							if (selectedNail != null)
							{

								if (preNail != selectedNail)
									preNail = selectedNail;
							}
							//khi click vào nail khác thì nail cũ chạy animation Deselect
							if (selectNailPrefabAnimation != null)
							{
								selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
								Destroy(selectNailPrefabAnimation, 0.1f);
								preNail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
								if (selectedNail != null && selectedNail != selectedHole.GetComponent<Hole>().getNail())
								{
									//lấy nail
									selectedNail = selectedHole.GetComponent<Hole>().getNail();
									// animation select nail
									selectNailPrefabAnimation = Instantiate(selectNailPrefab, new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity, transform);
									//highLight nail đang được chọn
									selectNailPrefabAnimation.GetComponentInChildren<SpriteRenderer>().color = Color.green;
									// tắt sprite của nail tại vị trí đang lấy
									selectedNail.GetComponent<SpriteRenderer>().sprite = null;
									// chạy animation lấy nail
									selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Select");
								}
							}
							//khi click vào nail khác thì nail mới chạy animation select
							else
							{
								//lấy nail
								selectedNail = selectedHole.GetComponent<Hole>().getNail();
								// animation select nail
								selectNailPrefabAnimation = Instantiate(selectNailPrefab, new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity, transform);
								//highLight nail đang được chọn
								selectNailPrefabAnimation.GetComponentInChildren<SpriteRenderer>().color = Color.green;
								// tắt sprite của nail tại vị trí đang lấy
								selectedNail.GetComponent<SpriteRenderer>().sprite = null;
								// chạy animation lấy nail
								selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Select");
							}

							if (preHole == null)
							{
								if (selectedHole != preHole)
									preHole = selectedHole;
							}
							if (selectedNail != null)
							{
								preHole = selectedHole;
							}
							if (preNail != selectedNail)
							{

								preHingeJoint2D.Clear();
							}

						}
					}
				}
			}
			if (selectedNail != null && selectNailPrefabAnimation)
			{
				SetPreHinge();
				placeNail();
			}
		}
	}

	private void SetPreHinge()
	{
		RaycastHit2D[] HitIron = Physics2D.CircleCastAll(Ray, 0.1f, Vector3.forward, IronLayer);
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
							if (!preHingeJoint2D.Contains(hole))
							{
								preHingeJoint2D.Add(hole);
							}
						}
					}
				}
			}
		}
	}
	private void placeNail()
	{
		bool hasIron = false;
		int Hole = 0;
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] Hit = Physics2D.CircleCastAll(Ray, 0.2f, Vector3.forward, placeLayer);

			if (Hit.Length > 0)
			{
				for (int i = 0; i < Hit.Length; i++)
				{
					if (Hit[i].collider.tag == "Hole")
					{
						Hole = i;
					}
				}
				Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), 0.15f);
				foreach (Collider2D collider in colliders)
				{
					if (collider.transform.tag == "Iron")
					{
						hasIron = true;
						if (collider.GetComponent<IronPlate>().checkHitPoint(selectedHole.transform.position))
						{
							if (createNailInIron())
							{
								NailManager.instance.DestroyNail(selectedNail);
								selectedNail = null;

							}

						}
					}
				}
				if (!hasIron)
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
			//tạo nail tại vị trí mới
			StartCoroutine(SpawnNail());
			// tạo animation deselect tại vị trí mới
			selectNailPrefabAnimation.transform.position = new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y + 0.1f);
			selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
			//huy animation deselect;
			Destroy(selectNailPrefabAnimation, 0.2f);
			try
			{
				Physics2D.IgnoreCollision(newNail.GetComponent<CircleCollider2D>(), selectedIron.GetComponent<BoxCollider2D>());
			}
			catch (Exception ex)
			{
				// Nếu có lỗi, in ra thông báo lỗi
				Console.WriteLine("An error occurred: " + ex.Message);
			}
			return true;
		}
		return false;
	}
	public bool createNail()
	{
		if (selectedHole.GetComponent<Hole>().CheckNail() == false)
		{
			if (preHingeJoint2D != null)
			{
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
			//tạo nail tại vị trí mới
			StartCoroutine(SpawnNail());
			// tạo animation deselect tại vị trí mới
			selectNailPrefabAnimation.transform.position = new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y + 0.1f);
			selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
			//huy animation deselect;
			Destroy(selectNailPrefabAnimation, 0.2f);
			return true;
		}
		return false;
	}
	IEnumerator SpawnNail()
	{
		newNail = NailManager.instance.PoolNail(selectedHole.transform.position);
		preHole.GetComponent<Hole>().setNail(null);
		selectedHole.GetComponent<Hole>().setNail(selectedNail);
		yield return new WaitForSeconds(0.25f);
		if (newNail != selectedNail)
			newNail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
		yield return newNail;
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
			for (int i = 0; i < iron.GetComponent<IronPlate>().hingeJoint2Ds.Length; i++)
			{
				iron.GetComponent<IronPlate>().hingeJoint2Ds[i].anchor = new Vector2(iron.GetComponent<IronPlate>().holes[i].transform.localPosition.x, iron.GetComponent<IronPlate>().holes[i].transform.localPosition.y);
				iron.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedAnchor = Vector2.zero;
			}
		}
	}
	public void selectDeteleNail()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(Ray, 0.2f, Vector3.forward, iNSelectionLayer);
			if (cubeHit.Length > 0)
			{
				foreach (RaycastHit2D ray in cubeHit)
				{
					if (ray.collider.tag == "Hole")
					{
						//chọn hố
						selectedHole = ray.collider.gameObject;
						if (ray.collider.GetComponent<Hole>().CheckNail() == true)
						{
							selectedNail = selectedHole.GetComponent<Hole>().getNail();
							NailManager.instance.DestroyNail(selectedNail);
							selectedHole.GetComponent<Hole>().setNail(null);
							GameManager.instance.deleting = false;
						}
					}
				}
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
		Gizmos.DrawWireSphere(mousePosition, 0.2f);
	}
}
