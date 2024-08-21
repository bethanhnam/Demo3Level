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
        DontDestroyOnLoad(this.gameObject);

        levelManagerNew.Init();
        dataLevelManager.Init();

        if (instance == null)
        {
            instance = this;
        }
        if (string.IsNullOrEmpty(PlayerPrefs.GetString("firstOpenDate")))
        {
            PlayerPrefs.SetString("firstOpenDate", System.DateTime.Now.Date.Ticks.ToString());
        }
    }
}
