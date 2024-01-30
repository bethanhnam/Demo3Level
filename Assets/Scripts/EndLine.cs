using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndLine : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Iron")
		{
			Destroy(collision.gameObject);
			InputManager.instance.numOfIronPlate--;

			if (InputManager.instance.numOfIronPlate <= 0)
			{
				GameManager.instance.CheckLevel();
			}
		}
	}
}
