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

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		levelCount = levels.Count;
		CurrentLevel = SaveSystem.instance.level;
		LoadLevel(CurrentLevel);

	}
	public void LoadLevel(int currentLevel)
	{
		// Load prefab of the new level
		GameObject levelPrefab = levels[currentLevel];

		if (levelPrefab != null)
		{
			// Instantiate prefab as a new instance in the scene
			GameObject levelInstance = Instantiate(levelPrefab, levelPrefab.transform.position, Quaternion.identity);
			levelInstance.GetComponent<MenuLevel>().menuImg.GetComponent<KeepRatio>().SampleRatio = new Vector2(1080f, 1920f);
			levelInstance.GetComponent<MenuLevel>().menuImg.GetComponent<KeepRatio>().SampleRect = this.GetComponentInParent<RectTransform>();
			levelInstance.GetComponent<MenuLevel>().menuImg.transform.SetParent(this.GetComponent<MenuPanel>().bgImg.transform);
			// Set the level instance as a child of a parent transform if needed
			if(SaveSystem.instance.level == 0 )
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
						GameObject bg = UIManager.instance.menuPanel.bgImg.transform.GetChild(0).gameObject;
						Destroy(bg,3f);
						for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons.Length; i++)
						{
							levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
							int level = levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level;
							levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
							levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
							levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
						}
					});
				}
				else
				{
					levelInstance.GetComponent<MenuLevel>().menuImg.transform.position = new Vector3(0, 0, 1);
					levelInstance.GetComponent<MenuLevel>().menuImg.GetComponent<KeepRatio>().SampleRatio = new Vector2(1080f, 1920f);
					levelInstance.GetComponent<MenuLevel>().menuImg.GetComponent<KeepRatio>().SampleRect = this.GetComponentInParent<RectTransform>();
					for (int i = 0; i < levelInstance.GetComponent<MenuLevel>().playButtons.Length; i++)
					{
						levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.SetParent(this.GetComponent<MenuPanel>().buttons.transform);
						int level = levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<LevelButton>().level;
						levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(() => GameManager.instance.SetCurrentLevel(level));
						levelInstance.GetComponent<MenuLevel>().playButtons[i].GetComponent<Button>().onClick.AddListener(this.GetComponent<MenuPanel>().PlayGame);
						levelInstance.GetComponent<MenuLevel>().playButtons[i].transform.position = levelInstance.GetComponent<MenuLevel>().itemPositions[i].transform.position;
					}
				}
			
			// Store the reference to the new level instance
			levelInstances.Add(levelInstance);

		}
		else
		{
			Debug.LogError("Level prefab not found!");
		}

		//InputManager.instance.setHinge();
	}

	public void RemoveLevel(int prelevel)
	{
		for (int i = 0; i < UIManager.instance.menuPanel.buttons.transform.childCount; i++)
		{
			PlayerPrefs.DeleteKey(i.ToString());
			Destroy(UIManager.instance.menuPanel.buttons.transform.GetChild(i).gameObject);
		}
		// Check if there is a level at the specified index
		if (prelevel >= 0)
		{
			GameObject levelToRemove = levelInstances[0];
			levelInstances.Remove(levelToRemove);
			Destroy(levelToRemove);
		}
	}
	public int getLevel()
	{
		return levelCount;
	}

}
