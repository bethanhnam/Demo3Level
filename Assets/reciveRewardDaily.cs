using DG.Tweening;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class reciveRewardDaily : MonoBehaviour
{
    public DailyPanel dailyPanel;
    public GameObject posDes;
    public GameObject coinDes;

    public List<GameObject> coinList = new List<GameObject>();
    public List<GameObject> unscrewList = new List<GameObject>();
    public List<GameObject> undoList = new List<GameObject>();

    public GameObject coinPrefab;
    public GameObject unscrewprefab;
    public GameObject undoprefab;

    public Canvas spawnCanvas;
    public Image backgroundImg;
    public Image rewardImg;
    public TextMeshProUGUI rewardText;

    String x = "";
    int y = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void SetValue()
    {
        if (dailyPanel.lastDate != 6)
        {
            
            if (dailyPanel.isClaim)
            {
                
                if (dailyPanel.dayRewards[dailyPanel.lastDate].gold > 0)
                {
                    rewardImg.sprite = coinPrefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].gold;
                    x = "X" + y;
                }
                if (dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket > 0)
                {
                    rewardImg.sprite = unscrewprefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket;
                    x = "X" + y;
                }
                if (dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket > 0)
                {
                    rewardImg.sprite = undoprefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket;
                    x = "X" + y;
                }
                DOVirtual.Float(0, y,0.5f, (x) =>
                {
                    rewardText.text = Mathf.CeilToInt(x).ToString();
                    
                });
            }
            if (dailyPanel.isClaimX2)
            {
                int y = 0;
                if (dailyPanel.dayRewards[dailyPanel.lastDate].gold > 0)
                {
                    rewardImg.sprite = coinPrefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].gold*2;
                    x = "X" + y;
                    SaveSystem.instance.addCoin(y * 2);
                }
                if (dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket > 0)
                {
                    rewardImg.sprite = unscrewprefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket*2;
                    x = "X" + y;
                    SaveSystem.instance.AddBooster(y * 2,0);
                }
                if (dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket > 0)
                {
                    rewardImg.sprite = undoprefab.GetComponent<Image>().sprite;
                    y = dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket *2;
                    x = "X" + y;
                    
                }
                DOVirtual.Float(y/2, y * 2, 0.5f, (x) =>
                {
                    rewardText.text = Mathf.CeilToInt(x).ToString();
                });
            }
            
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
        DOVirtual.DelayedCall(3, () =>
        {
            SpawnObjects(dailyPanel.dayRewards[dailyPanel.lastDate].gold, dailyPanel.dayRewards[dailyPanel.lastDate].magicTiket, dailyPanel.dayRewards[dailyPanel.lastDate].powerTicket, rewardImg.gameObject);
        });
    }
    public void SpawnObjects(int goldValue, int unscrewValue, int undoValue, GameObject SpawnPoint)
    {
        // Kích thước của object cha (giả sử là 400x400)
        float parentWidth = this.GetComponent<RectTransform>().rect.width;
        float parentHeight = this.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < goldValue; i++)
        {
            var coin = Instantiate(coinPrefab, SpawnPoint.transform.position, Quaternion.identity, spawnCanvas.transform);
            coinList.Add(coin);

            var coinIndex = coin;
            coinIndex.transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = UnityEngine.Random.Range(-parentHeight / 2, parentHeight / 2);

            //Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

            coinIndex.SetActive(true);
            coinIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);
                coinIndex.GetComponent<Animator>().speed = randomSpeed;
            });

            // Instantiate object con và gán nó vào object cha
            coinIndex.transform.localPosition = randomPosition;
        }
        for (int i = 0; i < unscrewValue; i++)
        {
            var unscrew = Instantiate(unscrewprefab, SpawnPoint.transform.position, Quaternion.identity, spawnCanvas.transform);
            unscrewList.Add(unscrew);

            var unscrewIndex = unscrew;
            unscrewIndex.transform.localScale = Vector3.zero;

            //// Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = UnityEngine.Random.Range(-parentHeight / 2, parentHeight / 2);

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

            unscrewIndex.SetActive(true);
            unscrewIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);

            });

            // Instantiate object con và gán nó vào object cha
            unscrewIndex.transform.localPosition = randomPosition;
        }
        for (int i = 0; i < undoValue; i++)
        {
            var undo = Instantiate(undoprefab, SpawnPoint.transform.position, Quaternion.identity, spawnCanvas.transform);
            undoList.Add(undo);

            var undoIndex = undo;
            undoIndex.transform.localScale = Vector3.zero;

            //// Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = UnityEngine.Random.Range(-parentHeight / 2, parentHeight / 2);

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

            undoIndex.SetActive(true);
            undoIndex.transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);

            });

            // Instantiate object con và gán nó vào object cha
            undoIndex.transform.localPosition = randomPosition;
        }
        StartCoroutine(MoveToDes());
    }
    public void test(List<GameObject> list,int i)
    {
        if (list.Count > 0)
        {
            float time = .7f / coinList.Count;
            list[i].transform.DOMove(coinDes.transform.position, time).OnComplete(() =>
            {
                var x = list[i];
                list.Remove(x);
                Destroy(x.gameObject);
                test(list,i - 1);
            });
        }
    }
    public void test1(List<GameObject> list, int i)
    {
        if (list.Count > 0)
        {
            list[i].transform.DOMove(posDes.transform.position, 0.3f).OnComplete(() =>
            {
                var x = list[i];
                list.Remove(x);
                Destroy(x.gameObject);
                test1(list, i - 1);
            });
        }
    }
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(1.1f);
        if (coinList.Count > 0)
        {
            test(coinList, coinList.Count - 1);
            SaveSystem.instance.addCoin(y);
        }
        if (unscrewList.Count > 0)
        {
            test1(unscrewList, unscrewList.Count - 1);
            SaveSystem.instance.AddBooster(y,0);
        }
        if (undoList.Count > 0)
        {
            test1(undoList, undoList.Count - 1);
            SaveSystem.instance.AddBooster(0,y);
        }
        SaveSystem.instance.SaveData();
        StartCoroutine(Close());
    }
    IEnumerator Close()
    {
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
    public void close()
    {
        backgroundImg.gameObject.SetActive(false);
        dailyPanel.gameObject.SetActive(false);
        UIManagerNew.Instance.ButtonMennuManager.Appear();
    }
}
