using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level =0;
	private void Start()
	{
		if(instance == null)
        {
            instance = this;
        }
        LoadData();
	}
	public void SaveData()
    {
        PlayerPrefs.SetInt("Level", level);
    }
    public int LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
        return level;
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }
}
