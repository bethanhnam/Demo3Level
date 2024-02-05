using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
	public static Level instance;
    public GameObject[] levels;

    public int stage = 0;

	private void Start()
	{
		if(instance == null)
		{
			instance = this;
		}
		LoadStage(stage);
	}
	public void CheckLevel()
	{
		StageManager.instance.RemoveLevel(stage);
		UIManager.instance.chestPanel.Open();
		if (stage >= levels.Length)
		{
			GameManager.instance.LoadLevel();

		}
	}
	public void LoadStage(int stage)
	{
		UIManager.instance.gamePlayPanel.ButtonOff();
		UIManager.instance.gamePlayPanel.ButtonOn();
		try
		{
			InputManager.instance.hasSave = false;
		}
		catch
		{

		}
		StageManager.instance.LoadStage(stage);

	}
	public void NextStage()
	{
		stage++;
		LoadStage(stage);
	}
}
