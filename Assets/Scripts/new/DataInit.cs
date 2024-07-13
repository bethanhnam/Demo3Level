using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataInit : MonoBehaviour
{
	public static DataInit instance;
	[SerializeField]
	public DataLevelManager dataLevelManager;
	[SerializeField]
	public LevelManagerNew levelManagerNew;

	private void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

		if (PlayerPrefs.GetInt("CompleteLastPic") == 1)
		{
			dataLevelManager.RepairData();
			dataLevelManager.SaveData();
            Debug.Log("chayj vaof repairdata");
        }
		else
		{
			levelManagerNew.Init();
			dataLevelManager.Init();
            Debug.Log("chayj vaof init bth");
        }

		if (instance == null)
		{
			instance = this;
		}
	}
}
