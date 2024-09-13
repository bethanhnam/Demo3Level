//using DG.Tweening;
//using SCN.Common;
//using Sirenix.Utilities;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.UI;

//public class MenuLevelManager : MonoBehaviour
//{
//	public static MenuLevelManager instance;
//	public List<GameObject> levelInstances = new List<GameObject>(); // List to store the instances of loaded levels
//	public List<GameObject> levels = new List<GameObject>(); // List to store the instances of loaded levels
//	public int levelCount = 0;
//	public int CurrentLevel = 0;
//	public Vector3 scale;
//	public bool hasStarted = false;
//	public bool hasChecked = false;

//	public PictureUIManager pictureUIManager;

//	private void Awake()
//	{
//	}
//	public void Start()
//	{
//		CurrentLevel = SaveSystem.instance.menuLevel;
//		if (instance == null)
//		{
//			instance = this;
//		}
//		levelCount = levels.Count;
//		LoadLevel(CurrentLevel);

//	}
//	private void OnEnable()
//	{
		
//	}
//	private void checkButtonLevel()
//	{
//		//if (UIManager.instance.menuPanel.buttons.transform.childCount > 0)
//		//{
//		//	for (int i = 0; i < UIManager.instance.menuPanel.buttons.transform.childCount; i++)
//		//	{
//		//		if (UIManager.instance.menuPanel.buttons.transform.GetChild(i).gameObject.activeSelf != false)
//		//		{
//		//			PlayButton.instance.level = UIManager.instance.menuPanel.buttons.transform.GetChild(i).GetComponent<LevelButton>().level;
//		//			PlayButton.instance.haschange = true;
//		//			return;
//		//		}
//		//	}
//		//}
//	}
//	private void Update()
//	{
//		checkButtonLevel();
//	}
//	public void LoadLevel(int currentLevel)
//	{
//		pictureUIManager = Instantiate(DataLevelManager.Instance.DatatPictureScriptTableObjects[currentLevel].PictureUIManager, SaveSystem.instance.transform);
//		pictureUIManager.transform.SetSiblingIndex(0);
//		// Load prefab of the new level
//	}

//	//InputManager.instance.setHinge();
//	public void RemoveLevel(int prelevel)
//	{
//		PlayerPrefs.SetInt("HasDone",0);
//		for (int i = 0; i < UIManager.instance.menuPanel.buttons.transform.childCount; i++)
//		{
//			PlayerPrefs.DeleteKey(i.ToString());
//			UnityEngine.Debug.Log("detele " + i.ToString());
//			Destroy(UIManager.instance.menuPanel.buttons.transform.GetChild(i).gameObject);
//		}
//		Destroy(UIManager.instance.menuPanel.bgImg.transform.GetChild(1).gameObject);
//		for (int i = 0; i < this.levelInstances[0].GetComponent<MenuLevel>().characters.Length; i++)
//		{
//			Destroy(this.levelInstances[0].GetComponent<MenuLevel>().characters[i].gameObject);
//		}
//		// Check if there is a level at the specified index
//		if (prelevel >= 0)
//		{
//			try
//			{
//				GameObject levelToRemove = levelInstances[0];
//				levelInstances.Remove(levelToRemove);
//				Destroy(levelToRemove);
//			}
//			catch { }
//		}
//	}
//	public int getLevel()
//	{
//		return levelCount;
//	}
//	public void DisableLevel()
//	{
//		pictureUIManager.gameObject.SetActive(false);	
//	}

//}
