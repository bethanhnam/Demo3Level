using DG.Tweening;
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

    private string firstOpenDate = "firstOpenDate";


    private string isBuyRemoveAds = "isBuyRemoveAds";
    private string isBuyRemoveAdsPack = "isBuyRemoveAdsPack";

    public string FirstOpenDate { get => firstOpenDate; }
    public string IsBuyRemoveAds { get => isBuyRemoveAds; }
    public string IsBuyRemoveAdsPack { get => isBuyRemoveAdsPack; }

    private void Awake()
    {
        
    }
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        levelManagerNew.Init();
         
        if (instance == null)
        {
            instance = this;
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("firstOpenDate")))
        {
            PlayerPrefs.SetString("firstOpenDate", System.DateTime.UtcNow.Date.Ticks.ToString());
        }

        DOVirtual.DelayedCall(2, () => {
            dataLevelManager.Init();
        });

        LoadingScreen.instance.LoadSceneDone();
    }
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("chạy vào pause");
        if (pause)
        {
            Debug.Log("chạy vào pause 1");
            //if (weeklyEvent != null)
            //{
            //    SaveData("WeeklyEvent", weeklyEvent);
            //}
            //if (weeklyEventTreasureClimb != null)
            //{
            //    SaveData("TreasureClimb", weeklyEventTreasureClimb);
            //}
            //if (weeklyEventHauntedTreasure != null)
            //{
            //    SaveData("HauntedTreasure", weeklyEventHauntedTreasure);
            //}
        }
    }
}
