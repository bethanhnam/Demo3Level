using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level =0;
    public int purpleStar = 0;
    public int goldenStar = 0;
    public int days = 0;
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
        PlayerPrefs.SetInt("BlueTicket", purpleStar);
        PlayerPrefs.SetInt("SilverTicket", goldenStar);
        PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.SetInt("Days", days);
	}
    public int LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
		purpleStar = PlayerPrefs.GetInt("BlueTicket");
		goldenStar = PlayerPrefs.GetInt("SilverTicket");
		days = PlayerPrefs.GetInt("Days");
		return level;
    }
    public int GetBlueTicket()
    {
        return purpleStar;
    }
    public int GetSilverTicket()
    {
        return goldenStar;
    }
    public void SetTiket(int blueTicket,int silverTicket)
    {
        this.purpleStar = blueTicket;
        this.goldenStar = silverTicket;
    }
    public void SetLevel(int level)
    {
        this.level = level;
    }
}
