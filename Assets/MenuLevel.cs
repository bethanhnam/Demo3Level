using System;
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
	public int x;
	public int level;
	public int currentStage = 0;
	public Image menuImg;
	public Sprite completeImg;
	public Button[] playButtons;
	public Button[] playButtons1;
	public Button[] playButtons2;
	public int numOfButtons =0 ;
	public List<Button[]> listPlayButtons = new List<Button[]>();

	public GameObject[] characters;
	public GameObject[] itemPositions;
	public GameObject[] itemPositions1;
	public GameObject[] itemPositions2;
	public GameObject[] targetItemPositions;
	public List<GameObject> completeList = new List<GameObject>();
	public List<GameObject> unCompleteList = new List<GameObject>();
	public int stageLeft;
	public bool hasDone = false;
	public bool hasChange = false;
	public bool hasChangelv1 = false;
	public bool hasChangelv2 = false;
	private void Start()
	{
		listPlayButtons.Add(playButtons);
		listPlayButtons.Add(playButtons1);
		listPlayButtons.Add(playButtons2);
		for (int i = 0; i < playButtons.Length; i++)
		{
			if (!unCompleteList.Contains(playButtons[i].gameObject))
			{
				unCompleteList.Add(playButtons[i].gameObject);
			}
		}
		UIManager.instance.menuPanel.slider[0].maxValue = playButtons.Length + playButtons1.Length + playButtons2.Length;
		UIManager.instance.menuPanel.slider[1].maxValue = playButtons.Length + playButtons1.Length + playButtons2.Length;
		UIManager.instance.menuPanel.slider[2].maxValue = playButtons.Length + playButtons1.Length + playButtons2.Length;
		UIManager.instance.menuPanel.slider[2].value = playButtons.Length + playButtons1.Length + playButtons2.Length;
		UIManager.instance.menuPanel.maxScore.text = UIManager.instance.menuPanel.slider[0].maxValue.ToString();
		numOfButtons = playButtons.Length + playButtons1.Length + playButtons2.Length;
	}
	
	private void Update()
	{
		SetPlayerPrefs(0);
		if (completeList.Count == UIManager.instance.menuPanel.slider[0].maxValue)
		{
			PlayButton.instance.GetComponent<Button>().interactable = false;
			hasDone = true;
		}
		//if (hasChangelv1 == true)
		//{
		//	if (hasChangelv2 == false)
		//	{
		//		if (completeList.Count == playButtons1.Length)
		//		{
		//			for (int i = 0; i < playButtons1.Length; i++)
		//			{
		//				PlayerPrefs.DeleteKey(i.ToString());
		//			}
		//			for (int j = 0; j < playButtons2.Length; j++)
		//			{
		//				playButtons2[j].gameObject.SetActive(true);
		//				itemPositions2[j].gameObject.SetActive(true);
		//			}
		//			hasChangelv2 = true;
		//			currentStage++;
		//		}

		//	}
		//}
		//else if (hasChangelv1 == false)
		//{
		//	if (completeList.Count == playButtons.Length)
		//	{
		//		for (int i = 0; i < playButtons.Length; i++)
		//		{
		//			PlayerPrefs.DeleteKey(i.ToString());
		//		}
		//		for (int j = 0; j < playButtons1.Length; j++)
		//		{
		//			playButtons1[j].gameObject.SetActive(true);
		//			itemPositions1[j].gameObject.SetActive(true);
		//		}
		//		hasChangelv1 = true;
		//		currentStage++;
		//	}
		//}
		//if (hasChangelv2)
		//{
		//	SetPlayerPrefs(2);
		//}
		//else if (hasChangelv1)
		//{
		//	SetPlayerPrefs(1);
		//}
		//else
		//{
		//	SetPlayerPrefs(0);
		//}
		
	}
	private void SetPlayerPrefs(int currentStage)
	{
		Button[] array = listPlayButtons[currentStage];
		for (int i = 0; i < array.Length; i++)
		{
			if (!PlayerPrefs.HasKey(i.ToString())) { 
				if (array[i].GetComponent<LevelButton>().hasDone)
				{
					PlayerPrefs.SetInt(i.ToString(), 1);
					MenuLevelManager.instance.hasChecked = false;
				}
			}
			
		}
	}
	private void OnEnable()
	{
		if (hasChange == false)
		{
			if (SaveSystem.instance.strike >= numOfButtons)
			{

				try
				{
					UIManager.instance.menuPanel.bgImg.transform.GetChild(0).GetComponent<Image>().sprite = completeImg;
					UIManager.instance.menuPanel.CheckStrike();
					hasChange = true;
				}
				catch
				{

				}
			}
		}
		UnLockLevel();
		//if (hasChangelv2)
		//{
		//	UnLockLevel1(2);
		//	for (int i = 0; i < 2; i++)
		//	{
		//		LockLevel(i);
		//	}
		//}
		//else if (hasChangelv1)
		//{
		//	UnLockLevel1(1);
		//	for (int i = 0; i < 1; i++)
		//	{
		//		LockLevel(i);
		//	}
		//}
		//else
		//{
		//	UnLockLevel1(0);
		//}

	}

	

	private void UnLockLevel1(int level)
	{
		try
		{
			Button[] array = listPlayButtons[level];
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] != null)
				{
					if (PlayerPrefs.HasKey(i.ToString()))
					{
						if (PlayerPrefs.GetInt(i.ToString()) == 1)
						{
							array[i].gameObject.SetActive(false);
							if (array[i].GetComponent<LevelButton>().fixedImg != null)
							{
								array[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
								array[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
								try
								{
									if (!completeList.Contains(array[i].gameObject))
									{
										completeList.Add(array[i].gameObject);
									}
								}
								catch { }
								if (unCompleteList.Contains(completeList[i]))
								{
									unCompleteList.Remove(completeList[i]);
								}
							}
						}
					}
				}
			}
		}
		catch { };
	}
	private void UnLockLevel()
	{
		try
		{
			for (int i = 0; i < playButtons.Length; i++)
			{
				if (playButtons[i] != null)
				{
					if (PlayerPrefs.HasKey(i.ToString()))
					{
						if (PlayerPrefs.GetInt(i.ToString()) == 1)
						{
							playButtons[i].gameObject.SetActive(false);
							if (playButtons[i].GetComponent<LevelButton>().fixedImg != null)
							{
								playButtons[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
								playButtons[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
								try
								{
									if (!completeList.Contains(playButtons[i].gameObject))
									{
										completeList.Add(playButtons[i].gameObject);
									}
								}
								catch { }
								if (unCompleteList.Contains(playButtons[i].gameObject))
								{
									unCompleteList.Remove(playButtons[i].gameObject);
								}
							}
						}
					}
				}
			}
		}
		catch { };
		
	}
	private void LockLevel(int level)
	{
		Button[] array = listPlayButtons[level];
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] != null)
			{
				array[i].gameObject.SetActive(false);
				if (array[i].GetComponent<LevelButton>().fixedImg != null)
				{
					array[i].GetComponent<LevelButton>().unfixedImg.SetActive(false);
					array[i].GetComponent<LevelButton>().fixedImg.SetActive(true);
					try
					{
						if (!completeList.Contains(array[i].gameObject))
						{
							completeList.Add(array[i].gameObject);
						}
					}
					catch { }
				}
			}
		}
	}
	public void NextLevel()
	{
		StartCoroutine(nextLevel());
	}
	IEnumerator nextLevel()
	{
		yield return new WaitForSeconds(2);
		UIManager.instance.menuPanel.CheckStrike();
	}
}
