using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.Security.Cryptography;

public class DataLevelManager : MonoBehaviour
{
	public static DataLevelManager Instance;

	private int level;

	[SerializeField]
	private DatatPictureScriptTableObject[] datatPictureScriptTableObjects;

	private string dataName = "data_level";

	private DataLevel dataLevel;

	public DataLevel DataLevel { get => dataLevel; set => dataLevel = value; }
	public DatatPictureScriptTableObject[] DatatPictureScriptTableObjects { get => datatPictureScriptTableObjects; }

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	public void Init()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		string dataString = PlayerPrefs.GetString(dataName);

		if (string.IsNullOrEmpty(dataString))
		{
			CreateNewData();
			SaveData();
		}
		else
		{
			try
			{
				dataLevel = JsonConvert.DeserializeObject<DataLevel>(dataString);
				if (dataLevel == null)
				{
					CreateNewData();
					SaveData();
				}
				else
				{
					RepairData();
				}
			}
			catch
			{
				CreateNewData();
				SaveData();
			}
		}
	}

	private void CreateNewData()
	{
		dataLevel = new DataLevel();
		dataLevel.Data = new DataLevelChil[datatPictureScriptTableObjects.Length];
		for (int i = 0; i < datatPictureScriptTableObjects.Length; i++)
		{
			dataLevel.Data[i] = new DataLevelChil();
			dataLevel.Data[i].IndexStage = 0;
			dataLevel.Data[i].Stage = new DataStageLevel[datatPictureScriptTableObjects[i].Stage.Length];
			for (int j = 0; j < dataLevel.Data[i].Stage.Length; j++)
			{
				dataLevel.Data[i].Stage[j] = new DataStageLevel();
				dataLevel.Data[i].Stage[j].DataItmeLevel = new DataItmeLevel[datatPictureScriptTableObjects[i].Stage[j].Item.Length];
				for (int k = 0; k < dataLevel.Data[i].Stage[j].DataItmeLevel.Length; k++)
				{
					dataLevel.NumOfLevel++;
					dataLevel.Data[i].Stage[j].DataItmeLevel[k] = new DataItmeLevel();
					dataLevel.Data[i].Stage[j].DataItmeLevel[k].Id = datatPictureScriptTableObjects[i].Stage[j].Item[k].Id;
					dataLevel.Data[i].Stage[j].DataItmeLevel[k].IsUnlock = false;
					dataLevel.Data[i].Stage[j].DataItmeLevel[k].IndexSelect = 0;
				}
			}
		}
	}

	private void RepairData()
	{

	}

	public void SaveData()
	{
		PlayerPrefs.SetString(dataName, JsonConvert.SerializeObject(dataLevel));
	}

	public int GetLevel()
	{
		for (int i = 0; i < dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage[dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].DataItmeLevel.Length; i++)
		{
			if (!dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage[dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].DataItmeLevel[i].IsUnlock)
			{
				return i;
			}
		}

		return 0;
	}
	public void SetLevelDone(int i)
	{
		dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage[dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].DataItmeLevel[i].IsUnlock = true;
		LevelManagerNew.Instance.LevelBase.CountLevelWin += 1;
		SaveData();
		LevelManagerNew.Instance.SaveData();
	}
	public void ResetData()
	{
		for(int i = 0;i < dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage.Length; i++)
		{
			for(int j = 0;j < dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage[i].DataItmeLevel.Length;j++)
			{
				dataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].Stage[i].DataItmeLevel[j].IsUnlock = false;
			}
		}
		DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage = 0;
		LevelManagerNew.Instance.LevelBase.CountLevelWin = 0;
		SaveData();
		LevelManagerNew.Instance.SaveData();
	}
}

[Serializable]
public class DataLevel
{
	[SerializeField]
	private DataLevelChil[] data;

	[SerializeField]
	private int numOfLevel;
	public int NumOfLevel { get => numOfLevel; set => numOfLevel = value; }

	public DataLevelChil[] Data { get => data; set => data = value; }
}

[Serializable]
public class DataLevelChil
{
	[SerializeField]
	private int indexStage;
	[SerializeField]
	private DataStageLevel[] stage;
	
	public int IndexStage { get => indexStage; set => indexStage = value; }
	public DataStageLevel[] Stage { get => stage; set => stage = value; }
}
[Serializable]
public class DataStageLevel
{
	[SerializeField]
	private DataItmeLevel[] dataItmeLevel;

	public DataItmeLevel[] DataItmeLevel { get => dataItmeLevel; set => dataItmeLevel = value; }
}

[Serializable]
public class DataItmeLevel
{
	
	[SerializeField]
	private int id;
	[SerializeField]
	private bool isUnlock;
	[SerializeField]
	private int indexSelect;

	public int Id { get => id; set => id = value; }
	public bool IsUnlock { get => isUnlock; set => isUnlock = value; }
	public int IndexSelect { get => indexSelect; set => indexSelect = value; }
	
}
