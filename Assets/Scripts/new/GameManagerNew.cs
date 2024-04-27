using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManagerNew : MonoBehaviour
{
	public static GameManagerNew Instance;

	private PictureUIManager pictureUIManager;

	[SerializeField]
	private Transform parPic;
	[SerializeField]
	private Transform gamePlayPanel;
	[SerializeField]
	private Stage currentLevel;
	private int level;

	private LayerMask iNSelectionLayer1;
	private LayerMask IronLayer12;

	[SerializeField]
	private GameObject clickEffect;

	[SerializeField]
	private ItemMoveControl itemMoveControl;

	public LayerMask INSelectionLayer { get => iNSelectionLayer1; }
	public LayerMask IronLayer1 { get => IronLayer12; }
	public Stage CurrentLevel { get => currentLevel; set => currentLevel = value; }
	public int Level { get => level; set => level = value; }
	public ItemMoveControl ItemMoveControl { get => itemMoveControl; }
	public PictureUIManager PictureUIManager { get => pictureUIManager; set => pictureUIManager = value; }
	public Transform GamePlayPanel { get => gamePlayPanel; set => gamePlayPanel = value; }

	private void Awake()
	{
		Instance = this;
		iNSelectionLayer1 = LayerMask.GetMask("Hole");
		iNSelectionLayer1 = LayerMask.GetMask("Hole");
		IronLayer12 = LayerMask.GetMask("IronLayer1", "IronLayer2", "IronLayer3", "IronLayer4", "IronLayer5", "IronLayer6", "IronLayer7", "IronLayer8", "BothLayer", "layer1vs2", "layer1vs2vs3", "layer1vs2vs3vs4", "layer1vs2vs3vs4");
	}

	public void InitStartGame()
	{
		PictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PictureUIManager, parPic);
		ScalePicForDevices(PictureUIManager.transform.gameObject);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock.Length);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		SetCompletImg();
		SetCompleteStory();
		Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);
	}
	public void ScalePicForDevices(GameObject obj)
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;

		if (windowAspect < targetAspect)
		{
			obj.transform.localScale = obj.transform.localScale * (targetAspect / windowAspect);
		}

	}
	public void ScaleForDevices(GameObject obj)
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;

		if (windowAspect < targetAspect)
		{
			obj.transform.localScale = obj.transform.localScale / (targetAspect / windowAspect);
		}

	}

	public void CreateLevel(int _level)
	{
		Level = _level;
		CurrentLevel = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Level, new Vector2(0, 1), Quaternion.identity, GamePlayPanel);
		ScaleForDevices(CurrentLevel.transform.gameObject);
		CurrentLevel.Init(Level);
		PictureUIManager.Close(); 
		UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
	}
	public void Replay()
	{
		Destroy(currentLevel.gameObject);
		ReOpenLevel(() =>
		{
			GamePlayPanelUIManager.Instance.Settimer(181);
			CurrentLevel = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Level, new Vector2(0, 1), Quaternion.identity, GamePlayPanel);
			CurrentLevel.transform.SetSiblingIndex(0);
			CurrentLevel.Init(Level);
		});
	}
	public void ReOpenLevel(Action action)
	{
		UIManagerNew.Instance.GamePlayPanel.Appear();
		action();
	}
	public void CloseLevel(bool status)
	{
			CurrentLevel.gameObject.SetActive(status);
	}
	public void CloseLevelForReopen(bool status)
	{
		if (status == true)
		{
			Vector3 targetScale = CurrentLevel.transform.localScale;
			CurrentLevel.transform.localScale = new Vector3(0.5f, 0.5f, 1);
			CurrentLevel.transform.DOScale(targetScale, 0.3f).OnComplete(() =>
			{
				CurrentLevel.gameObject.SetActive(status);
				
			});
		}
		else
		{
			CurrentLevel.gameObject.SetActive(status);
		}
	}
	public void CreateFxClickFail(Vector2 pos)
	{
		var clickeffect = Instantiate(clickEffect, pos, Quaternion.identity);
		Destroy(clickeffect, 0.2f);
	}
	public void BackToMenu()
	{
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		PictureUIManager.Open();
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		UIManagerNew.Instance.GamePlayPanel.Close();
	}

	public void CallWin()
	{

	}
	public void WinContinueButton()
	{
		PictureUIManager.Open();
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		itemMoveControl.MoveToFix(Vector3.one, PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjLock[level].transform.position,() =>
		{
			UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
			PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
			itemMoveControl.gameObject.SetActive(false);
			CreateParticleEF();
			PictureUIManager.ChangeItem(PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[level]);
			UIManagerNew.Instance.ChestSLider.ChangeValue(() =>
			{
				SetCompleteStory();
			});
		});
		UIManagerNew.Instance.WinUI.Close();
	}

	private void CreateParticleEF()
	{
		Vector3 spawnPos = new Vector3(PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[level].transform.position.x, PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[level].transform.position.y + 4, 1);
		var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, spawnPos, Quaternion.identity);
		ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
		var shape = particleSystem.shape;
		shape.sprite = itemMoveControl.GetComponentInChildren<Image>().sprite;
		Destroy(gameobj, 1f);
	}
	public void SetCompleteStory()
	{
		if(LevelManagerNew.Instance.LevelBase.CountLevelWin == PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock.Length)
		{
			if (DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage + 1 == PictureUIManager.Stage.Length)
			{
				StartCoroutine(CompleteImgAppear());
			}
			else
			{
				StartCoroutine(NextStage(() =>
				{
					UIManagerNew.Instance.ButtonMennuManager.Appear();
				}));
			}
		}
		else
		{
			if (UIManagerNew.Instance.CompleteImg.gameObject.activeSelf)
			{
				CompleteImgDisappear();
			}
		}
	}
	IEnumerator CompleteImgAppear()
	{
		yield return new WaitForSeconds(1);
		UIManagerNew.Instance.CompleteImg.gameObject.SetActive(true);
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().alpha = 0;
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().DOFade(1, 0.3f);
		UIManagerNew.Instance.ButtonMennuManager.Close();
	}
	public void CompleteImgDisappear()
	{
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().alpha = 1;
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() =>
		{
			UIManagerNew.Instance.CompleteImg.gameObject.SetActive(false);
		});
	}
	public void SetCompletImg()
	{
		UIManagerNew.Instance.CompleteImg.sprite = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Completeimg;
		UIManagerNew.Instance.CompleteImg.SetNativeSize();
	}
	IEnumerator NextStage(Action action)
	{
		yield return new WaitForSeconds(1);
		LevelManagerNew.Instance.NextStage();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock.Length);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		action();
	}
	public void NextLevelPicture()
	{
		Destroy(PictureUIManager.gameObject);
		LevelManagerNew.Instance.NetxtLevel();
		PictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PictureUIManager, parPic);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager.Stage[0].ObjunLock.Length);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		SetCompletImg();
		SetCompleteStory();
		Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);

	}
}
