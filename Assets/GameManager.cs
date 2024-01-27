using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public LevelManager levelLoader;
	public int currentLevel = 0;
	public int checkLevel = 0;
	public bool endgame = false;
	public LevelManager levelManager;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		// Load level from prefab
		levelLoader.LoadLevel(currentLevel);
	}

	private void Update()
	{

	}
	public void CheckLevel()
	{
		if (InputManager.instance.ironObjects.Length <= 0)
		{
			levelLoader.RemoveLevel(currentLevel);
			if (checkLevel == 0)
			{
				currentLevel++;
				checkLevel++;
				if (currentLevel >= levelManager.levelCount)
				{
					currentLevel = 0;
				}
				Invoke("NextLevel", 1f);
			}
		}

	}
	public void NextLevel()
	{
		checkLevel = 0;
		levelLoader.LoadLevel(currentLevel);
	}
}
