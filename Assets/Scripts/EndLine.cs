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
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		DisableIronPlate(collision);

	}

	private void DisableIronPlate(Collider2D collision)
	{
		if (GameManagerNew.Instance.isMinigame)
		{
            if (collision.gameObject.tag == "Iron")
            {
                collision.gameObject.SetActive(false);
                MiniGameStage.Instance.numOfIronPlates -= 1;
                //AudioManager.instance.PlaySFX("DropIron");
                Debug.Log("numOfIronPlates" + MiniGameStage.Instance.numOfIronPlates);
                MiniGameStage.Instance.CheckDoneLevel();
                UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].gameObject.gameObject.SetActive(true);
                UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].gameObject.transform.position = collision.transform.position;
				UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].transform.DOMoveY(UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].transform.position.y + 18, 0.5f).OnComplete(() =>
				{
                    UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].transform.DOMoveY(UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].transform.position.y - 5, 0.5f).OnComplete(() => { 

                    UIManagerNew.Instance.MiniGamePlay.miniItem[MiniGameStage.Instance.numOfIronPlates].MoveItem();
                    });

                });
            }
        }
		else
		{
			if (collision.gameObject.tag == "Iron")
			{
				collision.gameObject.SetActive(false);
				Stage.Instance.numOfIronPlates -= 1;
				//AudioManager.instance.PlaySFX("DropIron");
				Debug.Log("numOfIronPlates" + Stage.Instance.numOfIronPlates);
				Stage.Instance.CheckDoneLevel();
				var partical1 = Instantiate(partical, collision.transform.position, Quaternion.identity);
				Destroy(partical1, 1f);
			}
		}
	}

	IEnumerator loadNextLevel()
	{
		yield return new WaitForSeconds(.5f);
		//Level.instance.CheckLevel();
		hasnext = true;

	}
}
