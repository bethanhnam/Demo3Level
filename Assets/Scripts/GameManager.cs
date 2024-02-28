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
	
	public int purpleStar;
	public int goldenStar;
	public bool deleting;
	public bool hasUI;
	public bool hasMove;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		currentLevel = SaveSystem.instance.level;
	}
	private void OnEnable()
	{
		try
		{
			purpleStar = SaveSystem.instance.GetPurpleStar();
			goldenStar = SaveSystem.instance.GetGoldenStar();
		}
		catch
		{

		};
	}
	private void Update()
	{

	}
	private void OnApplicationQuit()
	{
		SaveSystem.instance.SaveData();
	}
	//public void CheckLevel()
	//{
	//	levelManager.RemoveLevel(currentLevel);
	//	UIManager.instance.chestPanel.Open();
	//	if (currentLevel >= levelManager.levelCount)
	//	{
	//		currentLevel = 0;
	//		//hiện win pop
	//		return;

	//	}
	//}
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
				UIManager.instance.gamePlayPanel.deteleNailPanel.hasUse = false;
				UIManager.instance.gamePlayPanel.undoPanel.hasUse = false;
			}
		}

	}
	public void LoadLevelFromUI()
	{
		StartCoroutine(LoadLevel());
	}
	public IEnumerator LoadLevel()
	{
		UIManager.instance.gamePlayPanel.ButtonOff();
		yield return new WaitForSeconds(0.5f);
		UIManager.instance.gamePlayPanel.ButtonOn();
		try
		{
			InputManager.instance.hasSave = false;
		}
		catch
		{

		}
		levelManager.LoadLevel(currentLevel);

	}

}
