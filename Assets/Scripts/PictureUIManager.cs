using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureUIManager : MonoBehaviour
{

	[SerializeField]
	private ItemInStage[] stage;

	private int level;

	[SerializeField]
	private CanvasGroup canvasGroup;

	public ItemInStage[] Stage { get => stage; set => stage = value; }
	public int Level { get => level; set => level = value; }

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
							stage[i].ObjLock[j].SetActive(false);
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
	}
	public void ChangeItem(GameObject obj)
	{
		obj.transform.DOScale(1.05f, 0.2f).OnComplete(() =>
		{
			obj.transform.DOScale(0.95f, 0.2f).OnComplete(() =>
			{
				obj.transform.DOScale(1f, 0.1f);
			});
		});
	}

	public void Close()
	{
		canvasGroup.blocksRaycasts = false;
		canvasGroup.DOFade(0, 0.3f).OnComplete(() =>
		{
			gameObject.SetActive(false);
		});
	}
	public void Open()
	{
		canvasGroup.blocksRaycasts = true;
		canvasGroup.DOFade(1, 0.3f).OnComplete(() =>
		{
			gameObject.SetActive(true);
		});
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
