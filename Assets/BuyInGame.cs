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
using static FirebaseAnalyticsControl;

public class BuyInGame : MonoBehaviour
{
    public int unscrewPoint;
    public int undoPoint;
    public int extraHolePoint;
    public MyItem myitem;

    public Sprite ticketBarNor;
    public Sprite ticketBarRed;

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
                char t = '-';
                SetMinusText(t,gold);
                LoadData(-gold);
                CreateStar(this.transform.position,startPos.position, () =>
                {
                   
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                  
                    AddBoosterByads(gold);
                }, 5);
                String packname = null;
                if (myitem.id == 4)
                {
                    packname = "dill";
                }
                else if (myitem.id == 5)
                {
                    packname = "unscrew";
                }
                else if (myitem.id == 6)
                {
                    packname = "undo";
                }

                FirebaseAnalyticsControl.Instance.LogEvenGoldStatus(GoldStatus.success, GoldType.Shop_BuyByGold.ToString() + packname.ToString());
            }
            else
            {
                startPos.GetComponent<Image>().sprite = ticketBarRed;
                startPos.DORotate(new Vector3(0, 0, 10), .2f)
                .SetEase(Ease.InOutSine) // Sử dụng Ease.InOutSine để xoay mượt mà
                .SetLoops(-1, LoopType.Yoyo);
                DOVirtual.DelayedCall(1.5f, () =>
                {
                    startPos.DORotate(Vector3.zero, 0.05f).OnComplete(() =>
                    {
                        DOTween.Kill(startPos);
                    }); // Đặt góc quay về 0
                    startPos.GetComponent<Image>().sprite = ticketBarNor;

                });
                String packname = null;
                if(myitem.id == 4)
                {
                    packname = "dill";
                }
                else if(myitem.id == 5)
                {
                    packname = "unscrew";
                }
                else if (myitem.id == 6)
                {
                    packname = "undo";
                }

                FirebaseAnalyticsControl.Instance.LogEvenGoldStatus(GoldStatus.fail, GoldType.Shop_BuyByGold.ToString() + packname.ToString());
            }
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }

    }
    public void SetMinusText(char t,int value)
    {
        minusCoinText.gameObject.SetActive(true);
        minusCoinText.text = t + value.ToString();
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
        SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint, this.extraHolePoint);
        SaveSystem.instance.SaveData();
        myitem.numOfBuy++;
        myitem.Save();
        myitem.CheckNumOfUse();
    }

    public void watchAds()
    {
        if (myitem.CheckNumOfUse())
        {
            AdsManager.instance.ShowRewardVideo(AddType.Shop_BuyByAds, "Gold", () =>
            {
                char t = '+';
                SetMinusText(t,20);
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    LoadData(20);
                });
                CreateStar(startPos.position,this.transform.position, () =>
                {
                    FirebaseAnalyticsControl.Instance.BuyByAds(myitem.id);
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                    AddCoinByAds();
                }, 5);

            });
            PlayerPrefs.SetString("ClaimTime", DateTime.Today.ToString());
        }
    }
    public void watchAds1()
    {
        if (myitem.CheckNumOfUse())
        {
            AdsManager.instance.ShowRewardVideo(AddType.Shop_BuyByAds, myitem.id.ToString(), () =>
            {
                FirebaseAnalyticsControl.Instance.BuyByAds(myitem.id);
                SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint, this.extraHolePoint);
                SaveSystem.instance.SaveData();
                myitem.numOfBuy++;
                myitem.Save();
                myitem.CheckNumOfUse();

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
    IEnumerator Spawn(Vector3 des,Vector3 spawnPos, Action action, int numOfStar)
    {

        for (int i = 0; i < numOfStar; i++)
        {
            yield return new WaitForSeconds(0.3f);
            var coin = Instantiate(coinsPrefab, spawnPos, Quaternion.identity, spawnCanvas.transform);
            coin.transform.localScale = new Vector3(.5f, .5f, 1f);
            coins.Add(coin);
            MoveToDes(des, action, coin,i);
        }
    }
    public void CreateStar(Vector3 des, Vector3 spawnPos, Action action, int numOfStar)
    {
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
        this.gameObject.SetActive(true);
        StartCoroutine(Spawn(des, spawnPos, action, numOfStar));
    }
    public void MoveToDes(Vector3 des, Action action, GameObject star,int index)
    {
        star.transform.DOScale(1, 1f);
        //Vector3 rotationAngles = new Vector3(0, 0, 360);
       
        //star.transform.DORotate(Vector3.zero, 0.05f); ; // Đặt góc quay về 0
        star.transform.DOMove(des, .8f).OnComplete(() =>
        {

            StartCoroutine(Close(action, star));
        });
    }
    IEnumerator Close(Action action, GameObject star)
    {
        yield return new WaitForSeconds(0.1f);
        coins.Remove(star);
        Destroy(star,.05f);
        if (coins.IsNullOrEmpty())
        {
            action();
        }
    }
    public void LoadData(int addValue)
    {
        DOVirtual.Float(SaveSystem.instance.coin, SaveSystem.instance.coin + addValue, 1.7f, (x) =>
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

