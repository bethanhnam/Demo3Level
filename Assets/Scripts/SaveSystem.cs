using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level;
    public int magicTiket;
    public int powerTicket;
    public int days = 0;
    public float playHardTime = 0;
    public bool playingHard;
    public int nonAds =0;
    public int strike =0;
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
		if (playingHard == true)
		{
			playHardTime += Time.deltaTime;
		}
	}
	public void SaveData()
    {
		PlayerPrefs.SetInt("magicTiket", magicTiket);
        PlayerPrefs.SetInt("powerTicket", powerTicket);
        PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.SetInt("Days", days);
		PlayerPrefs.SetInt("NonADS", nonAds);
		PlayerPrefs.SetInt("Strike", strike);
	}
    public void CreateData()
    {
        magicTiket = 100;
        powerTicket = 100;
    }
    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
		magicTiket = PlayerPrefs.GetInt("magicTiket");
		powerTicket = PlayerPrefs.GetInt("powerTicket");
		days = PlayerPrefs.GetInt("Days");
        nonAds = PlayerPrefs.GetInt("NonADS");
        strike = PlayerPrefs.GetInt("Strike");
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
	}
	public void SetLevel(int level)
    {
        this.level = level;
    }
}
