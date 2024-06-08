using DG.Tweening;
using GoogleMobileAds.Api;
using JetBrains.Annotations;
using Sirenix.Utilities;
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

    public Transform startPos;
    public List<GameObject> coins;
    public GameObject coinsPrefab;
    public Canvas spawnCanvas;

    public TextMeshProUGUI CoinText;
    public TextMeshProUGUI minusCoinText;
    public void BuyByGold(int gold)
    {
        if (myitem.CheckNumOfUse())
        {
            if (SaveSystem.instance.coin - gold >= 0)
            {
                SetMinusText(gold);
                LoadData(-gold);
                CreateStar(this.transform.position, () =>
                {
                    AddBoosterByads(gold);
                }, 5);
            }
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }

    }
    public void SetMinusText(int value)
    {
        minusCoinText.gameObject.SetActive(true);
        minusCoinText.text = "-" + value.ToString();
        StartCoroutine(DisableText());
    }
    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(0.5f);
        minusCoinText.gameObject.SetActive(false);
    }
    private void AddBoosterByads(int gold)
    {
        SaveSystem.instance.addCoin(-gold);
        SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint);
        SaveSystem.instance.SaveData();
        myitem.numOfBuy++;
        myitem.Save();
        myitem.CheckNumOfUse();
    }

    public void watchAds()
    {
        if (myitem.CheckNumOfUse())
        {
            AdsManager.instance.ShowRewardVideo(() =>
            {
                
                CreateStar(this.transform.position, () =>
                {
                    AddCoinByAds();

                },0);
            });
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }
    }

    private void AddCoinByAds()
    {
        
        SaveSystem.instance.addCoin(20);
        SaveSystem.instance.SaveData();
        myitem.numOfBuy++;

        myitem.Save();
        myitem.CheckNumOfUse();
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
    IEnumerator Spawn(Vector3 des, Action action, int numOfStar)
    {
        for (int i = 0; i < numOfStar; i++)
        {
            yield return new WaitForSeconds(0.3f);
            var coin = Instantiate(coinsPrefab, startPos.position, Quaternion.identity, spawnCanvas.transform);
            coin.transform.localScale = new Vector3(.5f, .5f, 1f);
            coins.Add(coin);
            MoveToDes(des, action, coin);
        }
    }
    public void CreateStar(Vector3 des, Action action, int numOfStar)
    {
        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
        this.gameObject.SetActive(true);
        StartCoroutine(Spawn(des, action, numOfStar));
    }
    public void MoveToDes(Vector3 des, Action action, GameObject star)
    {
        star.transform.DOScale(1, 1f);
        //Vector3 rotationAngles = new Vector3(0, 0, 360);
       
        //star.transform.DORotate(Vector3.zero, 0.05f); ; // Đặt góc quay về 0
        star.transform.DOMove(des, 1f).OnComplete(() =>
        {

            StartCoroutine(Close(action, star));
        });
    }
    IEnumerator Close(Action action, GameObject star)
    {
        yield return new WaitForSeconds(0.1f);
        coins.Remove(star);
        Destroy(star);
        if (coins.IsNullOrEmpty())
        {
            action();
        }
    }
    public void LoadData(int addValue)
    {
        DOVirtual.Float(SaveSystem.instance.coin, SaveSystem.instance.coin + addValue, 0.3f, (x) =>
        {
            CoinText.SetText(Mathf.CeilToInt(x).ToString());
        });
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

