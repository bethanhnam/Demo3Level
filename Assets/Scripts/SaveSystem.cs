using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level =13;
    public int purpleStar;
    public int goldenStar;
    public int days = 0;
    public float playHardTime = 0;
    public bool playingHard;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		LoadData();
        CreateData();
		SaveData();
	}
	private void Start()
	{
		
	}
	private void Update()
	{
		if (playingHard == true)
		{
			playHardTime += Time.deltaTime;
		}
	}
	public void SaveData()
    {
		PlayerPrefs.SetInt("PurpleStar", purpleStar);
        PlayerPrefs.SetInt("GoldenStar", goldenStar);
        PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.SetInt("Days", days);
	}
    public void CreateData()
    {
        purpleStar = 100;
        goldenStar = 100;
 
	}
    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
		purpleStar = PlayerPrefs.GetInt("PurpleStar");
		goldenStar = PlayerPrefs.GetInt("GoldenStar");
		days = PlayerPrefs.GetInt("Days");
    }
    public int GetPurpleStar()
    {
        return purpleStar;
    }
    public int GetGoldenStar()
    {
        return goldenStar;
    }
    public void SetTiket(int goldenStar,int purpleStar)
    {
        this.goldenStar = goldenStar;
        this.purpleStar = purpleStar;
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }
}
