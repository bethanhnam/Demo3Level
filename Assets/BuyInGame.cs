using JetBrains.Annotations;
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
    public int unscrewPoint;
    public int undoPoint;
    public MyItem myitem;
    

    public void BuyByGold(int gold)
    {
        if (myitem.CheckNumOfUse())
        {
            if (SaveSystem.instance.coin - gold >= 0)
            {
                SaveSystem.instance.coin -= gold;
                SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint);
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
        public TextMeshProUGUI numOfUseText;

        public void Save()
        {
            PlayerPrefs.SetInt("numOfBuy" + id, numOfBuy);
            SetNumOfUseText(numOfBuy);
        }
        public void loadData()
        {
            numOfBuy = PlayerPrefs.GetInt("numOfBuy" + id);
            SetNumOfUseText(numOfBuy);
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
        public void SetNumOfUseText(int numofUse)
        {
            String t = "(" + numofUse.ToString() + "/3)";
            numOfUseText.text = t;
        }
    }
}

