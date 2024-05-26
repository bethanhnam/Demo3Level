using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level;
    public int menuLevel;
    public int magicTiket;
    public int powerTicket;
    public int days = 0;
    public float playHardTime = 0;
    public bool playingHard;
    public int nonAds =0;
    public int strike =0;
    public int coin;
    public int star;
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
		if(nonAds > 0)
        {
            AdsManager.instance.isRemoveAds = true;
        }
	}
	private void Update()
	{

	}
	public void SaveData()
    {
		PlayerPrefs.SetInt("magicTiket", magicTiket);
        PlayerPrefs.SetInt("powerTicket", powerTicket);
        PlayerPrefs.SetInt("Level", level);
        PlayerPrefs.SetInt("MenuLevel", menuLevel);
		PlayerPrefs.SetInt("Days", days);
		PlayerPrefs.SetInt("NonADS", nonAds);
		PlayerPrefs.SetInt("Strike", strike);
		PlayerPrefs.SetInt("Coin", coin);
		PlayerPrefs.SetInt("Star", star);
	}
    public void CreateData()
    {
        magicTiket = 1000;
        powerTicket = 1000;
        coin = 1000;
        star = 50;
        days = 6;
    }
    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
        menuLevel = PlayerPrefs.GetInt("MenuLevel");
		magicTiket = PlayerPrefs.GetInt("magicTiket");
		powerTicket = PlayerPrefs.GetInt("powerTicket");
		days = PlayerPrefs.GetInt("Days");
        nonAds = PlayerPrefs.GetInt("NonADS");
        strike = PlayerPrefs.GetInt("Strike");
        coin = PlayerPrefs.GetInt("Coin");
        star = PlayerPrefs.GetInt("Star");
	}
    public int GetmagicTiket()
    {
        return magicTiket;
    }
    public int GetpowerTicket()
    {
        return powerTicket;
    }
    public void SetTiket(int powerTicket,int magicTiket)
    {
        this.powerTicket = powerTicket;
        this.magicTiket = magicTiket;
    }
	public void addTiket(int powerTicket1, int magicTiket1)
	{
        AudioManager.instance.PlaySFX("Coins");
		this.powerTicket += powerTicket1;
		this.magicTiket += magicTiket1;
        SaveData();
	}
	public void SetLevel(int level)
    {
        this.menuLevel = level;
    }
    public void addCoin(int coin)
    {
        this.coin += coin;
    }
    public void addStar(int star)
    {
        this.star += star;
    }

}
