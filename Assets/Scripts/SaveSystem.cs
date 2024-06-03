using DG.Tweening;
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
    public int nonAds = 0;
    public int coin;
    public int star;
    public int unscrewPoint;
    public int undoPoint;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        LoadData();
        CreateData();
        transfer();
        SaveData();
    }
    private void Start()
    {
        if (nonAds > 0)
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
        
        PlayerPrefs.SetInt("Coin", coin);
        PlayerPrefs.SetInt("Star", star);
        PlayerPrefs.SetInt("unscrewPoint", unscrewPoint);
        PlayerPrefs.SetInt("undoPoint", undoPoint);

        
    }
    public void CreateData()
    {
        //magicTiket = 10;
        //powerTicket = 10;
        coin = 1000;
        star = 10;
        //days =7;
    }
    public void LoadData()
    {
        level = PlayerPrefs.GetInt("Level");
        menuLevel = PlayerPrefs.GetInt("MenuLevel");
        magicTiket = PlayerPrefs.GetInt("magicTiket");
        powerTicket = PlayerPrefs.GetInt("powerTicket");
        days = PlayerPrefs.GetInt("Days");
        nonAds = PlayerPrefs.GetInt("NonADS");
  
        coin = PlayerPrefs.GetInt("Coin");
        star = PlayerPrefs.GetInt("Star");
        unscrewPoint = PlayerPrefs.GetInt("unscrewPoint");
        undoPoint = PlayerPrefs.GetInt("undoPoint");
    }
    public int GetmagicTiket()
    {
        return magicTiket;
    }
    public int GetpowerTicket()
    {
        return powerTicket;
    }
    public void SetTiket(int powerTicket, int magicTiket)
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
        UIManagerNew.Instance.LoadData(SaveSystem.instance.magicTiket, SaveSystem.instance.powerTicket, SaveSystem.instance.coin, SaveSystem.instance.star);
    }
    public void addStar(int star)
    {
        this.star += star;
        UIManagerNew.Instance.LoadData(SaveSystem.instance.magicTiket, SaveSystem.instance.powerTicket, SaveSystem.instance.coin, SaveSystem.instance.star);
    }
    public void transfer()
    {
        while (magicTiket > 0)
        {
            {
                magicTiket = magicTiket - 1;
                addCoin(50);
            }
        }
        while (powerTicket > 0)
        {

            powerTicket = powerTicket - 1;
            addCoin(30);
        }
    }
    public void AddBooster(int unscrewPoint,int undoPoint)
    {
        this.unscrewPoint += unscrewPoint;
        this.undoPoint += undoPoint;
        UIManagerNew.Instance.LoadData(SaveSystem.instance.magicTiket, SaveSystem.instance.powerTicket, SaveSystem.instance.coin, SaveSystem.instance.star);
    }
    //public void DelayAdding(int startValue, int endValue, float time)
    //{
    //    DOVirtual.Float(startValue, endValue, time, (x);
    //    DOVirtual.Float(0, y, 0.5f, (x) =>
    //    {
    //    }
    //}
}
