using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuyInGame : MonoBehaviour
{
    public int magicTicket;
    public int powerTicket;
    public MyItem myitem;

    public void BuyByGold(int gold)
    {
        if (myitem.CheckNumOfUse())
        {
            if (SaveSystem.instance.coin - gold >= 0)
            {
                SaveSystem.instance.coin -= gold;
                SaveSystem.instance.addTiket(this.powerTicket, this.magicTicket);
                SaveSystem.instance.SaveData();
                myitem.numOfBuy++;
                myitem.Save();
                myitem.CheckNumOfUse();
            }
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }

    }
    public void watchAds()
    {
        if (myitem.CheckNumOfUse())
        {
            AdsManager.instance.ShowRewardVideo(() =>
            {
                SaveSystem.instance.coin += 20;
                SaveSystem.instance.SaveData();
                myitem.numOfBuy++;
                myitem.Save();
                myitem.CheckNumOfUse();
            });
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }
    }
    private void Start()
    {
        checkDay();
    }
    private void checkDay()
    {
        //get last claim time");
        
        string lastTime = PlayerPrefs.GetString("ClaimTime");
        DateTime lastclaimTime;
        if (!string.IsNullOrEmpty(lastTime))
        {
            lastclaimTime = DateTime.Parse(lastTime);
        }
        else
        {
            lastclaimTime = DateTime.MinValue;
        }
        //enable / disable claim button
        if (DateTime.Today > lastclaimTime)
        {
            myitem.numOfBuy = 0;
            myitem.Save();

        }
        else
        {

            myitem.loadData();
            myitem.CheckNumOfUse();

        }
    }
    [Serializable]
    public class MyItem
    {
        public int id;
        public int numOfBuy;
        public Button button;

        public void Save()
        {
            PlayerPrefs.SetInt("numOfBuy" + id, numOfBuy);
        }
        public void loadData()
        {
            numOfBuy = PlayerPrefs.GetInt("numOfBuy" + id);

        }
        public bool CheckNumOfUse()
        {
            bool status = false;
            if (numOfBuy >= 3)
            {
                button.interactable = false;
            }
            else
            {
                button.interactable = true;
                status = true;
            }
            return status;
        }
    }
}

