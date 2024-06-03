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
            close();
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
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(1.1f);
        for (int i = 0; i < coinList.Count; i++)
        {
            //Destroy(coin[i],i * .1f);
            coinList[i].transform.DOMove(coinDes.transform.position, i * .1f).OnComplete(() =>
            {
                //coinImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                //{
                //    coinImgDes.gameObject.transform.DOScale(1f, 0f);
                //});
                StartCoroutine(Close(i));
            });
        }
        for (int i = 0; i < unscrewList.Count; i++)
        {
            
            //Destroy(star[i], i * .1f);
            unscrewList[i].transform.DOMove(posDes.transform.position, 0.3f).OnComplete(() =>
            {
                unscrewList[i].gameObject.SetActive(false);
                //StarImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                //{
                //    StarImgDes.gameObject.transform.DOScale(1f, 0f);
                //});
            });
        }
        for (int i = 0; i < undoList.Count; i++)
        {
            
            //Destroy(star[i], i * .1f);
            undoList[i].transform.DOMove(posDes.transform.position, 0.3f).OnComplete(() =>
            {
                undoList[i].gameObject.SetActive(false);
                //StarImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                //{
                //    StarImgDes.gameObject.transform.DOScale(1f, 0f);
                //});
            });
        }
        //this.gameObject.SetActive(false);
    }
    IEnumerator Close(int time)
    {
        yield return new WaitForSeconds(time * 0.08f);
        for (int i = 0; i < coinList.Count; i++)
        {
            var x = coinList[i];
            coinList[i].SetActive(false);
            coinList.Remove(x);
            Destroy(x, 5f);
            SaveSystem.instance.addCoin(y);
        }
        for (int i = 0; i < unscrewList.Count; i++)
        {
            var x = unscrewList[i];
            unscrewList[i].SetActive(false);
            unscrewList.Remove(x);
            Destroy(x,5f);
            SaveSystem.instance.AddBooster(y,0);
        }
        for (int i = 0; i < undoList.Count; i++)
        {
            var x = undoList[i];
            undoList[i].SetActive(false);
            undoList.Remove(x);
            Destroy(x,5f);
            SaveSystem.instance.AddBooster(0, y);
        }
        SaveSystem.instance.SaveData();
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
