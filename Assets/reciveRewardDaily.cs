using DG.Tweening;
using GoogleMobileAds.Api;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class reciveRewardDaily : MonoBehaviour
{
    public DailyPanel dailyPanel;
    public GameObject posDes;
    public GameObject coinDes;

    public List<CoinReward> coinList = new List<CoinReward>();
    public List<CoinReward> unscrewList = new List<CoinReward>();
    public List<CoinReward> undoList = new List<CoinReward>();

    public CoinReward coinPrefab;
    public CoinReward unscrewprefab;
    public CoinReward undoprefab;

    public Canvas spawnCanvas;
    public Image backgroundImg;
    public Image rewardImg;
    public TextMeshProUGUI rewardText;

    String x = "";
    [SerializeField]
    int gold = 0;
    [SerializeField]
    int unscrew = 0;
    [SerializeField]
    int undo = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void SetValue()
    {
        if (dailyPanel.isClaim)
        {

            if (dailyPanel.dayRewards[dailyPanel.lastDate].gold > 0)
            {
                rewardImg.sprite = coinPrefab.transform.GetChild(0).GetComponent<Image>().sprite;
                gold = dailyPanel.dayRewards[dailyPanel.lastDate].gold;
            }
            if (dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket > 0)
            {
                rewardImg.sprite = unscrewprefab.GetComponent<Image>().sprite;
                unscrew = dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket;
            }
            if (dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket > 0)
            {
                rewardImg.sprite = undoprefab.GetComponent<Image>().sprite;
                undo = dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket;

            }
        }
        if (dailyPanel.isClaimX2)
        {
            if (dailyPanel.dayRewards[dailyPanel.lastDate].gold > 0)
            {
                rewardImg.sprite = coinPrefab.transform.GetChild(0).GetComponent<Image>().sprite;
                gold = dailyPanel.dayRewards[dailyPanel.lastDate].gold * 2;

            }
            if (dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket > 0)
            {
                rewardImg.sprite = unscrewprefab.GetComponent<Image>().sprite;
                unscrew = dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket * 2;

            }
            if (dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket > 0)
            {
                rewardImg.sprite = undoprefab.GetComponent<Image>().sprite;
                undo = dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket * 2;
            }
            //DOVirtual.Float(y/2, y * 2, 0.5f, (x) =>
            //{
            //    rewardText.text = Mathf.CeilToInt(x).ToString();
            //});

        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
    }
    public void claim()
    {
        backgroundImg.gameObject.SetActive(true);
        SetValue();
        DOVirtual.DelayedCall(4, () =>
        {
            SpawnObjects(dailyPanel.dayRewards[dailyPanel.lastDate].gold, dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket, dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket, rewardImg.gameObject);
        });
    }
    public void SpawnObjects(int goldValue, int unscrewValue, int undoValue, GameObject SpawnPoint)
    {

        // Kích thước của object cha (giả sử là 400x400)
        float parentWidth = this.GetComponent<RectTransform>().rect.width;
        float parentHeight = this.GetComponent<RectTransform>().rect.height;

        if (goldValue > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                var coin = coinList[i];
                coin.transform.position = SpawnPoint.transform.position;
                //var coin = Instantiate(coinPrefab, SpawnPoint.transform.position, Quaternion.identity, spawnCanvas.transform);
                //coinList.Add(coin);

                var coinIndex = coin;
                coinIndex.transform.localPosition = Vector3.zero;
                coinIndex.transform.localScale = Vector3.zero;

                // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
                float randomX = UnityEngine.Random.Range(-parentWidth / 2, parentWidth / 2);
                float randomY = UnityEngine.Random.Range(-parentHeight / 2, parentHeight / 2);

                //Tạo một vị trí mới cho object con
                Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

                coinIndex.gameObject.SetActive(true);
                coinIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
                {
                    float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);
                    coinIndex.GetComponent<Animator>().speed = randomSpeed;
                });

                // Instantiate object con và gán nó vào object cha
                coinIndex.transform.DOLocalMove(randomPosition, 0.4f);
                //coinIndex.transform.localPosition = randomPosition;
            }
        }
        for (int i = 0; i < this.unscrew; i++)
        {
            var unscrew = unscrewList[i];
            unscrew.transform.position = SpawnPoint.transform.position;

            var unscrewIndex = unscrew;
            unscrewIndex.transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-100 / 2, 100 / 2);
          

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX,0, 1f);

            //unscrewIndex.gameObject.SetActive(true);
            unscrewIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);

            });
            // Instantiate object con và gán nó vào object cha
            unscrewIndex.transform.DOLocalMove(randomPosition, 0.4f);
        }
        for (int i = 0; i < this.undo; i++)
        {
            var undo = undoList[i];
            undo.transform.position = SpawnPoint.transform.position;

            var undoIndex = undo;
            undoIndex.transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-100 / 2, 100 / 2);
            
            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, 0, 1f);

            //undoIndex.gameObject.SetActive(true);
            undoIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);

            });
            // Instantiate object con và gán nó vào object cha
            undoIndex.transform.DOLocalMove(randomPosition, 0.4f);
        }
        StartCoroutine(MoveToDes());
    }
    public void test(List<CoinReward> list, int i,int value)
    {
        if (value > 0)
        {
            float time = .7f / value;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (i - 1 >= 0)
                {
                    if (i - 1 >= 0)
                        test(list, i - 1,value-1);
                }
            });
            list[i].MoveToFix(list[i], list[i].transform.position, coinDes.transform.position, new Vector3(0.8f, 0.8f, 1),1, new Vector3(-1, 3, 0), () =>
            {
                coinDes.gameObject.transform.DOScale(.8f, 0.15f).OnComplete(() =>
                {

                    AudioManager.instance.PlaySFX("AddCoin");
                    coinDes.gameObject.transform.DOScale(.7f, 0.02f);
                });
                if (i >= 0)
                {
                    var x = list[i];
                    x.gameObject.SetActive(false);
                }
            });
        }
    }
    public void test1(List<CoinReward> list, int i,int value)
    {
        if (value > 0)
        {
            float time = .7f / value;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                if(i - 1 >= 0)
                test1(list, i - 1, value - 1);
            });
            list[i].MoveToFix(list[i], list[i].transform.position, posDes.transform.position, new Vector3(0.8f, 0.8f, 1), 1, new Vector3(-1, 3, 0), () =>
            {
                posDes.gameObject.transform.DOScale(1.1f, 0.15f).OnComplete(() =>
                {

                    AudioManager.instance.PlaySFX("AddCoin");
                    posDes.gameObject.transform.DOScale(1f, 0.02f);
                });
                if (i >= 0)
                {
                    var x = list[i];
                    x.gameObject.SetActive(false);
                }
            });
        }
    }
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(.5f);
        if (gold > 0)
        {
            var x = dailyPanel.startValue;
            DOVirtual.Float(x, x + gold, 2.2f, (x) =>
            {
                UIManagerNew.Instance.coinTexts[0].text = Mathf.CeilToInt(x).ToString();

            });
            test(coinList, coinList.Count - 1,gold);
            //SaveSystem.instance.addCoin(gold);
        }
        if (unscrew > 0)
        {
            test1(unscrewList, unscrewList.Count - 1,unscrew);
            //SaveSystem.instance.AddBooster(unscrew, 0);
        }
        if (undo > 0)
        {
            test1(undoList, undoList.Count - 1,undo);
            //SaveSystem.instance.AddBooster(0, undo);
        }
        SaveSystem.instance.SaveData();
        
        StartCoroutine(Close());
    }
    IEnumerator Close()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
        if(gold > 0)
        {
            for (int i = 0; i < 10; i++)
            {
                coinList[i].gameObject.SetActive(false);
            }
        }
        if (unscrew > 0)
        {
            for (int i = 0; i < unscrew; i++)
            {
                unscrewList[i].gameObject.SetActive(false);
            }
        }
        if (undo > 0)
        {
            for (int i = 0; i < undo; i++)
            {
                undoList[i].gameObject.SetActive(false);
            }
        }
       DOVirtual.DelayedCall(0.3f, () =>
        {
            gold = 0;
            unscrew = 0;
            undo = 0;
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
        });
    }
    public void close()
    {
        backgroundImg.gameObject.SetActive(false);
        dailyPanel.gameObject.SetActive(false);
        UIManagerNew.Instance.ButtonMennuManager.Appear();
    }
}
