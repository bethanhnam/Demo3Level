using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;
    public int level =13;
    public int magicTiket;
    public int powerTicket;
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
		PlayerPrefs.SetInt("magicTiket", magicTiket);
        PlayerPrefs.SetInt("powerTicket", powerTicket);
        PlayerPrefs.SetInt("Level", level);
		PlayerPrefs.SetInt("Days", days);
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
		this.powerTicket += powerTicket1;
		this.magicTiket += magicTiket1;
	}
	public void SetLevel(int level)
    {
        this.level = level;
    }
}
