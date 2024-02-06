using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
		
	}
		IEnumerator LoadHardLevel()
		{
			UIManager.instance.gamePlayPanel.hardLevel.Open();
			yield return new WaitForSeconds(1f);
			UIManager.instance.gamePlayPanel.hardLevel.Close();
			LoadStage(stage);
			UIManager.instance.gamePlayPanel.level.Lv2.sprite = UIManager.instance.gamePlayPanel.level.done;
		}
	public void ChangeLayer()
	{
		int layer = LayerMask.NameToLayer(layerName); // Chuyển đổi tên layer thành ID layer
		if (layer != -1) // Kiểm tra xem layer có tồn tại không
		{
			stageManager.levelInstances[0].GetComponent<Stage>().holeToUnlock.gameObject.layer = layer; // Đặt layer cho GameObject
		}
		else
		{
			Debug.LogError("Layer " + layerName + " does not exist!"); // In ra thông báo lỗi nếu layer không tồn tại
		}
	}
}