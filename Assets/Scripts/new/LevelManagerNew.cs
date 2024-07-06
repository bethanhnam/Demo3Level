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

	public List<Stage> stageList = new List<Stage>();

	public int stage;
    private void Awake()
	{
		Instance = this;
		stage = PlayerPrefs.GetInt("stage");
		//test
		//stage = 24;
    }

	public void Init()
	{
		string dataString = PlayerPrefs.GetString(dataName);
		if (string.IsNullOrEmpty(dataName))
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
				//dataString = "{\"Level\":4,\"CountLevelWin\":0}";
                //levelBase = JsonConvert.DeserializeObject<LevelBase>(dataString);
                Debug.Log(dataString);
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
			Debug.Log(levelBase.Level);
			levelBase.Level++;
			levelBase.CountLevelWin = 0;
			SaveData();
		}
		
	}
	public void ResetLevel(Action action)
	{
		
		SaveData();
		action();
	}
	public void NextPicStage()
	{
		DataLevelManager.Instance.DataLevel.Data[levelBase.Level].IndexStage++;
		SaveData();
		DataLevelManager.Instance.SaveData();
	}
	public int GetStage()
	{
		if(stage > stageList.Count)
		{
			stage = stageList.Count - 1;
		}
		return stage;
	}
	public void SaveStage()
	{
		PlayerPrefs.SetInt("stage", stage);
	}
	public int NextStage()
	{
        if (stage + 1 > stageList.Count)
        {
            stage = stageList.Count - 1;
        }
		else
		{
			stage++;
		}
        SaveStage();
		Debug.Log(stage);
        return stage;
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
