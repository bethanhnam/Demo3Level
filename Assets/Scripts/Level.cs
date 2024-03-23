using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
	public static Level instance;
	public StageManager stageManager;
	public string layerName = "Hole";


	public int stage = 0;

	private void Start()
	{
		if (instance == null)
		{
			instance = this;
		}
		stage = 0;
		LoadStage(stage);
		UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(false);
	}
	public void CheckLevel()
	{

		stageManager.RemoveLevel(stage);
		if (stage >= stageManager.levels.Count)
		{
			{
				if (GameManager.instance.currentLevel == 2)
				{
					UIManager.instance.gamePlayPanel.OpenRatingPanel();
				}
				else
				{
					SaveSystem.instance.playingHard = false;
					UIManager.instance.winPanel.Open();
					UIManager.instance.DeactiveTime();
					UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
					UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(true);
					UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			StartCoroutine(LoadHardLevel());
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
		SaveSystem.instance.SaveData();

	}
	IEnumerator LoadHardLevel()
	{
		ClosePopUp();
		if (GameManager.instance.currentLevel == 0)
		{
			if (stage == 1)
			{
				UIManager.instance.gamePlayPanel.OpenWelcomePanel();
			}
			else
			{
				UIManager.instance.gamePlayPanel.OpenHardPanel();
				AudioManager.instance.PlaySFX("Warning");
			}
		}
		else
		{
			UIManager.instance.gamePlayPanel.OpenHardPanel();
		}
		yield return new WaitForSeconds(1.5f);
		if (GameManager.instance.currentLevel == 0)
		{
			if (stage == 1)
			{
				UIManager.instance.gamePlayPanel.welcomeLevel.Close();
			}
			else
			{
				UIManager.instance.gamePlayPanel.hardLevel.Close();
			}
		}
		else
		{
			UIManager.instance.gamePlayPanel.hardLevel.Close();
		}
		
		LoadStage(stage);
		UIManager.instance.gamePlayPanel.level.done.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.notDone.gameObject.SetActive(true);
		UIManager.instance.gamePlayPanel.level.levelBar.gameObject.SetActive(true);
		SaveSystem.instance.playingHard = true;
		SaveSystem.instance.playHardTime = 0;

	}

	private static void ClosePopUp()
	{
		UIManager.instance.gamePlayPanel.extraHolePanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.deteleNailPanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.rePlayPanel.gameObject.SetActive(false);
		UIManager.instance.gamePlayPanel.undoPanel.gameObject.SetActive(false);
	}

	public void ChangeLayer()
	{
		int layer = LayerMask.NameToLayer(layerName); // Chuyển đổi tên layer thành ID layer
		if (layer != -1) // Kiểm tra xem layer có tồn tại không
		{
			if (stageManager.levelInstances[0].GetComponent<Stage>().holeToUnlock.gameObject != null)
			{
				stageManager.levelInstances[0].GetComponent<Stage>().holeToUnlock.gameObject.layer = layer; // Đặt layer cho GameObject

			}
		}
		else
		{
			Debug.LogError("Layer " + layerName + " does not exist!"); // In ra thông báo lỗi nếu layer không tồn tại
		}
	}
}