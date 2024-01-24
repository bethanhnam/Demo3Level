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
	[SerializeField] private LayerMask ironLayer;
	[SerializeField] private GameObject[] ironObjects;
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
		selectNail();
		if (selectedNail != null)
		{
			placeNail();
		}
	}
	private void selectNail()
	{

		if (Input.GetMouseButtonDown(0))
		{
			Vector2 cubeRay = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D cubeHit = Physics2D.Raycast(cubeRay, Vector2.zero, 100f, ~ironLayer);

			if (cubeHit)
			{
				Debug.Log("We hit " + cubeHit.collider.name);
				if (cubeHit.collider.gameObject.tag == "Nail")
				{
					selectedNail = cubeHit.collider.gameObject;
				}
			}

		}
	}
	private void placeNail()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Vector2 Ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D Hit = Physics2D.Raycast(Ray, Vector2.zero, 100f);

			if (Hit)
			{
				Debug.Log("We hit " + Hit.collider.name);
				if (Hit.collider.gameObject.tag == "Hole" && selectedNail != null)
				{
					selectedNail.SetActive(false);
					GameObject NewNail = Instantiate(nailPrefab, new Vector3(Hit.collider.gameObject.transform.position.x, Hit.collider.gameObject.transform.position.y), quaternion.identity);
					NewNail.SetActive(true);
					Destroy(selectedNail );
					selectedNail = null;
				}
			}
		}
	}
	private void OnDrawGizmos()
	{
		float circumference = 0.1f;
		Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Gizmos.color = Color.green;
		Gizmos.DrawWireSphere(mousePosition, 0.1f);
	}

}
