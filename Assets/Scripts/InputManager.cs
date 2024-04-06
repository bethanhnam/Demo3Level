using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
	[SerializeField] public GameObject selectedNail;
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
	public Transform[] pointerPositions;
	public GameObject clickEffect;
	public GameObject pickUpEffect;
	public GameObject pickUpEffect1;
	public GameObject destroyNailEffect;
	//
	public NailManager nailManager;
	//lưu tài nguyên sau từng chuyển động
	public bool hasSave;
	public bool hasDelete;
	public bool hasBoom;
	public bool outOfMoves = false;
	public bool checkOutOfMoves = false;
	public bool hasShowNotice = false;

	public List<Hole> holeCollider = new List<Hole>();
	public List<IronPlate> ironCollider = new List<IronPlate>();
	public List<IronPlate> ignoreIronCollider = new List<IronPlate>();
	public List<Sprite> ignoreColliderCheckSprite = new List<Sprite>();

	GameObject nailToDetele;
	GameObject holeToDetele;
	public GameObject ironToDeteleNail;
	[SerializeField] private GameObject holeBeforeMove;
	[SerializeField] private GameObject holeBeToReturn;
	public List<GameObject> ironObjectsBeforemove = new List<GameObject>();
	public List<Vector3> ironObjectsTransformBeforemove = new List<Vector3>();
	public List<Quaternion> ironObjectsRotationBeforemove = new List<Quaternion>();
	public List<GameObject> nailObjectsBeforemove = new List<GameObject>();
	public List<GameObject> nailObjectsBeforeBoom = new List<GameObject>();
	public List<GameObject> nailsJointBeforemove = new List<GameObject>();
	public List<Vector3> nailObjectsTransformBeforemove = new List<Vector3>();

	public int numOfIronPlateCircle = 0;
	public int numOfIronPlate;
	public int numOfIronPlateCollider;
	public int minLayer = 0;

	private void Awake()
	{
		iNSelectionLayer = LayerMask.GetMask("Hole");
		placeLayer = LayerMask.GetMask("Hole", "IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2", "layer1vs2vs3", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
		IronLayer = LayerMask.GetMask("IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2", "layer1vs2vs3", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
		unSelectLayer = LayerMask.GetMask("square", "IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2", "layer1vs2vs3", "layer1vs2vs3vs4", "layer1vs2vs3vs4");

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
		numOfIronPlateCollider = numOfIronPlate;
		foreach (var ironObject in ironObjects)
		{
			foreach (var sprite in ignoreColliderCheckSprite)
			{
				if (ironObject.GetComponent<SpriteRenderer>().sprite == sprite)
				{
					if (ironObject.GetComponent<IronPlate>().hingeJoint2Ds.Length <= 1)
					{
						ignoreIronCollider.Add(ironObject.GetComponent<IronPlate>());
						numOfIronPlateCollider--;
					}
				}
			}
		}

	}

	// Update is called once per frame
	void Update()
	{
		Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;

		if (!GameManager.instance.hasUI)
		{
			if (GameManager.instance.deleting != true)
			{
				if (GameManager.instance.deletingIron != true)
				{
					if (canSelect)
					{
						selectHole();
					}
				}
				else
				{
					selectBoomNail();
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
		try
		{
			if (ignoreIronCollider.Count > 0)
			{
				foreach (var iron in ignoreIronCollider)
				{
					if (iron.gameObject.activeSelf == false)
					{
						ignoreIronCollider.Remove(iron);
					}
				}
			}
		}
		catch { }
		if (numOfIronPlate != 1)
		{
			foreach (var hole in holeObjects)
			{
				if (hole.GetComponent<Hole>().isOsccupied == true)
				{
					if (!holeCollider.Contains(hole.GetComponent<Hole>()))
					{
						holeCollider.Add(hole.GetComponent<Hole>());
					}
				}
				else
				{
					holeCollider.Remove(hole.GetComponent<Hole>());
				}
			}


			foreach (var iron in ironObjects)
			{
				if (iron.GetComponent<Rigidbody2D>().velocity == Vector2.zero)
				{
					if (!ironCollider.Contains(iron.GetComponent<IronPlate>()))
					{
						ironCollider.Add(iron.GetComponent<IronPlate>());
					}
				}
				else
				{
					ironCollider.Remove(iron.GetComponent<IronPlate>());
				}
			}
		}
		if (holeCollider.Count == holeObjects.Length)
		{
			if (ironCollider.Count - ignoreIronCollider.Count == numOfIronPlateCollider)
			{
				if (hasShowNotice == false)
				{
					Invoke("ShowNotice", 1f);
				}
			}
		}
		else
		{
			numOfIronPlateCircle = 0;
			hasShowNotice = false;
			UIManager.instance.gamePlayPanel.noticePanel.gameObject.SetActive(false);
		}
	}

	private void ShowNotice()
	{
		if (holeCollider.Count == holeObjects.Length)
		{
			UIManager.instance.gamePlayPanel.noticePanel.gameObject.SetActive(true);
			hasShowNotice = true;
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
			RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(Ray, 0.5f, Vector3.forward, Mathf.Infinity);

			try
			{
				bool hasHole = false;
				int hole = 0;
				for (int i = 0; i < cubeHit.Length; i++)
				{
					if (cubeHit[i].transform.gameObject.tag == "Hole")
					{
						hasHole = true;
						hole = i;
					}
				}
				if (hasHole != false)
				{
					selectedHole = cubeHit[hole].transform.gameObject;
					try
					{
						if (Notice.Instance.gameObject.activeSelf)
						{
							StartCoroutine(DisappearNotice());
						}
					}
					catch
					{

					}
					preHingeJoint2D.Clear();
					AudioManager.instance.PlaySFX("PickUpScrew");
				}
				else
				{
					if (cubeHit[0].transform.gameObject.layer != 5)
					{
						AudioManager.instance.PlaySFX("Click");
					}
					var clickeffect = Instantiate(clickEffect, new Vector2(Ray.x, Ray.y), quaternion.identity);
					Destroy(clickeffect, 0.2f);
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
							var pickUpeffect = Instantiate(pickUpEffect, selectedNail.transform.position, quaternion.identity);
							Destroy(pickUpeffect, 0.2f);
							var pickUpeffect1 = Instantiate(pickUpEffect1, selectedNail.transform.position, quaternion.identity);
							Destroy(pickUpeffect1, 0.4f);
						}
					}
					//khi click vào nail khác thì nail mới chạy animation select
					else
					{
						//lấy nail
						selectedNail = selectedHole.GetComponent<Hole>().getNail();
						// animation select nail

						selectNailPrefab.transform.transform.localScale = new Vector3(selectedNail.transform.localScale.x + 0.1f, selectedNail.transform.localScale.y + 0.1f, 1f);
						selectNailPrefabAnimation = Instantiate(selectNailPrefab, new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity, transform);
						selectNailPrefabAnimation.transform.GetChild(0).GetChild(0).localScale = new Vector3(selectedNail.transform.localScale.x + 0.01f, selectedNail.transform.localScale.y + 0.01f, 1f);
						//highLight nail đang được chọn
						//selectNailPrefabAnimation.GetComponentInChildren<SpriteRenderer>().color = Color.green;
						// tắt sprite của nail tại vị trí đang lấy
						selectedNail.GetComponent<SpriteRenderer>().sprite = null;
						// chạy animation lấy nail
						selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isNotSelected", false);
						selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetBool("isSelected", true);
						var pickUpeffect = Instantiate(pickUpEffect, selectedNail.transform.position, quaternion.identity);
						Destroy(pickUpeffect, 0.2f);
						var pickUpeffect1 = Instantiate(pickUpEffect1, selectedNail.transform.position, quaternion.identity);
						Destroy(pickUpeffect1, 0.4f);
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
			if (selectedNail != null && selectNailPrefabAnimation)
			{

				SetPreHinge(selectedNail, preHingeJoint2D);
				placeNail();
			}
		}
	}

	IEnumerator DisappearNotice()
	{
		yield return new WaitForSeconds(1f);
		Notice.Instance.DisableNotice();
	}

	private void SetPreHinge(GameObject nail, List<HingeJoint2D> preHingJoint)
	{
		RaycastHit2D[] HitIron = Physics2D.CircleCastAll(nail.transform.position, 0.1f, Vector3.forward, IronLayer);
		if (HitIron.Length > 0)
		{
			foreach (RaycastHit2D ray in HitIron)
			{
				if (ray.transform.tag == "Iron")
				{
					selectedIron = ray.transform.gameObject;
					for (int i = 0; i < ray.transform.GetComponent<IronPlate>().holes.Length; i++)
					{
						var hole = ray.transform.GetComponent<IronPlate>().hingeJoint2Ds[i];
						if (hole.connectedBody == nail.transform.GetComponent<Rigidbody2D>())
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
		bool allin = true;
		Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y), 0.17f);
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
			SetPreHinge(selectedNail, preHingeJoint2D);
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
								StartCoroutine(destroyNail());
							}
					}
				}
				if (!hasIron && hasHole)
				{
					if (createNail())
					{
						StartCoroutine(destroyNail());
					}
				}
			}
			//}
		}
	}
	IEnumerator destroyNail()
	{
		yield return new WaitForSeconds(0.01f);
		nailManager.DestroyNail(selectedNail);
		selectedNail = null;
		selectedHole = null;
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
			hasBoom = false;
			//tạo nail tại vị trí mới
			if (preHingeJoint2D != null)
			{
				foreach (var hinge in preHingeJoint2D)
				{
					hinge.connectedBody = null;
					hinge.enabled = false;
					hinge.connectedAnchor = Vector2.zero;
				}
				preHingeJoint2D.Clear();
			}

			// tạo animation deselect tại vị trí mới
			selectNailPrefabAnimation.transform.position = new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y + 0.1f);
			selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
			Destroy(selectNailPrefabAnimation, 0.32f);
			StartCoroutine(SpawnNailInIron());
			//huy animation deselect;

			//try
			//{
			//	Physics2D.IgnoreCollision(newNail.GetComponent<CircleCollider2D>(), selectedIron.GetComponent<Collider2D>());
			//}
			//catch (Exception ex)
			//{
			//	// Nếu có lỗi, in ra thông báo lỗi
			//	Console.WriteLine("An error occurred: " + ex.Message);
			//}
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
			hasBoom = false;
			if (preHingeJoint2D != null)
			{
				foreach (var preHinge in preHingeJoint2D)
				{
					preHinge.connectedBody = null;
					preHinge.enabled = false;
					preHinge.connectedAnchor = Vector2.zero;
				}
				preHingeJoint2D.Clear();
			}
			//tạo nail tại vị trí mới
			// tạo animation deselect tại vị trí mới
			selectNailPrefabAnimation.transform.position = new Vector2(selectedHole.transform.position.x, selectedHole.transform.position.y + 0.1f);
			selectNailPrefabAnimation.GetComponentInChildren<Animator>().SetTrigger("Deselect");
			Destroy(selectNailPrefabAnimation, 0.32f);
			StartCoroutine(SpawnNail());
			//huy animation deselect;

			return true;
		}
		return false;
	}
	IEnumerator SpawnNail()
	{
		preNail = selectedNail;
		newNail = nailManager.PoolNail(selectedHole.transform.position);
		newNail.transform.localScale = preHole.GetComponent<Hole>().getNail().transform.localScale;
		newNail.transform.localScale = new Vector3(selectedHole.transform.localScale.x + 0.01f, selectedHole.transform.localScale.y + 0.01f, 1f);
		preHole.GetComponent<Hole>().setNail(null);
		selectedHole.GetComponent<Hole>().setNail(selectedNail);
		if (newNail != selectedNail)
		{
			StartCoroutine(SetdefaultSprite(newNail));
		}
		yield return newNail;
	}
	IEnumerator SpawnNailInIron()
	{
		preNail = selectedNail;
		newNail = nailManager.PoolNail(selectedHole.transform.position);
		newNail.transform.localScale = preHole.GetComponent<Hole>().getNail().transform.localScale;
		newNail.transform.localScale = new Vector3(selectedHole.transform.localScale.x + 0.01f, selectedHole.transform.localScale.y + 0.01f, 1f);
		newNail.GetComponent<Collider2D>().isTrigger = true;
		//selectedIron.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
		//selectedIron.GetComponent<Rigidbody2D>().angularVelocity = Vector3.zero.magnitude;
		//selectedIron.GetComponent<Rigidbody2D>().freezeRotation = true;
		preHole.GetComponent<Hole>().setNail(null);
		selectedHole.GetComponent<Hole>().setNail(selectedNail);
		yield return new WaitForSeconds(0.1f);
		selectedIron.GetComponent<Collider2D>().isTrigger = false;
		selectedIron.GetComponent<Rigidbody2D>().freezeRotation = false;
		newNail.GetComponent<Collider2D>().isTrigger = false;
		if (newNail != selectedNail)
		{

			StartCoroutine(SetdefaultSpriteInIron(newNail));
		}
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
							var Destroyeffect1 = Instantiate(destroyNailEffect, nailToDetele.transform.position, quaternion.identity);
							Destroy(Destroyeffect1, 0.5f);
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
	public void selectBoomNail()
	{
		//TurnNailsToRed(true);
		if (Input.GetMouseButtonDown(0))
		{
			Collider2D[] cubeHit = Physics2D.OverlapCircleAll(Ray, 0.1f, IronLayer);
			if (cubeHit.Length > 0)
			{
				int minLayerIndex = cubeHit[0].gameObject.layer; // Gán minLayerIndex bằng layer của phần tử đầu tiên
				minLayer = 0;
				for (int j = 1; j < cubeHit.Length; j++)
				{
					int currentLayerIndex = cubeHit[j].gameObject.layer; // Lấy layer của GameObject hiện tại

					if (currentLayerIndex < minLayerIndex)
					{
						minLayer = j;
						minLayerIndex = currentLayerIndex; // Nếu layer của GameObject hiện tại nhỏ hơn minLayerIndex, cập nhật minLayerIndex
					}
				}
				if (cubeHit[minLayer].gameObject.tag == "Iron")
				{
					ironToDeteleNail = cubeHit[minLayer].gameObject;
					ClearData();
					SaveGameObject();
					hasDelete = false;
					hasBoom = true;
					ironToDeteleNail.GetComponent<Rigidbody2D>().isKinematic = true;
					if (ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds.Length > 0)
					{
						for (int i = 0; i < ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds.Length; i++)
						{
							if (ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody != null)
							{
								if (!nailObjectsBeforeBoom.Contains(ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody.gameObject))
								{
									nailObjectsBeforeBoom.Add(ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody.gameObject);
								}
							}
							if (ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody != null)
							{
								boomNailHingJoint(ironToDeteleNail.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody.gameObject);
							}
						}
					}
					//var Destroyeffect1 = Instantiate(destroyNailEffect, nailToDetele.transform.position, quaternion.identity);
					//Destroy(Destroyeffect1, 0.5f);
					GameManager.instance.deletingIron = false;
					UIManager.instance.gamePlayPanel.ButtonOn();
				}
			}
		}
	}
	public void boomNailHingJoint(GameObject nail)
	{
		Collider2D[] cubeHit = Physics2D.OverlapCircleAll(nail.transform.position, 0.1f, IronLayer);
		if (cubeHit.Length > 0)
		{
			foreach (var cube in cubeHit)
			{
				if (cube.gameObject.tag == "Iron")
				{
					for (int i = 0; i < cube.GetComponent<IronPlate>().hingeJoint2Ds.Length; i++)
					{
						if (nail.GetComponent<Rigidbody2D>() == cube.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody)
						{
							//cube.GetComponent<Rigidbody2D>().isKinematic = true;
							cube.GetComponent<IronPlate>().hingeJoint2Ds[i].enabled = false;
							cube.GetComponent<IronPlate>().hingeJoint2Ds[i].connectedBody = null;
							//cube.GetComponent<Rigidbody2D>().isKinematic = false;
						}
					}
				}
			}
		}
		NailManager.instance.DestroyNail(nail);
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
				if (!hasBoom)
				{
					try
					{
						Destroy(selectNailPrefabAnimation);
						StartCoroutine(SetdefaultSprite(selectedNail));
					}

					catch (Exception e) { }
				}
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
						if (hasBoom)
						{
							foreach (var nail in nailObjectsBeforeBoom)
							{
								nail.SetActive(true);
								nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
							}
						}
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
						nailToDetele.GetComponent<Collider2D>().isTrigger = true;
						nailToDetele.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;

					}
					catch { }
				}
				hasDelete = false;
				hasBoom = false;
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
			nailObjectsBeforeBoom.Clear();

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
		yield return new WaitForSeconds(0.3f);
		canSelect = true;
		nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
	}
	IEnumerator SetdefaultNailBoomSprite(List<GameObject> nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.3f);
		canSelect = true;
		foreach (var nailObj in nail)
		{
			nailObj.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
		}
	}
	IEnumerator SetdefaultSpriteInIron(GameObject nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.2f);
		canSelect = true;

		nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
	}
	IEnumerator SetdefaultSpriteSameHole(GameObject nail)
	{
		canSelect = false;
		yield return new WaitForSeconds(0.3f);
		canSelect = true;
		nail.GetComponent<SpriteRenderer>().sprite = nailDefaulSprite;
	}
	private void setCanNotSelect()
	{
		canSelect = false;
	}
	private void setCanSelect()
	{
		canSelect = true;
	}
	//private void OnDrawGizmos()
	//{
	//	if (this.isActiveAndEnabled)
	//	{
	//		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	//		Gizmos.color = Color.green;
	//		Gizmos.DrawWireSphere(mousePosition, 0.5f);
	//	}
	//}

}