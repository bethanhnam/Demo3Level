using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerNew : MonoBehaviour
{
	public static LevelManagerNew Instance;

	private string dataName = "data_level_base";

	private LevelBase levelBase;

	public LevelBase LevelBase { get => levelBase; set => levelBase = value; }

	private void Awake()
	{
		Instance = this;
	}

	public void Init()
	{
		string dataString = PlayerPrefs.GetString(dataName);
		if (string.IsNullOrEmpty(dataString))
		{
			levelBase = new LevelBase();
			levelBase.Level = 0;
			levelBase.CountLevelWin = 0;
			SaveData();
		}
		else
		{
			try
			{
				levelBase = JsonConvert.DeserializeObject<LevelBase>(dataString);
				if (levelBase == null)
				{
					levelBase = new LevelBase();
					levelBase.Level = 0;
					levelBase.CountLevelWin = 0;
					SaveData();
				}
			}
			catch
			{
				levelBase = new LevelBase();
				levelBase.Level = 0;
				levelBase.CountLevelWin = 0;
				SaveData();
			}
		}
	}

	public void SaveData()
	{
		PlayerPrefs.SetString(dataName, JsonConvert.SerializeObject(levelBase));
	}
	public void NetxtLevel()
	{
		if(levelBase.Level+1 >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{
			UIManagerNew.Instance.ButtonMennuManager.OpenCompletePanel();
		}
		else
		{
			levelBase.Level++;
			levelBase.CountLevelWin = 0;
			SaveData();
		}
		
	}
	public void ResetLevel()
	{
		DataLevelManager.Instance.ResetData();
		levelBase.CountLevelWin = 0;
		SaveData();
	}
	public void NextStage()
	{
		DataLevelManager.Instance.DataLevel.Data[levelBase.Level].IndexStage++;
		SaveData();
		DataLevelManager.Instance.SaveData();
	}
	
}

[Serializable]
public class LevelBase
{
	[SerializeField]
	private int level;
	[SerializeField]
	private int countLevelWin;

	public int Level { get => level; set => level = value; }
	public int CountLevelWin { get => countLevelWin; set => countLevelWin = value; }
}
