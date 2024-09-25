using DG.Tweening;
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

	public bool hasLoadDone = false;

	public int displayLevel = 0;

	public LevelBase LevelBase { get => levelBase; set => levelBase = value; }

	public List<Stage> stageList = new List<Stage>();

	public List<LevelStage> stageList1 = new List<LevelStage>();

	public List<Stage> testingStageList = new List<Stage>();

	public List<MiniGameStage> miniStageList = new List<MiniGameStage>();

	public int stage;
	private void Awake()
	{
		Instance = this;
		stage = PlayerPrefs.GetInt("stage");
		displayLevel = PlayerPrefs.GetInt("displayLevel");
		Debug.LogError("stage " + stage);
		if(displayLevel < stage)
		{
			displayLevel = stage;
		}
		//test
		//stage = 5;
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
		hasLoadDone = true;
	}

	public void SaveData()
	{
		PlayerPrefs.SetString(dataName, JsonConvert.SerializeObject(levelBase));
	}
	public void NetxtLevel()
	{
		if (levelBase.Level + 1 >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{
            PlayerPrefs.SetInt("CompleteLastPic", 1);
            ResetLevel(() =>
            {
                UIManagerNew.Instance.CongratPanel.Close();
                UIManagerNew.Instance.CompleteImg.Disablepic();
                if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
                {
                    UIManagerNew.Instance.ButtonMennuManager.Appear();
                }
            });
        }
		else
		{
			Debug.Log(levelBase.Level);
			levelBase.Level++;
			levelBase.CountLevelWin = 0;
			SaveData();
		}

	}
	public void CheckForNewPic()
	{
		if (levelBase.Level + 1 >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{

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
	public void NetxtLevelForNewPic()
	{
		if (levelBase.Level + 1 >= DataLevelManager.Instance.DatatPictureScriptTableObjects.Length)
		{
			return;
		}
		else
		{
			Debug.Log(levelBase.Level);
			levelBase.Level++;
			levelBase.CountLevelWin = 0;
			SaveData();
		}
	}
	public int GetStage()
	{
		if (stage > stageList.Count)
		{
			stage = stageList.Count - 1;
		}
		return stage;
	}
	public void SaveStage()
	{
		PlayerPrefs.SetInt("stage", stage);
		PlayerPrefs.SetInt("displayLevel", displayLevel);
	}
	public int NextStage()
	{
		if (stage + 1 > stageList.Count)
		{
			//stage = stageList.Count - 1;
			if(displayLevel == 0)
			{
				displayLevel = stageList.Count;
			}
			else
			{
				displayLevel++;
			}
		}
		else
		{
			stage++;
			displayLevel = stage;

        }
		SaveStage();
		Debug.Log(stage);
		return stage;
	}


	public void RepeatLevel()
	{
        //if (GameManagerNew.Instance.CheckLevelStage())
        //{
            int replayLevel = UnityEngine.Random.Range(10, LevelManagerNew.Instance.stageList.Count-1);
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.7f, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                GameManagerNew.Instance.CreateLevel(replayLevel);
            });
        //}
        //else
        //{
        //    LevelManagerNew.Instance.ResetLevel(() =>
        //    {
        //        UIManagerNew.Instance.CongratPanel.Close();
        //        UIManagerNew.Instance.CompleteImg.Disablepic();
        //        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        //        {
        //            UIManagerNew.Instance.ButtonMennuManager.Appear();
        //        }
        //    });
        //}
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

[Serializable]
public class LevelStage
{
	[SerializeField]
	private int level;
	[SerializeField]
	private Stage Stage;

	public int Level { get => level; set => level = value; }
	public Stage Stage1 { get => Stage; set => Stage = value; }
}
