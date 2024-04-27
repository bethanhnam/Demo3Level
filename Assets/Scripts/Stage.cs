using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Stage : MonoBehaviour
{
	public static Stage Instance;
	public GameObject holeToUnlock;
	public SpriteRenderer sprRenderItem;
	public GameObject Square;
	// Start is called before the first frame update

	[SerializeField]
	private Hole[] holes;
	[SerializeField]
	private NailControl[] nailControls;
	[SerializeField]
	private IronPlate[] ironPlates;
	[SerializeField]
	public int numOfIronPlates;
	[SerializeField]
	private NailControl curNail;
	[SerializeField]
	private Hole curHole;
	[SerializeField]
	private Hole preHole;

	//extra 
	public string layerName = "Hole";
	//deteleting
	[SerializeField]
	private Hole holeToDetele;
	[SerializeField]
	NailControl nailToDetele;
	public bool isDeteleting = false;
	public bool hasDelete;

	// hingeJoint
	public List<HingeJoint2D> HingeJointBeforeRemove = new List<HingeJoint2D>();

	//undo 
	[SerializeField] Hole holeToUndo;
	[SerializeField] Hole holeBeforeUndo;
	[SerializeField] NailControl nailToUndo;
	[SerializeField] private GameObject holeBeforeMove;
	[SerializeField] private GameObject holeBeToReturn;
	public bool hasSave;
	public List<IronPlate> ironObjectsBeforemove = new List<IronPlate>();
	public List<Vector3> ironObjectsTransformBeforemove = new List<Vector3>();
	public List<Quaternion> ironObjectsRotationBeforemove = new List<Quaternion>();
	public List<NailControl> nailObjectsBeforemove = new List<NailControl>();
	public List<NailControl> nailObjectsBeforeBoom = new List<NailControl>();
	public List<NailControl> nailsJointBeforemove = new List<NailControl>();
	public List<Vector3> nailObjectsTransformBeforemove = new List<Vector3>();

	private void Start()
	{
		Instance = this;
		ClearData();

	}
	public void Init(int level)
	{
		ChangeSize(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[level].SprItem);
	}
	
	public void ChangeSize(Sprite sprite)
	{
		Vector2 sizeSprite = sprite.rect.size;

		float sWidth = sizeSprite.x / 500f;
		float sHeight = sizeSprite.y / 500f;

		sprRenderItem.transform.localScale = Vector2.one * (1 / Mathf.Max(sWidth, sHeight));
		sprRenderItem.sprite = sprite;
	}

	private void Update()
	{
		numOfIronPlates = GameObject.FindGameObjectsWithTag("Iron").Length;
		if (Input.GetMouseButtonDown(0))
		{
			if (isDeteleting)
			{
				GamePlayPanelUIManager.Instance.ButtonOff();
				selectDeteleNail();
			}
			else
			{
				Click();
			}
		}
	}
	public void Click()
	{
		Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

		RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(posMouse, 0.5f, Vector3.forward, Mathf.Infinity);

		for (int i = 0; i < cubeHit.Length; i++)
		{
			if (cubeHit[i].transform.gameObject.tag == "Hole")
			{
				curHole = cubeHit[i].collider.gameObject.GetComponent<Hole>();
				if (curHole.CheckNail() && curNail != curHole.getNail())
				{
					curNail = curHole.getNail();
					curNail.check();
					curNail.PickUp(curHole.transform.position);
				}

				goto lb100;
			}
		}
		AudioManager.instance.PlaySFX("Click");
		GameManagerNew.Instance.CreateFxClickFail(posMouse);
		curHole = null;

		lb100:

		if (curHole != null)
		{
			if (curHole == preHole)
			{
				preHole = null;
				if (curNail != null)
				{
					curNail.Unselect();
				}


			}
			else
			{
				if (preHole == null)
				{
					preHole = curHole;
				}
				if (curHole.CheckNail())
				{
					//if (curNail != null)
					//{
					//	curNail.Unselect();
					//}
					if (preHole.getNail() != null)
					{
						preHole.getNail().Unselect();
					}
					preHole = curHole;
					curNail = curHole.getNail();
					curNail.check();
					curNail.PickUp(curHole.transform.position);
				}
				else
				{
					if (curNail != null && curHole.isOsccupied == false)
					{
						// continue code
						SaveGameObject();
						curNail.SetTrigger();
						curNail.SetNewPos(curHole.transform.position);
						curNail.RemoveHinge();
						curHole.setNail(curNail);
						preHole.setNail(null);
						curNail = null;
						

					}
				}
			}
		}
		else
		{
			//do nothing;
		}
		//selectedHole = cubeHit[hole].transform.gameObject;
		////try
		////{
		////if (Notice.Instance.gameObject.activeSelf)
		////{
		////	StartCoroutine(DisappearNotice());
		////}
		////}
		////catch
		////{

		////}
		//preHingeJoint2D.Clear();
		//AudioManager.instance.PlaySFX("PickUpScrew");
	}
	public void CheckDoneLevel()
	{
		if (numOfIronPlates <= 0)
		{
			AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_win, () =>
			{
				UIManagerNew.Instance.GamePlayPanel.Close();
				GameManagerNew.Instance.ItemMoveControl.Init(sprRenderItem.transform.position, sprRenderItem.sprite, sprRenderItem.transform.localScale, UIManagerNew.Instance.WinUI.PosImage.transform.position);
				if (!sprRenderItem.gameObject.activeSelf)
				{
					sprRenderItem.gameObject.SetActive(false);
				}
				transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
				{
					Destroy(this.gameObject);
				});
				DataLevelManager.Instance.SetLevelDone(GameManagerNew.Instance.Level);
				UIManagerNew.Instance.WinUI.Appear();
			},null);
		}
	}
	public void selectDeteleNail()
	{
		Vector2 posMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D[] cubeHit = Physics2D.CircleCastAll(posMouse, 0.2f, Vector3.forward);
		if (cubeHit.Length > 0)
		{
			foreach (RaycastHit2D ray in cubeHit)
			{
				if (ray.collider.tag == "Hole")
				{
					//chọn hố
					if (ray.collider.GetComponent<Hole>().CheckNail() == true)
					{

						holeToDetele = ray.collider.GetComponent<Hole>();
						ClearData();
						SaveGameObject();
						nailToDetele = holeToDetele.getNail();
						nailToDetele.check();
						SaveGameObject();
						holeToDetele.setNail(null);
						nailToDetele.RemoveHinge();
						nailToDetele.gameObject.SetActive(false);
						setDeteleting(false);
						hasDelete = true;
						
					}
					//var Destroyeffect1 = Instantiate(destroyNailEffect, nailToDetele.transform.position, quaternion.identity);
					//Destroy(Destroyeffect1, 0.5f);
				}
			}
		}
	}

	private void TurnRed(bool status)
	{
		for (int i = 0; i < nailControls.Length; i++)
		{
			nailControls[i].redNailSprite.enabled = status;
		}
	}
	public void setDeteleting(bool status)
	{
		if(status == true)
		{
			GamePlayPanelUIManager.Instance.ButtonOff();
		}
		else
		{
			GamePlayPanelUIManager.Instance.ButtonOn();
		}
		isDeteleting = status;
		TurnRed(status);
	}
	public void SaveGameObject()
	{

		//code cu
		{
			//code đoạn này ngu vl
			ClearData();
			SavePreData();
			

		}
		hasSave = true;
		GamePlayPanelUIManager.Instance.UndoButton.interactable = true;
	}

	private void SavePreData()
	{
		nailToUndo = curNail;
		holeBeforeUndo = curHole;
		holeToUndo = preHole;
		foreach (var iron in ironPlates)
		{
			ironObjectsBeforemove.Add(iron);
		}
		for (int i = 0; i < ironObjectsBeforemove.Count; i++)
		{
			ironObjectsTransformBeforemove.Add(ironObjectsBeforemove[i].transform.localPosition);
			ironObjectsRotationBeforemove.Add(ironObjectsBeforemove[i].transform.rotation);

		}
		for (int i = 0; i < nailControls.Length; i++)
		{
			nailObjectsBeforemove.Add(nailControls[i]);

		}
		for (int i = 0; i < nailControls.Length; i++)
		{
			nailObjectsTransformBeforemove.Add(nailObjectsBeforemove[i].transform.position);
		}
		foreach (var iron in ironPlates)
		{
			foreach (var hinge in iron.hingeJoint2Ds)
			{
				if (hinge.connectedBody != null)
				{
					HingeJointBeforeRemove.Add(hinge);
					nailsJointBeforemove.Add(hinge.connectedBody.gameObject.GetComponent<NailControl>());
				}
			}
		}
	}

	public void Undo()
	{
		if (hasSave)
		{
			//code cu
			{
				for (int i = 0; i < ironObjectsBeforemove.Count; i++)
				{
					ironObjectsBeforemove[i].transform.SetLocalPositionAndRotation(ironObjectsTransformBeforemove[i], ironObjectsRotationBeforemove[i]);
					ironObjectsBeforemove[i].GetComponent<Collider2D>().isTrigger = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().isKinematic = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().freezeRotation = true;
					ironObjectsBeforemove[i].GetComponent<Rigidbody2D>().velocity = Vector3.zero;
					ironObjectsBeforemove[i].gameObject.SetActive(true);
				}
				if (hasDelete == false)
				{
					foreach (var iron in ironObjectsBeforemove)
					{
						foreach (var hinge in iron.hingeJoint2Ds)
						{
							if (hinge.connectedBody == nailToUndo.gameObject.GetComponent<Rigidbody2D>())
							{
								hinge.connectedBody = null;
								hinge.connectedAnchor = Vector2.zero;
								hinge.enabled = false;
							}
						}
					}
					nailToUndo.gameObject.transform.position = holeToUndo.transform.position;
					holeToUndo.setNail(nailToUndo);
					holeBeforeUndo.setNail(null);
				}
				else
				{
					RestoreDeteleData();
				}
				for (int i = 0; i < HingeJointBeforeRemove.Count; i++)
				{
					HingeJointBeforeRemove[i].connectedBody = nailsJointBeforemove[i].rigidbody2D;
					HingeJointBeforeRemove[i].connectedAnchor = Vector2.zero;
					HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = true;
					HingeJointBeforeRemove[i].autoConfigureConnectedAnchor = false;
				}
				hasDelete = false;
				Continute();
				resetData();
			}
		}
	}

	private void RestoreDeteleData()
	{
		nailToDetele.gameObject.SetActive(true);
		holeToDetele.setNail(nailToDetele);
		nailToDetele.collider2D.isTrigger = true;
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
		if (nailToDetele != null)
		{
			nailToDetele.collider2D.isTrigger = false;
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

			hasSave = false;
			GamePlayPanelUIManager.Instance.UndoButton.interactable = false;
		}
		catch (Exception e) { }
	}
	public void ChangeLayer()
	{
		int layer = LayerMask.NameToLayer(layerName); // Chuyển đổi tên layer thành ID layer
		if (layer != -1) // Kiểm tra xem layer có tồn tại không
		{
			if (holeToUnlock.gameObject != null)
			{
				holeToUnlock.gameObject.layer = layer; // Đặt layer cho GameObject

			}
		}
		else
		{
			Debug.LogError("Layer " + layerName + " does not exist!"); // In ra thông báo lỗi nếu layer không tồn tại
		}
	}
	public void resetData()
	{
		curNail = null;
		curHole = null;
		preHole = null;
	}
	public void ResetBooster()
	{
		UIManagerNew.Instance.DeteleNailPanel.numOfUsed = 1;
		UIManagerNew.Instance.UndoPanel.numOfUsed = 1;
		UIManagerNew.Instance.RePlayPanel.numOfUsed = 1;
		if (isDeteleting)
		{
			isDeteleting = false;
		}
	}
}
