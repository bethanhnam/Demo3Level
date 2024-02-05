using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Level : MonoBehaviour
{
	public static Level instance;
	public StageManager stageManager;


	public int stage = 0;

	private void Start()
	{
		if(instance == null)
		{
			instance = this;
		}
		LoadStage(stage);
		UIManager.instance.gamePlayPanel.level.Lv1.sprite = UIManager.instance.gamePlayPanel.level.done;
		UIManager.instance.gamePlayPanel.level.Lv2.sprite = UIManager.instance.gamePlayPanel.level.notDone;
	}
	public void CheckLevel()
	{
		stageManager.RemoveLevel(stage);
		if (stage >= stageManager.levels.Count)
		{
			UIManager.instance.chestPanel.Open();
			UIManager.instance.gamePlayPanel.level.Lv1.sprite = UIManager.instance.gamePlayPanel.level.notDone;
			UIManager.instance.gamePlayPanel.level.Lv2.sprite = UIManager.instance.gamePlayPanel.level.notDone;
		}
		else
		{
			LoadStage(stage);
			UIManager.instance.gamePlayPanel.level.Lv2.sprite = UIManager.instance.gamePlayPanel.level.done;
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
		stageManager.LoadStage(stage);
		this.stage++;

	}
}
