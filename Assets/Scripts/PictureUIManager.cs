using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictureUIManager : MonoBehaviour
{

	[SerializeField]
	private ItemInStage[] stage;

	private int level;

	[SerializeField]
	private CanvasGroup canvasGroup;

	public SkeletonGraphic[] characters;

	public GameObject windowObj;

	public String[] animationStage = { "idle_sad", "sad-happy", "happy", "idle_happy", "tremble", "tremble_happy" };
	public ItemInStage[] Stage { get => stage; set => stage = value; }
	public int Level { get => level; set => level = value; }
	public CanvasGroup CanvasGroup { get => canvasGroup; set => canvasGroup = value; }

	//bool
	public bool hasWindow = false;

    //tutor pic
	public PicTutor picTutor;
    public bool hasFixed = false;

    public void Init(int _level)
	{

		level = _level;
		if (level >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{
			level = 0;
		}

		for (int i = 0; i < stage.Length; i++)
		{
			if (i < DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
			{
				for (int j = 0; j < stage[i].ObjBtn.Length; j++)
				{
					if (stage[i].ObjBtn[j].activeSelf)
					{
						stage[i].ObjBtn[j].SetActive(false);
					}
				}

				for (int j = 0; j < stage[i].ObjLock.Length; j++)
				{
					if (stage[i].ObjLock[j].activeSelf)
					{
						stage[i].ObjLock[j].SetActive(false);
					}
				}

				for (int j = 0; j < stage[i].ObjunLock.Length; j++)
				{
					if (!stage[i].ObjunLock[j].activeSelf)
					{
						stage[i].ObjunLock[j].SetActive(true);
					}
				}
			}
			else
			{
				if (i == DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
				{
					for (int j = 0; j < stage[i].ObjBtn.Length; j++)
					{
						if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
						{
							if (stage[i].ObjBtn[j].activeSelf)
							{
								stage[i].ObjBtn[j].SetActive(false);
							}
						}
						else
						{
							if (!stage[i].ObjBtn[j].activeSelf)
							{
								stage[i].ObjBtn[j].SetActive(true);
							}
						}
					}

					for (int j = 0; j < stage[i].ObjLock.Length; j++)
					{
						if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
						{
							if (stage[i].ObjLock[j].activeSelf)
							{
								stage[i].ObjLock[j].SetActive(false);
							}
						}
						else
						{
							if (!stage[i].ObjLock[j].activeSelf)
							{
								stage[i].ObjLock[j].SetActive(true);
							}
						}
					}

					for (int j = 0; j < stage[i].ObjunLock.Length; j++)
					{
						if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
						{
							if (!stage[i].ObjunLock[j].activeSelf)
							{
								stage[i].ObjunLock[j].SetActive(true);
							}
						}
						else
						{
							if (stage[i].ObjunLock[j].activeSelf)
							{
								stage[i].ObjunLock[j].SetActive(false);
							}
						}
					}
				}
				else
				{
					for (int j = 0; j < stage[i].ObjBtn.Length; j++)
					{
						if (stage[i].ObjBtn[j].activeSelf)
						{
							stage[i].ObjBtn[j].SetActive(false);
						}
					}

					for (int j = 0; j < stage[i].ObjLock.Length; j++)
					{
						if (stage[i].ObjLock[j].activeSelf)
						{
							stage[i].ObjLock[j].SetActive(true);
						}
					}

					for (int j = 0; j < stage[i].ObjunLock.Length; j++)
					{
						if (stage[i].ObjunLock[j].activeSelf)
						{
							stage[i].ObjunLock[j].SetActive(false);
						}
					}
				}
			}
		}
		SetStarText();
		CheckForWindow();

	}
	public void CheckForWindow()
	{

		if (windowObj.gameObject != null && windowObj.gameObject.activeSelf)
		{
			hasWindow = true;
		}
		else
		{
			PlayerPrefs.SetInt("windowFixed", 1);
			hasWindow = false;
		}
	}
	public void ChangeReaction(float time, string t, bool loop, bool hasWindow)
	{
		StartCoroutine(ChangeReaction1(time, t, loop, hasWindow));
	}
	IEnumerator ChangeReaction1(float time, string t, bool loop, bool hasWindow)
	{
		yield return new WaitForSeconds(time);
		if (characters != null)
		{
			foreach (var character in characters)
			{
                character.gameObject.SetActive(true);
                character.AnimationState.SetAnimation(0, t, loop);
			}
		}
	}
	public void HiddenButton()
	{
		for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn.Length; j++)
		{
			stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].SetActive(false);
		}
	}
	public void DisplayButton()
	{
		for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn.Length; j++)
		{
			if (!DataLevelManager.Instance.DataLevel.Data[level].Stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].DataItmeLevel[j].IsUnlock)
			{
				stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].SetActive(true);
				stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].transform.localScale = Vector3.zero;
				stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].transform.DOScale(1f, 0.2f);
				stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].transform.GetChild((1)).GetComponent<Image>().SetNativeSize();
			}
		}
		SetStarText();

    }
    public void SetStarText()
	{
		for (int j = 0; j < stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn.Length; j++)
		{
			var starText = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[j].Star;
			stage[DataLevelManager.Instance.DataLevel.Data[level].IndexStage].ObjBtn[j].GetComponentInChildren<LevelButton>().SetStarText(starText);
		}
	}
	public void ChangeItem(GameObject obj)
	{
		int bouncingImage = Animator.StringToHash("FixedObjBouncing");
		obj.GetComponent<Animator>().Play(bouncingImage);
	}

	public void Close()
	{
		CanvasGroup.blocksRaycasts = false;
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].gameObject.SetActive(false);
		}
		CanvasGroup.DOFade(0, 0.3f).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});
	}
	public void Open()
	{
		for (int i = 0; i < characters.Length; i++)
		{
			characters[i].gameObject.SetActive(true);
		}
		gameObject.SetActive(true);
		CanvasGroup.DOFade(1, 0.3f).OnComplete(() =>
		{

			CanvasGroup.blocksRaycasts = true;
		});
	}
	public void DisableCV()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.interactable = false;
	}
	public void EnableCV()
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.interactable = true;
	}
	public void ChangeItemOnly(int _level,bool showBT = true)
	{
		level = _level;
		if (level >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{
			level = 0;
		}

		for (int i = 0; i < stage.Length; i++)
		{
			if (i < DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
			{
				for (int j = 0; j < stage[i].ObjBtn.Length; j++)
				{
					if (stage[i].ObjBtn[j].activeSelf)
					{
						stage[i].ObjBtn[j].SetActive(false);
					}
				}

				for (int j = 0; j < stage[i].ObjLock.Length; j++)
				{
					if (stage[i].ObjLock[j].activeSelf)
					{
						stage[i].ObjLock[j].SetActive(false);
					}
				}

				for (int j = 0; j < stage[i].ObjunLock.Length; j++)
				{
					if (!stage[i].ObjunLock[j].activeSelf)
					{
						stage[i].ObjunLock[j].SetActive(true);
					}
				}
			}
			else
			{
				if (i == DataLevelManager.Instance.DataLevel.Data[level].IndexStage)
				{
					//for (int j = 0; j < stage[i].ObjBtn.Length; j++)
					//{
					//	if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
					//	{
					//		if (stage[i].ObjBtn[j].activeSelf)
					//		{
					//			stage[i].ObjBtn[j].SetActive(false);
					//		}
					//	}
					//	else
					//	{
					//		if (!stage[i].ObjBtn[j].activeSelf)
					//		{
					//			stage[i].ObjBtn[j].SetActive(true);
					//		}
					//	}
					//}

					for (int j = 0; j < stage[i].ObjLock.Length; j++)
					{
						if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
						{
							if (stage[i].ObjLock[j].activeSelf)
							{
								stage[i].ObjLock[j].SetActive(false);
							}
						}
						else
						{
							if (!stage[i].ObjLock[j].activeSelf)
							{
								stage[i].ObjLock[j].SetActive(true);
							}
						}
					}

					for (int j = 0; j < stage[i].ObjunLock.Length; j++)
					{
						if (DataLevelManager.Instance.DataLevel.Data[level].Stage[i].DataItmeLevel[j].IsUnlock)
						{
							if (!stage[i].ObjunLock[j].activeSelf)
							{
								stage[i].ObjunLock[j].SetActive(true);
							}
							hasFixed = true;
						}
						else
						{
							if (stage[i].ObjunLock[j].activeSelf)
							{
								stage[i].ObjunLock[j].SetActive(false);
							}
						}
					}
				}
				else
				{
					for (int j = 0; j < stage[i].ObjBtn.Length; j++)
					{
						if (stage[i].ObjBtn[j].activeSelf)
						{
							stage[i].ObjBtn[j].SetActive(false);
						}
					}

					for (int j = 0; j < stage[i].ObjLock.Length; j++)
					{
						if (stage[i].ObjLock[j].activeSelf)
						{
							stage[i].ObjLock[j].SetActive(true);
						}
					}

					for (int j = 0; j < stage[i].ObjunLock.Length; j++)
					{
						if (stage[i].ObjunLock[j].activeSelf)
						{
							stage[i].ObjunLock[j].SetActive(false);
						}
					}
				}
			}
		}
		HiddenButton();
        StartCoroutine(NormalInit(showBT));
	}
	IEnumerator NormalInit(bool showBT)
	{
		yield return new WaitForSeconds(.5f);
		Init(level);
		if (showBT) { 
			DisplayButton(); 
		}
		//if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
		//{
		//	UIManagerNew.Instance.ButtonMennuManager.Appear();
		//}
		if (!GameManagerNew.Instance.CheckSliderValueAndDisplay())
		{
			UIManagerNew.Instance.ButtonMennuManager.ActiveCVGroup();
		}
	}

	public Vector3 GetCurrentPosItem()
	{
		return stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].ObjunLock[GameManagerNew.Instance.Level].transform.position;
	}
	public void DisableCharacter()
	{
		foreach (var character in characters)
		{
			Destroy(character.gameObject);
		}
	}
	public void SetHasWindowFirstTime()
	{
		if (PlayerPrefs.GetInt("windowFixed") == 1)
		{
			hasWindow = false;
			ChangeReaction(0, "idle_sad", true, GameManagerNew.Instance.PictureUIManager.hasWindow);
		}
	}
}
[Serializable]
public class ItemInStage
{
	[SerializeField]
	private GameObject[] objLock;
	[SerializeField]
	private GameObject[] objunLock;
	[SerializeField]
	private GameObject[] objBtn;

	public GameObject[] ObjLock { get => objLock; set => objLock = value; }
	public GameObject[] ObjunLock { get => objunLock; set => objunLock = value; }
	public GameObject[] ObjBtn { get => objBtn; set => objBtn = value; }
}
