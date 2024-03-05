using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Unity.Mathematics;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;
	//khai bao tai nguyen
	[SerializeField] private GameObject nailPrefab;
	[SerializeField] private GameObject selectNailPrefab;
	[SerializeField] private GameObject selectNailPrefabAnimation;
	[SerializeField] private Sprite nailDefaulSprite;
	// lay nail truoc va dang chon
	public bool canSelect;
	[SerializeField] private GameObject selectedNail;
	[SerializeField] private GameObject preSelectedNail;
	public GameObject newNail;
	public GameObject preNail;
	// lay ho va ho truoc
	public GameObject selectedHole;
	[SerializeField] private GameObject preHole;
	//lay iron
	public GameObject selectedIron;
	//hingejoint
	public List<HingeJoint2D> preHingeJoint2D = new List<HingeJoint2D>();
	public List<HingeJoint2D> preHingeJoint2DToDetele = new List<HingeJoint2D>();
	public List<HingeJoint2D> HingeJointBeforeRemove = new List<HingeJoint2D>();
	// cac layer mask
	private LayerMask iNSelectionLayer;
	private LayerMask IronLayer;
	private LayerMask unSelectLayer;
	private LayerMask placeLayer;
	//lay tai nguyen
	public GameObject[] ironObjects;
	public GameObject[] holeObjects;
	//vi tri chuot
	Vector2 Ray;

	//
	public NailManager nailManager;
	//lưu tài nguyên sau từng chuyển động
	public bool hasSave;

	public bool hasDelete;
	GameObject nailToDetele;
	GameObject holeToDetele;
	[SerializeField] private GameObject holeBeforeMove;
	[SerializeField] private GameObject holeBeToReturn;
	public List<GameObject> ironObjectsBeforemove = new List<GameObject>();
	public List<Vector3> ironObjectsTransformBeforemove = new List<Vector3>();
	public List<Quaternion> ironObjectsRotationBeforemove = new List<Quaternion>();
	public List<GameObject> nailObjectsBeforemove = new List<GameObject>();
	public List<GameObject> nailsJointBeforemove = new List<GameObject>();
	public List<Vector3> nailObjectsTransformBeforemove = new List<Vector3>();

	public int numOfIronPlate;

	private void Awake()
	{
		iNSelectionLayer = LayerMask.GetMask("Hole");
		placeLayer = LayerMask.GetMask("Hole", "IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
		IronLayer = LayerMask.GetMask("IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
		unSelectLayer = LayerMask.GetMask("square", "IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2vs3vs4", "layer1vs2vs3vs4");

	}
	// Start is called before the first frame update

	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		if (instance != this)
		{
			instance = this;
		}
		holeObjects = GameObject.FindGameObjectsWithTag("Hole");
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
		nailManager = FindFirstObjectByType<NailManager>();
		canSelect = true;
	}

	// Update is called once per frame
	void Update()
	{

		Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
		//selectHole();
		//if (Input.GetKeyDown(KeyCode.K))
		//{
		//	Undo();
		//}
		//test
		if (!GameManager.instance.hasUI)
		{
			if (GameManager.instance.deleting != true)
			{
				if (canSelect)
				{
					selectHole();
				}
			}
			else
			{
				try
				{
					Destroy(selectNailPrefabAnimation);
					selectedNail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
				}
				catch (Exception e) { }
				selectDeteleNail();
			}
		}
		else
		{
			if (selectNailPrefabAnimation)
			{
				Destroy(selectNailPrefabAnimation);
				StartCoroutine(SetdefaultSpriteSameHole(selectedNail));
			}
		}
	}

	private void TurnNailsToRed(bool isOn)
	{
		for (int i = 0; i < nailManager.nails.Count; i++)
		{
			Transform transform = nailManager.nails[i].transform;
			transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = isOn;

		}
	}

	private void selectHole()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(Ray, 0.35f, Vector3.forward, Mathf.Infinity);
			//RaycastHit2D[] cubeHit1 = Physics2D.CircleCastAll(Ray, 0.05f, Vector3.forward, Mathf.Infinity, ~iNSelectionLayer);
			//if (cubeHit.Length > 0)
			//{
			//	foreach (RaycastHit2D ray in cubeHit)
			//	{
			//if (ray.collider.tag == "Hole")
			//{
			//List<GameObject> holes = new List<GameObject>();
			//holes.Add(ray.collider.gameObject);
			////chọn hố
			//float [] Distances = new float[holes.Count];
			//for (int i = 0; i < holes.Count; i++)
			//{
			//	Distances[i] = Vector2.Distance(holeObjects[i].transform.position, holes[i].transform.position);
			//}
			//for (int i = 0; i < holes.Count; i++)
			//{
			//	if (Distances[i] == Distances.Min())
			//}
			try
			{
				bool hasHole =false;
				int hole=0;
				for (int i = 0; i < cubeHit.Length; i++)
				{
					if (cubeHit[i].transform.gameObject.tag == "Hole") { 
						hasHole = true;
						hole = i;
					}
				}
				if(hasHole != false)
				{
					selectedHole = cubeHit[hole].transform.gameObject;
				}
				else
				{
					selectedHole = null;
				}
				if (selectNailPrefabAnimation != null)
				{
					selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");

					Destroy(selectNailPrefabAnimation, 0.33f);
					try
					{
						StartCoroutine(SetdefaultSpriteSameHole(selectedNail));
					}
					catch (Exception e) { }
				}
			}
			catch { }
			if (selectedHole != null)
			{
				if (selectedHole.GetComponent<Hole>().CheckNail() == true)
				{
					if (selectedNail != null)
					{

						if (preSelectedNail != selectedNail)
						{
							preSelectedNail = selectedNail;
						}
					}
					//khi click vào nail khác thì nail cũ chạy animation Deselect
					if (selectNailPrefabAnimation != null)
					{
						//selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isSelected", false);
						//selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isNotSelected", true);
						selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");

						Destroy(selectNailPrefabAnimation, 0.33f);
						try
						{
							StartCoroutine(SetdefaultSpriteSameHole(selectedNail));
						}
						catch (Exception e) { }
						if (selectedNail != null && selectedNail != selectedHole.GetComponent<Hole>().getNail())
						{
							//lấy nail
							selectedNail = selectedHole.GetComponent<Hole>().getNail();
							// animation select nail
							selectNailPrefabAnimation = Instantiate(selectNailPrefab, new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity, transform);
							////highLight nail đang được chọn
							//selectNailPrefabAnimation.GetComponentInChildren<SpriteRenderer>().color = Color.green;
							// tắt sprite của nail tại vị trí đang lấy
							selectedNail.GetComponent<SpriteRenderer>().sprite = null;
							// chạy animation lấy nail
							selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isNotSelected", false);
							selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isSelected", true);
						}
					}
					//khi click vào nail khác thì nail mới chạy animation select
					else
					{
						//lấy nail
						selectedNail = selectedHole.GetComponent<Hole>().getNail();
						// animation select nail
						
						selectNailPrefabAnimation = Instantiate(selectNailPrefab, new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity, transform);
						if (selectedNail.transform.localScale == new Vector3(.65f, 0.65f, 1f))
						{
							selectNailPrefabAnimation.transform.GetChild(0).localScale = new Vector3(.7f, .7f, 1f);
						}
						if (selectedNail.transform.localScale == new Vector3(.8f, 0.8f, 1f))
						{
							selectNailPrefabAnimation.transform.GetChild(0).localScale = new Vector3(0.85f, 0.85f, 1f);
						}
						//highLight nail đang được chọn
						//selectNailPrefabAnimation.GetComponentInChildren<SpriteRenderer>().color = Color.green;
						// tắt sprite của nail tại vị trí đang lấy
						selectedNail.GetComponent<SpriteRenderer>().sprite = null;
						// chạy animation lấy nail
						selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isNotSelected", false);
						selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isSelected", true);
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
					if (preSelectedNail != selectedNail)
					{
						preHingeJoint2D.Clear();
					}

				}
			}
			//		}
			//	}
			//}
			if (selectedNail != null && selectNailPrefabAnimation)
			{
				SetPreHinge(selectedNail, preHingeJoint2D);
				placeNail();
			}
		}
	}

	private void SetPreHinge(GameObject nail, List<HingeJoint2D> preHingJoint)
	{
		RaycastHit2D[] HitIron = Physics2D.CircleCastAll(Ray, 0.1f, Vector3.forward, IronLayer);
		if (HitIron.Length > 0)
		{
			foreach (RaycastHit2D ray in HitIron)
			{
				if (ray.collider.tag == "Iron")
				{
					selectedIron = ray.collider.gameObject;
					for (int i = 0; i < ray.collider.GetComponent<IronPlate>().holes.Length; i++)
					{
						var hole = ray.collider.GetComponent<IronPlate>().hingeJoint2Ds[i];
						if (hole.connectedBody == nail.GetComponent<Rigidbody2D>())
						{
							if (!preHingJoint.Contains(hole))
							{
								preHingJoint.Add(hole);
							}
						}
					}
				}
			}
		}
	}
	public bool checkAllin()
	{
		bool allin = false;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), 0.3f);

		foreach (Collider2D collider in colliders)
		{
			if (collider.transform.tag == "Iron")
			{
				if (collider.GetComponent<IronPlate>().hingeJoint2Ds.Length > 0)
				{
					if (!collider.GetComponent<IronPlate>().checkHitPoint(selectedHole.transform.position))
					{
						allin = false;
						return false;
					}
					else
					{
						allin = true;
					}
				}
			}
		}
		return allin;
	}
	private void placeNail()
	{
		bool hasIron = false;
		bool hasHole = false;

		if (Input.GetMouseButtonDown(0))
		{
			Vector2 Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			if (selectedHole != null)
			{
				Collider2D[] HitHole = Physics2D.OverlapCircleAll(new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), 0.1f);
				if (HitHole.Length > 0)
				{
					foreach (Collider2D collider in HitHole)
					{
						if (collider.transform.tag == "Hole")
						{
							hasHole = true;
						}
					}
				}
				if (selectedHole.GetComponent<Hole>().collider == true)
				{
					hasIron = true;
				}
				if (hasIron)
				{
					if (selectedHole.GetComponent<Hole>().collider == true)
					{
						if (checkAllin())
							if (createNailInIron())
							{
								nailManager.DestroyNail(selectedNail);
								selectedNail = null;
								selectedHole = null;
							}
					}
				}
				if (!hasIron && hasHole)
				{
					if (createNail())
					{
						nailManager.DestroyNail(selectedNail);
						selectedNail = null;
						selectedHole = null;
					}
				}
			}
			//}
		}
	}
	public bool createNailInIron()
	{
		if (selectedHole.GetComponent<Hole>().CheckNail() == false)
		{
			holeBeToReturn = selectedHole;
			holeBeforeMove = preHole;
			ClearData();
			SaveGameObject();
			hasDelete = false;
			//tạo nail tại vị trí mới
			StartCoroutine(SpawnNailInIron());
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

			// tạo animation deselect tại vị trí mới
			selectNailPrefabAnimation.transform.position = new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y + 0.1f);
			selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
			//huy animation deselect;
			Destroy(selectNailPrefabAnimation, 0.43f);
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
			holeBeToReturn = selectedHole;
			holeBeforeMove = preHole;
			ClearData();
			SaveGameObject();
			hasDelete = false;
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
			Destroy(selectNailPrefabAnimation, 0.3f);
			return true;
		}
		return false;
	}
	IEnumerator SpawnNail()
	{
		preNail = selectedNail;
		newNail = nailManager.PoolNail(selectedHole.transform.position);
		newNail.transform.localScale = preHole.GetComponent<Hole>().getNail().transform.localScale;
		preHole.GetComponent<Hole>().setNail(null);
		selectedHole.GetComponent<Hole>().setNail(selectedNail);
		if (newNail != selectedNail)
			StartCoroutine(SetdefaultSprite(newNail));
		yield return newNail;
	}
	IEnumerator SpawnNailInIron()
	{
		preNail = selectedNail;
		newNail = nailManager.PoolNail(selectedHole.transform.position);
		newNail.transform.localScale = preHole.GetComponent<Hole>().getNail().transform.localScale;
		newNail.GetComponent<Collider2D>().isTrigger = true;
		selectedIron.GetComponent<Collider2D>().isTrigger = true;
		selectedIron.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		selectedIron.GetComponent<Rigidbody2D>().angularVelocity = Vector3.zero.magnitude;
		selectedIron.GetComponent<Rigidbody2D>().freezeRotation = true;
		preHole.GetComponent<Hole>().setNail(null);
		selectedHole.GetComponent<Hole>().setNail(selectedNail);
		yield return new WaitForSeconds(0.1f);
		selectedIron.GetComponent<Collider2D>().isTrigger = false;
		selectedIron.GetComponent<Rigidbody2D>().freezeRotation = false;
		newNail.GetComponent<Collider2D>().isTrigger = false;
		if (newNail != selectedNail)
			StartCoroutine(SetdefaultSpriteInIron(newNail));
		yield return newNail;
	}
	public void selectDeteleNail()
	{
		TurnNailsToRed(true);
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
						if (ray.collider.GetComponent<Hole>().CheckNail() == true)
						{
							holeToDetele = ray.collider.gameObject;
							ClearData();
							SaveGameObject();
							nailToDetele = holeToDetele.GetComponent<Hole>().getNail();
							SetPreHinge(nailToDetele, preHingeJoint2DToDetele);
							holeToDetele.GetComponent<Hole>().setNail(null);
							if (preHingeJoint2DToDetele != null)
							{
								foreach (var hinge in preHingeJoint2DToDetele)
								{
									if (hinge != null)
									{
										hinge.connectedBody = null;
										hinge.enabled = false;
										hinge.connectedAnchor = Vector2.zero;
									}
								}
								preHingeJoint2DToDetele.Clear();
							}
							nailManager.DestroyNail(nailToDetele);
							GameManager.instance.deleting = false;
							UIManager.instance.gamePlayPanel.ButtonOn();
							hasDelete = true;
							TurnNailsToRed(false);
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
	public void SaveGameObject()
	{

		//code cu
		{
			//code đoạn này ngu vl
			foreach (var iron in ironObjects)
			{
				ironObjectsBeforemove.Add(iron);
			}
			for (int i = 0; i < ironObjectsBeforemove.Count; i++)
			{
				ironObjectsTransformBeforemove.Add(ironObjectsBeforemove[i].transform.localPosition);
				ironObjectsRotationBeforemove.Add(ironObjectsBeforemove[i].transform.rotation);

			}
			for (int i = 0; i < nailManager.nails.Count; i++)
			{
				nailObjectsBeforemove.Add(nailManager.nails[i]);

			}
			for (int i = 0; i < nailObjectsBeforemove.Count; i++)
			{
				nailObjectsTransformBeforemove.Add(nailObjectsBeforemove[i].transform.localPosition);
			}
			foreach (var iron in ironObjects)
			{
				foreach (var hinge in iron.GetComponent<IronPlate>().hingeJoint2Ds)
				{
					if (hinge.connectedBody != null)
					{
						HingeJointBeforeRemove.Add(hinge);
						nailsJointBeforemove.Add(hinge.connectedBody.gameObject);
					}
				}
			}

		}
		hasSave = true;
	}
	public void Undo()
	{
		if (hasSave)
		{
			//code cu
			{

				try
				{
					Destroy(selectNailPrefabAnimation);
					StartCoroutine(SetdefaultSprite(selectedNail));
				}
				catch (Exception e) { }
				for (int i = 0; i < ironObjectsBeforemove.Count; i++)
				{
					ironObjectsBeforemove[i].transform.SetLocalPositionAndRotation(ironObjectsTransformBeforemove[i], ironObjectsRotationBeforemove[i]);
					ironObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().isKinematic = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().freezeRotation = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
					ironObjectsBeforemove[i].SetActive(true);
				}
				for (int i = 0; i < HingeJointBeforeRemove.Count; i++)
				{
					HingeJointBeforeRemove[i].connectedBody = nailsJointBeforemove[i].GetComponent<Rigidbody2D>();
					HingeJointBeforeRemove[i].connectedAnchor = Vector2.zero;
					HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = true;
					HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = false;
				}
				if (hasDelete == false)
				{
					for (int i = 0; i < nailObjectsBeforemove.Count; i++)
					{
						nailObjectsBeforemove[i].transform.position = nailObjectsBeforemove[i].transform.position;
						nailObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = true;
					}

					try
					{
						foreach (var iron in ironObjects)
						{
							foreach (var hinge in iron.GetComponent<IronPlate>().hingeJoint2Ds)
							{
								if (hinge.connectedBody == newNail.GetComponent<Rigidbody2D>())
								{
									hinge.connectedBody = null;
									hinge.connectedAnchor = Vector2.zero;
									hinge.enabled = false;
								}
							}
						}
						nailManager.DestroyNail(newNail);
						preNail.SetActive(true);
						holeBeforeMove.GetComponent<Hole>().setNail(preNail);
						preNail.GetComponent<Collider2D>().isTrigger = true;
						preNail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
						holeBeToReturn.GetComponent<Hole>().setNail(null);
						holeBeforeMove = null;
					}
					catch (Exception e) { }
				}
				else
				{
					try
					{
						nailToDetele.SetActive(true);
						holeToDetele.GetComponent<Hole>().setNail(nailToDetele);

					}
					catch { }
				}
				hasDelete = false;
				Continute();
			}
		}
	}
	public void Continute()
	{
		for (int i = 0; i < ironObjectsBeforemove.Count; i++)
		{
			ironObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = false;
			ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().freezeRotation = false;
			ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().isKinematic = false;
			ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		}
		for (int i = 0; i < nailObjectsBeforemove.Count; i++)
		{
			nailObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = false;
		}
		for (int i = 0; i < HingeJointBeforeRemove.Count; i++)
		{
			HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = true;
			HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = false;
			HingeJointBeforeRemove[i].enabled = true;
		}
		ClearData();

	}
	public void ClearData()
	{
		try
		{
			nailObjectsBeforemove.Clear();
			nailObjectsTransformBeforemove.Clear();
			ironObjectsRotationBeforemove.Clear();
			ironObjectsBeforemove.Clear();
			ironObjectsTransformBeforemove.Clear();

			//mới add
			HingeJointBeforeRemove.Clear();
			nailsJointBeforemove.Clear();

			preNail = null;
			newNail = null;
			hasSave = false;
		}
		catch (Exception e) { }
	}
	IEnumerator SetdefaultSprite(GameObject nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.28f);
		canSelect = true;
		try
		{
			nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
		}
		catch { }
	}
	IEnumerator SetdefaultSpriteInIron(GameObject nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.2f);
		canSelect = true;
		try
		{
			nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
		}
		catch { }
	}
	IEnumerator SetdefaultSpriteSameHole(GameObject nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.3f);
		canSelect = true;
		try
		{
			nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
		}
		catch { }
	}
	private void setCanNotSelect()
	{
		canSelect = false;
	}
	private void setCanSelect()
	{
		canSelect = true;
	}
	private void OnDrawGizmos()
	{
		if (this.isActiveAndEnabled)
		{
			Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(mousePosition, 0.5f);
		}
	}

}