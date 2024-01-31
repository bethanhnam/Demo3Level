using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;
	public int currentLevel = 0;
	public int checkLevel = 0;
	public bool endgame = false;
	public LevelManager levelManager;
	public int targetFPS = 30;
	public int numOfTicket;
	public bool deleting;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFPS;
	}

	private void Update()
	{

	}
	public void CheckLevel()
	{
			levelManager.RemoveLevel(currentLevel);
			if (checkLevel == 0)
			{
				currentLevel++;
				checkLevel++;
				if (currentLevel >= levelManager.levelCount)
				{
					currentLevel = 0;
					Invoke("NextLevel",0.2f);
					return;

				}
				Invoke("NextLevel", 0.2f);
		}

	}
	public void NextLevel()
	{
		checkLevel = 0;
		levelManager.LoadLevel(currentLevel);
	}
	public void DeteleNail()
	{
		// dừng đồng hồ đếm lại
		
	}
}
