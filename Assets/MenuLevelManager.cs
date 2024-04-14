using DG.Tweening;
using SCN.Common;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuLevelManager : MonoBehaviour
{
	public static MenuLevelManager instance;
	public List<GameObject> levelInstances = new List<GameObject>(); // List to store the instances of loaded levels
	public List<GameObject> levels = new List<GameObject>(); // List to store the instances of loaded levels
	public int levelCount = 0;
	public int CurrentLevel = 0;
	public Vector3 scale;
	public bool hasStarted = false;
	public bool hasChecked = false;
	private void Awake()
	{
		CurrentLevel = SaveSystem.instance.menuLevel;
	}
	public void Start()
	{
		CurrentLevel = SaveSystem.instance.menuLevel;
		if (instance == null)
		{
			instance = this;
		}
		levelCount = levels.Count;
		LoadLevel(CurrentLevel);

	}
	private void OnEnable()
	{
		
	}
	private void checkButtonLevel()
	{
		if (hasChecked == false)
		{
			PlayButton.instance.button.interactable = false;
			for (int i = 0; i < 4; i++)
			{
				UnityEngine.Debug.Log(i);
				if (!PlayerPrefs.HasKey(i.ToString()))
				{
					hasChecked = true;
					UnityEngine.Debug.Log("dell co " + i.ToString());
					PlayButton.instance.level = this.levels[0].GetOrAddComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level;
					UnityEngine.Debug.Log(PlayButton.instance.level);
					PlayButton.instance.button.interactable = true;
					PlayButton.instance.haschange = true;
					return;
				}
			}
		}
	}
	private void Update()
	{
		checkButtonLevel();
	}
	public void LoadLevel(int currentLevel)
	{
		// Load prefab of the new level
		GameObject levelPrefab = levels[currentLevel];

		if (levelPrefab != null)
		{
			try
			{
				GameObject levelInstance = Instantiate(levelPrefab);
				levelInstance.GetComponent<MenuLevel>().menuImg.sprite = levelPrefab.GetComponent<MenuLevel>().menuImg.sprite;
				levelInstance.GetComponent<MenuLevel>().menuImg.transform.SetParent(this.GetComponent<MenuPanel>().bgImg.transform);
				levelInstance.GetComponent<MenuLevel>().menuImg.transform.position = this.GetComponent<MenuPanel>().bgImg.transform.position;
				levelInstance.GetComponent<MenuLevel>().menuImg.GetComponent<Image>().SetNativeSize();
				if (hasStarted == false)
				{
					scale = levelInstance.GetComponent<MenuLevel>().menuImg.transform.localScale;
					hasStarted = true;
				}
				else
				{
					levelInstance.GetComponent<MenuLevel>().menuImg.transform.localScale = scale;
				}
				// Set the level instance as a child of a parent transform if needed
				if (SaveSystem.instance.menuLevel == 0)
				{
					PlayerPrefs.SetInt("hasFlip", 1);
				}
				levelInstance.transform.SetParent(transform);
				if (PlayerPrefs.GetInt("hasFlip") == 0)
				{
					PlayerPrefs.SetInt("hasFlip", 1);
					levelInstance.GetComponent<MenuLevel>().menuImg.transform.position = new Vector3(1139f, 0, 1);
					//this.GetComponent<MenuPanel>().bgImg.transform.GetChild(0).AddComponent<CanvasGroup>();
					//this.GetComponent<MenuPanel>().bgImg.transform.GetChild(0).GetComponent<CanvasGroup>().alpha = 1;
					//this.GetComponent<MenuPanel>().bgImg.transform.GetChild(0).GetComponent<CanvasGroup>().DOFade(0, 0.2f);
					levelInstance.GetComponent<MenuLevel>().menuImg.transform.DOMove(Vector3.zero, 1f).OnComplete(() =>
					{
						for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons.Length; i++)
						{
							levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
							int level = levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level;
							levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
							levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
							levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
						}
						for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons1.Length; i++)
						{
							levelInstance.GetComponent<MenuLevel>().playButtons1[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
							int level = levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<LevelButton>().level;
							levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
							levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
							levelInstance.GetComponent<MenuLevel>().playButtons1[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
						}
						for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().characters.Length; i++)
						{
							levelInstance.GetComponent<MenuLevel>().characters[i].transform.SetParent(this.GetComponent<MenuPanel>().bgImg.transform.GetChild(0).transform);
						}

					});
				}
				else
				{

					for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons.Length; i++)
					{
						levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
						//levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.localPosition = Vector3.zero;

						int level = levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level;
						levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
						levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
						//levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
					}
					for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons1.Length; i++)
					{
						levelInstance.GetComponent<MenuLevel>().playButtons1[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
						int level = levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<LevelButton>().level;
						levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
						levelInstance.GetComponent<MenuLevel>().playButtons1[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
						levelInstance.GetComponent<MenuLevel>().playButtons1[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
					}
					for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().characters.Length; i++)
					{
						levelInstance.GetComponent<MenuLevel>().characters[i].transform.SetParent(this.GetComponent<MenuPanel>().bgImg.transform.GetChild(0).transform);
					}
				}
				// Store the reference to the new level instance
				levelInstances.Add(levelInstance);
				PlayButton.instance.GetComponent<Button>().interactable = true;
			}
			catch { }

		}
		else
		{
			Debug.LogError("Level prefab not found!");
		}
	}

	//InputManager.instance.setHinge();
	public void RemoveLevel(int prelevel)
	{
		for (int i = 0; i < UIManager.instance.menuPanel.buttons.transform.childCount; i++)
		{
			PlayerPrefs.DeleteKey(i.ToString());
			Destroy(UIManager.instance.menuPanel.buttons.transform.GetChild(i).gameObject);
		}
		Destroy(UIManager.instance.menuPanel.bgImg.transform.GetChild(1).gameObject);
		for (int i = 0; i < this.levelInstances[0].GetComponent<MenuLevel>().characters.Length; i++)
		{
			Destroy(this.levelInstances[0].GetComponent<MenuLevel>().characters[i].gameObject);
		}
		// Check if there is a level at the specified index
		if (prelevel >= 0)
		{
			try
			{
				GameObject levelToRemove = levelInstances[0];
				levelInstances.Remove(levelToRemove);
				Destroy(levelToRemove);
			}
			catch { }
		}
	}
	public int getLevel()
	{
		return levelCount;
	}

}
