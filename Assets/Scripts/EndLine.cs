using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndLine : MonoBehaviour
{
	// Start is called before the first frame update
	public bool ignoreIronCollider = false;
	public GameObject partical;
	public bool hasnext =false;
	void Start()
	{
		partical = Resources.Load<GameObject>("UseablePartical/paperFireWork 1");
	}

	// Update is called once per frame
	void Update()
	{
		if (InputManager.instance.numOfIronPlate <= 0)
		{
			if(hasnext == false)
			StartCoroutine(loadNextLevel());
		}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Iron")
		{
			//Destroy(collision.gameObject);
			collision.gameObject.SetActive(false);
			InputManager.instance.numOfIronPlate--;
			var partical1 =Instantiate(partical, collision.transform.position, Quaternion.identity);
			Destroy(partical1, 1f);
			for (int i = 0;i< InputManager.instance.ignoreIronCollider.Count;i++)
			{
					if (collision.GetComponent<IronPlate>() == InputManager.instance.ignoreIronCollider[i])
					{
						ignoreIronCollider =true;
						goto ignoreCollider;
					}
			}
			if (ignoreIronCollider == false)
			{
				InputManager.instance.numOfIronPlateCollider--;
			}
		}
		ignoreCollider:
		ignoreIronCollider = false;
		AudioManager.instance.PlaySFX("DropIron");

		
	}
	IEnumerator loadNextLevel()
	{
		yield return new WaitForSeconds(1f);
		Level.instance.CheckLevel();
		hasnext = true;

	}
}
