using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	public static InputManager instance;
	[SerializeField] private GameObject nailPrefab;
	[SerializeField] private GameObject selectedNail;
	[SerializeField] private GameObject selectedHole;
	[SerializeField] private GameObject preHole;
	[SerializeField] private LayerMask iNSelectionLayer;
	[SerializeField] private LayerMask holeLayer;
	[SerializeField] private GameObject[] ironObjects;

	//public RaycastHit2D[] cubeHit;

	public int numOfIronPlate;

	// Start is called before the first frame update
	void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		ironObjects = GameObject.FindGameObjectsWithTag("Iron");
		numOfIronPlate = ironObjects.Length;
	}

	// Update is called once per frame
	void Update()
	{
		if (ironObjects.Length <= 0)
		{
			Debug.Log("finish");
		}
		else
		{
			selectNail();

		}
	}
	private void selectNail()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] cubeHit = Physics2D.RaycastAll(cubeRay, Vector3.forward, ~iNSelectionLayer);
			if (cubeHit.Length > 0)
			{
				foreach (RaycastHit2D ray in cubeHit)
				{
					if (ray.collider.tag == "Hole")
					{
						selectedHole = ray.collider.gameObject;
						if (ray.collider.GetComponent<Hole>().CheckNail() == true)
						{
							Debug.Log("We select " + ray.collider.name);
							if (selectedNail != null)
							{
								selectedNail.GetComponent<SpriteRenderer>().color = Color.red;
							}
							selectedNail = ray.collider.GetComponent<Hole>().getNail();
							if (preHole == null)
							{
								preHole = selectedHole;
							}
							selectedNail.GetComponent<SpriteRenderer>().color = Color.green;
						}
					}

				}
				if (selectedNail != null)
				{
					placeNail();
				}
			}
		}
	}
	private void placeNail()
	{
		bool hasHole = false;
		bool hasIron = false;
		int selectedIronPlate =0;
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D[] Hit = Physics2D.RaycastAll(Ray, Vector3.forward, holeLayer);

			if (Hit.Length > 0)
			{
				for(int i = 0;i < Hit.Length;i++)
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
				if(hasIron)
				{
					if (hasHole)
					{
							if (Hit[selectedIronPlate].collider.GetComponent<IronPlate>().checkHitPoint(selectedHole.transform.position))
							{
								createNail();
							}
					}
				}
				else
				{
					if (hasHole)
					{
						createNail();
					}
				}
			}
		}
	}
	public void createNail()
	{
		if (selectedHole.GetComponent<Hole>().CheckNail() == false)
		{
			selectedNail.SetActive(false);
			GameObject NewNail = Instantiate(nailPrefab, new Vector3(selectedHole.transform.position.x, selectedHole.transform.position.y), quaternion.identity);
			NewNail.SetActive(true);
			preHole.GetComponent<Hole>().setNail(null);
			Destroy(selectedNail);
			selectedHole.GetComponent<Hole>().setNail(NewNail);
			preHole = null;
			selectedNail = null;
		}
	}
	private void OnDrawGizmos()
	{
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(mousePosition, 0.1f);
	}
}
