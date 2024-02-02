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
	public int targetFPS = 60;
	public int numOfTicket;
	public bool deleting;
	public bool hasUI;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFPS;
		
	}
	private void OnEnable()
	{
		StartCoroutine(LoadLevel());
	}
	private void OnDisable()
	{
		if (LevelManager.instance.transform.GetChild(0) != null)
		{
			Destroy(LevelManager.instance.transform.GetChild(0).gameObject);
			LevelManager.instance.levelInstances.Clear();
		}
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
					//hiện win pop
					//Invoke("NextLevel",0.2f);
					return;

				}
				Invoke("NextLevel", 0.2f);
			UIManager.instance.gamePlayPanel.Settimer();
		}

	}
	public void NextLevel()
	{
		checkLevel = 0;
		StartCoroutine(LoadLevel());
	}
	public void Replay()
	{
		if (LevelManager.instance.transform.childCount > 0)
		{
			if (LevelManager.instance.transform.GetChild(0) != null)
			{
				Destroy(LevelManager.instance.transform.GetChild(0).gameObject);
				LevelManager.instance.levelInstances.Clear();
				StartCoroutine(LoadLevel());
				UIManager.instance.gamePlayPanel.Settimer();
			}
		}
		
	}
	 public IEnumerator LoadLevel()
	{
		yield return new WaitForSeconds(0.3f);
		levelManager.LoadLevel(currentLevel);
	}
}
