using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using DG.Tweening;

public class EndLine : MonoBehaviour
{
	// Start is called before the first frame update
	public bool ignoreIronCollider = false;
	public GameObject partical;
	public bool hasnext = false;
	public bool hasfade = false;
	void Start()
	{
		partical = Resources.Load<GameObject>("UseablePartical/paperFireWork 1");
	}

	// Update is called once per frame
	void Update()
	{
		//if (InputManager.instance.numOfIronPlate <= 0)
		//{
			//	GameManager.instance.hasDone = true;
			//	if (hasfade == false)
			//	{
			//		var extraHole = Level.instance.transform.GetChild(0).GetChild(0).GetComponent<Stage>().holeToUnlock;
			//		var square = Level.instance.transform.GetChild(0).GetChild(0).GetComponent<Stage>().Square;
			//		var color1 = new Color(0, 0, 0, 0);
			//		square.GetComponent<SpriteRenderer>().DOColor(color1, .5f);
			//		for (int i = 0; i < square.transform.childCount - 1; i++)
			//		{
			//			var hole = square.transform.GetChild(i);
			//			SpriteRenderer sprite = hole.GetComponent<SpriteRenderer>();
			//			sprite.DOColor(color1, .5f);
			//		}
			//		extraHole.transform.GetChild(0).transform.gameObject.SetActive(false);
			//		for(int i = 0;i< NailManager.instance.nails.Count; i++)
			//		{
			//			NailManager.instance.nails[i].GetComponent<SpriteRenderer>().DOColor(color1, .5f);
			//		}
			//		Color color = new Color(1, 1, 1, 1);
			//		Level.instance.transform.GetChild(0).GetChild(0).GetComponent<Stage>().item.GetComponent<SpriteRenderer>().DOColor(color, 0.5f);
			//		hasfade = true;
			//	}
			//	if (hasnext == false)
		//	StartCoroutine(loadNextLevel());
		//}
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		DisableIronPlate(collision);

	}

	private void DisableIronPlate(Collider2D collision)
	{
		if (collision.gameObject.tag == "Iron")
		{
			collision.gameObject.SetActive(false);
			//InputManager.instance.numOfIronPlate--;
			Stage.Instance.numOfIronPlates--;
            //AudioManager.instance.PlaySFX("DropIron");
            Debug.Log("numOfIronPlates" + Stage.Instance.numOfIronPlates);
            Stage.Instance.CheckDoneLevel();
			var partical1 = Instantiate(partical, collision.transform.position, Quaternion.identity);
			Destroy(partical1, 1f);
			//for (int i = 0; i < InputManager.instance.ignoreIronCollider.Count; i++)
			//{
			//	if (collision.GetComponent<IronPlate>() == InputManager.instance.ignoreIronCollider[i])
			//	{
			//		ignoreIronCollider = true;
			//		goto ignoreCollider;
			//	}
			//}
			//if (ignoreIronCollider == false)
			//{
			//	InputManager.instance.numOfIronPlateCollider--;
			//}
		}
		//ignoreCollider:
		//ignoreIronCollider = false;
	}

	IEnumerator loadNextLevel()
	{
		yield return new WaitForSeconds(.5f);
		//Level.instance.CheckLevel();
		hasnext = true;

	}
}
