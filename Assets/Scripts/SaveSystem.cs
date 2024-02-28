using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level =0;
    public int purpleStar = 10;
    public int goldenStar = 10;
    public int days = 0;
    public float playTime = 0;
    public bool playing;
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		LoadData();
		SaveData();
        playing = true;
	}
	private void Start()
	{
		
	}
	private void Update()
	{
        if (playing == true)
        {
            playTime += Time.deltaTime;
        }
	}
	public void SaveData()
    {
		PlayerPrefs.SetFloat("TimePlay", playTime);
		PlayerPrefs.SetInt("PurpleStar", purpleStar);
        PlayerPrefs.SetInt("GoldenStar", goldenStar);
        PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.SetInt("Days", days);
	}
    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
		purpleStar = PlayerPrefs.GetInt("PurpleStar");
		goldenStar = PlayerPrefs.GetInt("GoldenStar");
		days = PlayerPrefs.GetInt("Days");
        playTime = PlayerPrefs.GetFloat("TimePlay");
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
