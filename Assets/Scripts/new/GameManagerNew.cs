using DG.Tweening;
using JetBrains.Annotations;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class GameManagerNew : MonoBehaviour
{
	public static GameManagerNew Instance;

	private PictureUIManager pictureUIManager;

	[SerializeField]
	public Transform parPic;
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

	[SerializeField]
	private GameObject Bg;

	[SerializeField]
	private Vector3 targetScale;

	public LayerMask INSelectionLayer { get => iNSelectionLayer1; }
	public LayerMask IronLayer1 { get => IronLayer12; }
	public Stage CurrentLevel { get => currentLevel; set => currentLevel = value; }
	public int Level { get => level; set => level = value; }
	public ItemMoveControl ItemMoveControl { get => itemMoveControl; }
	public PictureUIManager PictureUIManager { get => pictureUIManager; set => pictureUIManager = value; }
	public Transform GamePlayPanel { get => gamePlayPanel; set => gamePlayPanel = value; }
	public Vector3 TargetScale { get => targetScale; set => targetScale = value; }

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
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		SetCompletImg();
		SetCompleteStory();
		Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		Bg.SetActive(true);
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
		SetTargetScale(currentLevel.gameObject);
		UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
		GamePlayPanelUIManager.Instance.setText(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Id);
		CurrentLevel.Init(Level);
		CurrentLevel.ResetBooster();
		PictureUIManager.Close();
		AudioManager.instance.PlayMusic("GamePlayTheme");
		FirebaseAnalyticsControl.Instance.LogEventGamePlayAccessSuccessfully(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Id);
	}
	public Vector3 ScalelevelForDevices(GameObject obj)
	{
		float targetAspect = 9.0f / 16.0f;
		float windowAspect = (float)Screen.width / (float)Screen.height;

		if (windowAspect < targetAspect)
		{
			obj.transform.localScale = obj.transform.localScale * (targetAspect / windowAspect);
		}
		return obj.transform.localScale;
	}
	public void SetTargetScale(GameObject gameObject)
	{
		TargetScale = gameObject.transform.localScale;
	}
	public void Replay()
	{
		currentLevel.resetData();
		GamePlayPanelUIManager.Instance.ShowNotice(false);
		currentLevel.Close(true);
		ReOpenLevel(() =>
		{
			GamePlayPanelUIManager.Instance.Settimer(181);
			CurrentLevel = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Level, new Vector2(0, 1), Quaternion.identity, GamePlayPanel);
			currentLevel.resetData();
			Stage.Instance.DeactiveDeleting();
			GamePlayPanelUIManager.Instance.ShowNotice(false);
			GamePlayPanelUIManager.Instance.ButtonOn();
			CurrentLevel.Init(Level);
		});
	}
	public void Retry()
	{
		currentLevel.resetData();
		GamePlayPanelUIManager.Instance.ShowNotice(false);
		currentLevel.Close(true);
		ReOpenLevel(() =>
		{
			GamePlayPanelUIManager.Instance.Settimer(181);
			CurrentLevel = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[Level].Level, new Vector2(0, 1), Quaternion.identity, GamePlayPanel);
			currentLevel.resetData();
			GamePlayPanelUIManager.Instance.ShowNotice(false);
			Stage.Instance.ResetBooster();
			GamePlayPanelUIManager.Instance.ButtonOn();
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
		if (status == true)
		{
			pictureUIManager.DisableCharacter();
		}
		CurrentLevel.Close(status);

	}
	public void OpenLevel(bool status)
	{

		CurrentLevel.gameObject.SetActive(true);

	}
	public void ClosePicture(bool status)
	{
		pictureUIManager.gameObject.SetActive(status);
		//if (status == true)
		//{
		//	foreach (var character in pictureUIManager.characters)
		//	{
		//		character.gameObject.SetActive(true);
		//	}

		//}
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
		//PictureUIManager.Open();
		//UIManagerNew.Instance.ButtonMennuManager.Appear();

	}

	public void CreateParticleEF()
	{
		Vector3 spawnPos = new Vector3(PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[level].transform.position.x, PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[level].transform.position.y + 4, 1);
		var gameobj = Instantiate(ParticlesManager.instance.StarParticleObject, spawnPos, Quaternion.identity);
		ParticleSystem particleSystem = gameobj.transform.GetChild(0).GetComponent<ParticleSystem>();
		var shape = particleSystem.shape;
		shape.sprite = itemMoveControl.GetComponentInChildren<Image>().sprite;
		var Emission = particleSystem.emission;
		Emission.SetBursts(new ParticleSystem.Burst[] { (cauculateParticle(shape.sprite)) });
		Destroy(gameobj, 1f);
	}
	public ParticleSystem.Burst cauculateParticle(Sprite sprite)
	{

		ParticleSystem.Burst burst = new ParticleSystem.Burst();
		if (sprite.bounds.size.x > 500 || sprite.bounds.size.y > 500)
		{
			burst.maxCount = 400;
			burst.minCount = 250;

		}
		else
		{
			burst.maxCount = 50;
			burst.minCount = 20;
		}
		return burst;
	}
	public void SetCompleteStory()
	{
		var winStrike = 0;
		if (DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage >= 1)
		{
			for (int i = 0; i <= DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage; i++)
			{
				winStrike += PictureUIManager.Stage[i].ObjunLock.Length;
			}
		}
		else
		{
			winStrike = PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock.Length;
		}
		if (LevelManagerNew.Instance.LevelBase.CountLevelWin == winStrike)
		{
			if (DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage + 1 == PictureUIManager.Stage.Length)
			{
				CheckSliderValue();
			}
			else
			{
				StartCoroutine(NextStage(() =>
				{
					//UIManagerNew.Instance.ButtonMennuManager.Appear();
				}));
			}
		}
		else
		{
			pictureUIManager.ChangeReaction(1.2f, 0, true);
			if (UIManagerNew.Instance.CompleteImg.gameObject.activeSelf)
			{
				CompleteImgDisappear();
			}
		}
	}
	private void CreateCharacterParticleEF(Vector3 position, MeshRenderer mesh)
	{
		var gameobj = Instantiate(ParticlesManager.instance.characterReactionParticle, position, Quaternion.identity);
		ParticleSystem particleSystem = gameobj.GetComponent<ParticleSystem>();
		var shape = particleSystem.shape;
		//shape.meshRenderer = SkeletonGraphic.;
		Destroy(gameobj, 1f);
	}
	public void CompleteImgAppearViaButton()
	{
		StartCoroutine(CompleteImgAppear());
	}
	IEnumerator CompleteImgAppear()
	{
		pictureUIManager.ChangeReaction(0, 2, false);
		yield return new WaitForSeconds(1f);
		foreach (var character in pictureUIManager.characters)
		{
			character.transform.SetParent(UIManagerNew.Instance.CompleteImg.transform);
			CreateCharacterParticleEF(character.transform.position + new Vector3(0,1.5f,1), character.transform.GetComponent<MeshRenderer>());
		}
		pictureUIManager.ChangeReaction(0, 3, true);
		AudioManager.instance.musicSource.Play();
		UIManagerNew.Instance.CompleteImg.gameObject.SetActive(true);
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().alpha = 0;
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().DOFade(1, 1f);
		UIManagerNew.Instance.ButtonMennuManager.Close();
	}
	public void CompleteImgDisappear()
	{
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().alpha = 1;
		UIManagerNew.Instance.CompleteImg.GetComponent<CanvasGroup>().DOFade(0, 0.1f).OnComplete(() =>
		{
			UIManagerNew.Instance.CompleteImg.gameObject.SetActive(false);
			AudioManager.instance.musicSource.Play();
		});
	}
	public void SetCompletImg()
	{
		UIManagerNew.Instance.CompleteImg.sprite = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Completeimg;
		UIManagerNew.Instance.CompleteImg.SetNativeSize();
		ScalePicForDevices(UIManagerNew.Instance.CompleteImg.transform.gameObject);
	}
	IEnumerator NextStage(Action action)
	{
		yield return new WaitForSeconds(1);
		LevelManagerNew.Instance.NextStage();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		action();
	}
	public void NextLevelPicture()
	{
		//if (LevelManagerNew.Instance.LevelBase.Level == 0)
		//{
		//	UIManagerNew.Instance.ButtonMennuManager.OpenRattingPanel();
		//}
		//else
		//{
			pictureUIManager.DisableCharacter();
			CompleteLevelAfterReward();
		//}
	}
	public void CompleteLevelAfterReward()
	{
		Destroy(PictureUIManager.gameObject);
		LevelManagerNew.Instance.NetxtLevel();
		PictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PictureUIManager, parPic);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		ScalePicForDevices(PictureUIManager.transform.gameObject);
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		SetCompletImg();
		SetCompleteStory();
		UIManagerNew.Instance.CongratPanel.TakeReward();
		Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);
	}
	public void RecreatePicAfterCompleteGame()
	{
		Destroy(PictureUIManager.gameObject);
		PictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PictureUIManager, parPic);
		PictureUIManager.Init(LevelManagerNew.Instance.LevelBase.Level);
		ScalePicForDevices(PictureUIManager.transform.gameObject);
		UIManagerNew.Instance.ButtonMennuManager.Appear();
		UIManagerNew.Instance.ChestSLider.SetMaxValue(PictureUIManager);
		UIManagerNew.Instance.ChestSLider.SetCurrentValue(LevelManagerNew.Instance.LevelBase.CountLevelWin);
		SetCompletImg();
		SetCompleteStory();
		UIManagerNew.Instance.CongratPanel.TakeReward();
		Debug.Log(LevelManagerNew.Instance.LevelBase.CountLevelWin);
	}
	public void CheckSliderValue()
	{
		if (UIManagerNew.Instance.ChestSLider.mySlider.value == UIManagerNew.Instance.ChestSLider.mySlider.maxValue)
		{
			StartCoroutine(DisPlayPresent());
		}
	}
	IEnumerator DisPlayPresent()
	{
		yield return new WaitForSeconds(1f);
		UIManagerNew.Instance.ButtonMennuManager.DisPlayPresent();
	}
}
