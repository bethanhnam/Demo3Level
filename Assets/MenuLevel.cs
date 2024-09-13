using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuLevel : MonoBehaviour
{
	//public int x;
	//public int level;
	//public int currentStage = 0;
	//public Image menuImg;
	//public Sprite completeImg;
	//public Button[] playButtons;
	//public Button[] playButtons1;
	//public Button[] playButtons2;
	//public Button[] currentPlayButtons;
	//public int numOfButtons = 0;
	//public List<Button[]> listPlayButtons = new List<Button[]>();

	//public GameObject[] characters;
	//public GameObject[] itemPositions;
	//public GameObject[] itemPositions1;
	//public GameObject[] itemPositions2;
	//public GameObject[] targetItemPositions;
	//public List<GameObject> completeList = new List<GameObject>();
	//public List<GameObject> unCompleteList = new List<GameObject>();
	//public int stageLeft;
	//public bool hasDone = false;
	//public bool hasChange = false;
	//public bool justOpen = false;
	//public bool hasChangelv1 = false;
	//public bool hasChangelv2 = false;
	//private void Awake()
	//{
	//	numOfButtons = playButtons.Length + playButtons1.Length + playButtons2.Length;
	//}
	//private void Start()
	//{
	//	listPlayButtons.Add(playButtons);
	//	listPlayButtons.Add(playButtons1);
	//	listPlayButtons.Add(playButtons2);
	//	for (int i = 0; i < playButtons.Length; i++)
	//	{
	//		if (!unCompleteList.Contains(playButtons[i].gameObject))
	//		{
	//			unCompleteList.Add(playButtons[i].gameObject);
	//		}
	//	}
	//	UIManager.instance.menuPanel.slider[0].maxValue = LevelManager.instance.levelInstances[0].GetComponent<PictureUIManager>().Stage.Length;
	//	UIManager.instance.menuPanel.maxScore.text = UIManager.instance.menuPanel.slider[0].maxValue.ToString();
	//	if (PlayerPrefs.HasKey("CurrentStage"))
	//	{
	//		currentStage = PlayerPrefs.GetInt("CurrentStage");
	//	}
	//	else
	//	{
	//		currentStage = 0;
	//	}
	//	if (PlayerPrefs.GetInt("HasChangeLv2") == 1)
	//	{
	//		UnLockLevel1(playButtons2);
	//		LockLevel(playButtons1);
	//		LockLevel(playButtons);
	//	}
	//	else if (PlayerPrefs.GetInt("HasChangeLv1") == 1)
	//	{
	//		UnLockLevel1(playButtons1);
	//		LockLevel(playButtons);
	//	}
	//	else
	//	{
	//		UnLockLevel1(playButtons);
	//	}

	//}

	//private void Update()
	//{
	//	checkStage1();
	//	if (completeList.Count == numOfButtons)
	//	{
	//		PlayButton.instance.GetComponent<Button>().interactable = false;
	//		hasDone = true;
	//		PlayerPrefs.SetInt("HasDone", 1);
	//	}
	//	if (SaveSystem.instance.menuLevel == 1)
	//	{
	//		if (hasChangelv1 == true)
	//		{
	//			if (hasChangelv2 == false)
	//			{
	//				if (completeList.Count == playButtons1.Length)
	//				{
	//					hasChangelv2 = true;
	//					for (int i = 0; i < playButtons1.Length; i++)
	//					{
	//						PlayerPrefs.DeleteKey(i.ToString());
	//						UnityEngine.Debug.Log("detele " + i.ToString());
	//					}
	//					ShowStageItem(playButtons2);
	//					PlayerPrefs.SetInt("HasChangeLv2", 1);
	//					PlayerPrefs.SetInt("CurrentStage", 2);
	//				}

	//			}
	//		}
	//		else if (hasChangelv1 == false)
	//		{
	//			if (completeList.Count == playButtons.Length)
	//			{
	//				hasChangelv1 = true;
	//				for (int i = 0; i < playButtons.Length; i++)
	//				{
	//					PlayerPrefs.DeleteKey(i.ToString());
	//					UnityEngine.Debug.Log("detele " + i.ToString());
	//				}
	//				ShowStageItem(playButtons1);
	//				PlayerPrefs.SetInt("HasChangeLv1", 1);
	//				PlayerPrefs.SetInt("CurrentStage", 1);

	//			}
	//		}
	//	}
	//	if (PlayerPrefs.GetInt("HasChangeLv2") == 1)
	//	{
	//		SetPlayerPrefs(playButtons2);
	//	}
	//	else if (PlayerPrefs.GetInt("HasChangeLv1") == 1)
	//	{
	//		SetPlayerPrefs(playButtons1);
	//	}
	//	else
	//	{
	//		SetPlayerPrefs(playButtons);
	//	}
	//}

	//private void ShowStageItem(Button[] buttons)
	//{
	//	StartCoroutine(loadItem(playButtons1));
	//}
	//IEnumerator loadItem(Button[] buttons)
	//{
	//	yield return new WaitForSeconds(7);
	//	for (int j = 0; j < buttons.Length; j++)
	//	{
	//		buttons[j].gameObject.SetActive(true);
	//		itemPositions1[j].gameObject.SetActive(true);
	//	}
	//}

	//public void checkStage()
	//{
	//	if (PlayerPrefs.GetInt("HasChangeLv2") == 1)
	//	{
	//		currentPlayButtons = playButtons2;
	//		for (int j = 0; j < playButtons2.Length; j++)
	//		{
	//			playButtons2[j].gameObject.SetActive(true);
	//			itemPositions2[j].gameObject.SetActive(true);
	//		}
	//	}
	//	if (PlayerPrefs.GetInt("HasChangeLv1") == 1)
	//		{
	//			currentPlayButtons = playButtons1;
	//			for (int j = 0; j < playButtons1.Length; j++)
	//			{
	//				playButtons1[j].gameObject.SetActive(true);
	//				itemPositions1[j].gameObject.SetActive(true);
					
	//			}
	//		}	
	//	else
	//		{
	//			currentPlayButtons = playButtons;
	//		}
	//}
	//public void checkStage1()
	//{
	//	if (PlayerPrefs.GetInt("HasChangeLv2") == 1)
	//	{
	//		currentPlayButtons = playButtons2;
	//	}
	//	if (PlayerPrefs.GetInt("HasChangeLv1") == 1)
	//	{
	//		currentPlayButtons = playButtons1;
	//	}
	//	else
	//	{
	//		currentPlayButtons = playButtons;
	//	}
	//}
	//private void SetPlayerPrefs(Button[] buttons)
	//{
	//	for (int i = 0; i < buttons.Length; i++)
	//	{
	//		if (!PlayerPrefs.HasKey(i.ToString()))
	//		{
	//			if (buttons[i].GetComponent<LevelButton>().hasDone)
	//			{
	//				PlayerPrefs.SetInt(i.ToString(), 1);
	//				UnityEngine.Debug.Log(i.ToString());
	//				MenuLevelManager.instance.hasChecked = false;
	//			}
	//		}

	//	}
	//}
	//private void OnEnable()
	//{
	//	checkStage();
	//	StartCoroutine(checkReturn());
	//	if (PlayerPrefs.GetInt("HasChangeLv2") == 1)
	//	{
	//		UnLockLevel1(playButtons2);
	//		LockLevel(playButtons1);
	//		LockLevel(playButtons);
	//	}
	//	else if (PlayerPrefs.GetInt("HasChangeLv1") == 1)
	//	{
	//		UnLockLevel1(playButtons1);
	//		LockLevel(playButtons);
	//	}
	//	else
	//	{
	//		UnLockLevel1(playButtons);
	//	}

	//}


	//IEnumerator checkReturn()
	//{
	//	yield return new WaitForSeconds(1);
	//	if (UIManager.instance.menuPanel.Completing == false)
	//	{
	//		if (hasChange == false)
	//		{
	//			if (SaveSystem.instance.strike >= numOfButtons)
	//			{
	//				try
	//				{
	//					UIManager.instance.menuPanel.ChangeImgDoneLevel();
	//				}
	//				catch
	//				{

	//				}
	//			}
	//		}
	//	}
	//}
	//private void UnLockLevel1(Button[] button)
	//{
	//	try
	//	{
	//		for (int i = 0; i < button.Length; i++)
	//		{
	//			if (PlayerPrefs.HasKey(i.ToString()))
	//			{
	//				if (PlayerPrefs.GetInt(i.ToString()) == 1)
	//				{
	//					button[i].gameObject.SetActive(false);
	//					if (button[i].GetComponent<LevelButton>().fixedImg != null)
	//					{
	//						button[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
	//						button[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
	//						try
	//						{
	//							if (!completeList.Contains(button[i].gameObject))
	//							{
	//								completeList.Add(button[i].gameObject);
	//							}
	//						}
	//						catch { }
	//						if (unCompleteList.Contains(completeList[i]))
	//						{
	//							unCompleteList.Remove(completeList[i]);
	//						}
	//					}

	//				}
	//			}
	//		}
	//	}
	//	catch { };
	//}
	//private void UnLockLevel()
	//{
	//	try
	//	{
	//		for (int i = 0; i < playButtons.Length; i++)
	//		{
	//			if (playButtons[i] != null)
	//			{
	//				if (PlayerPrefs.HasKey(i.ToString()))
	//				{
	//					if (PlayerPrefs.GetInt(i.ToString()) == 1)
	//					{
	//						playButtons[i].gameObject.SetActive(false);
	//						if (playButtons[i].GetComponent<LevelButton>().fixedImg != null)
	//						{
	//							playButtons[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
	//							playButtons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
	//							try
	//							{
	//								if (!completeList.Contains(playButtons[i].gameObject))
	//								{
	//									completeList.Add(playButtons[i].gameObject);
	//								}
	//							}
	//							catch { }
	//							if (unCompleteList.Contains(playButtons[i].gameObject))
	//							{
	//								unCompleteList.Remove(playButtons[i].gameObject);
	//							}
	//						}
	//					}
	//				}
	//			}
	//		}
	//	}
	//	catch { };

	//}
	//private void LockLevel(Button[] buttons)
	//{

	//	for (int i = 0; i < buttons.Length; i++)
	//	{
	//		if (buttons[i] != null)
	//		{
	//			buttons[i].gameObject.SetActive(false);
	//			if (buttons[i].GetComponent<LevelButton>().fixedImg != null)
	//			{
	//				buttons[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
	//				buttons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
	//				try
	//				{
	//					if (!completeList.Contains(buttons[i].gameObject))
	//					{
	//						completeList.Add(buttons[i].gameObject);
	//					}
	//				}
	//				catch { }
	//			}
	//		}
	//	}
	//}
	//public void NextLevel()
	//{
	//	StartCoroutine(nextLevel());
	//}
	//IEnumerator nextLevel()
	//{
	//	yield return new WaitForSeconds(2);
	//	UIManager.instance.menuPanel.CheckStrike();
	//}
}
