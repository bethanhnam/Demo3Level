using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class RewardMove : MonoBehaviour
{
    public GameObject coinImgDes;
    public GameObject StarImgDes;
    public GameObject StarShadowImg;
    public Image CoinShadowImg;
    public List<CoinReward> coin = new List<CoinReward>();
    public List<CoinReward> star = new List<CoinReward>();
    public GameObject prefabToSpawn;
    public Transform startPos;

    public AnimationCurve scaleCurve;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        SpawnObjects();
    }
    private void SpawnObjects()
    {
        // Kích thước của object cha (giả sử là 400x400)
        float parentWidth = this.GetComponent<RectTransform>().rect.width;
        float parentHeight = this.GetComponent<RectTransform>().rect.height;

        star[0].transform.localPosition = startPos.position;
        star[0].transform.localScale = Vector3.zero;

        star[0].gameObject.SetActive(true);
        star[0].transform.DOScale(new Vector3(3.2f, 3.2f, 3.2f), 0.1f).OnComplete(() =>
        {
            star[0].transform.DOScale(new Vector3(2.5f, 2.5f, 2.5f), 0.1f).OnComplete(() =>
            {
                star[0].transform.DOScale(new Vector3(3f, 3f, 3f), 0.1f).OnComplete(() =>
                {
                    StartCoroutine(MoveToDes());
                });
            });
        });
    }

    private void SpawnCoin(float parentWidth, float parentHeight, Action action)
    {
        for (int i = 0; i < coin.Count; i++)
        {
            coin[i].transform.localPosition = startPos.position;
            coin[i].transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = UnityEngine.Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = UnityEngine.Random.Range(-parentHeight / 2, parentHeight / 2);

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            var coinIndex = coin[i];
            coin[i].gameObject.SetActive(true);
            coin[i].transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);
                coinIndex.GetComponent<Animator>().speed = randomSpeed;
            });

            // Instantiate object con và gán nó vào object cha
            coin[i].transform.DOLocalMove(randomPosition, 0.4f);
            //coin[i].transform.localPosition = randomPosition;
        }
        action();
    }

    public void MoveCoin(List<CoinReward> list, int i, int value)
    {
        if (i < list.Count)
        {
            float time = .7f / list.Count;
            //list[i].GetComponent<Animator>().enabled = true;
            DOVirtual.DelayedCall(0.1f, () =>
            {
                MoveCoin(list, i + 1, value + 1);
            });
            list[i].MoveToFix(list[i], list[i].transform.position, coinImgDes.transform.position, () =>
            {
                coinImgDes.gameObject.transform.DOScale(.8f, 0.15f).OnComplete(() =>
                {
                    UIManagerNew.Instance.coinTexts[0].SetText((SaveSystem.instance.coin + value).ToString());
                    AudioManager.instance.PlaySFX("AddCoin");
                    coinImgDes.gameObject.transform.DOScale(.7f, 0.02f);
                });
                CoinShadowImg.gameObject.SetActive(true);
                Color color = new Color(CoinShadowImg.color.r, CoinShadowImg.color.g, CoinShadowImg.color.b, 0);
                CoinShadowImg.DOColor(color, 0.3f);
                CoinShadowImg.gameObject.transform.DOScale(0.10f, 0.15f).OnComplete(() =>
                {
                    Color color = new Color(CoinShadowImg.color.r, CoinShadowImg.color.g, CoinShadowImg.color.b, 0.4f);
                    CoinShadowImg.DOColor(color, 0.05f);
                    CoinShadowImg.gameObject.transform.DOScale(.8f, 0.05f).OnComplete(() =>
                    {
                        CoinShadowImg.gameObject.SetActive(false);
                    });
                });
                var x = list[i];
                DOVirtual.DelayedCall(0f, () =>
                {
                    x.gameObject.SetActive(false);
                });
            });
        }
    }
    public void MoveStar(List<CoinReward> list, int i)
    {
        float time = .3f / list.Count;
        //list[i].GetComponent<Animator>().enabled = true;


        list[0].transform.DOScale(Vector3.one, 0.4f).SetEase(scaleCurve);
        list[0].MoveToFix(list[0], list[0].transform.position, StarImgDes.transform.position, () =>
        {
            int i = 0;
            SpawnCoin(this.GetComponent<RectTransform>().rect.width, this.GetComponent<RectTransform>().rect.height, () =>
            {
                if (i < coin.Count)
                {
                    MoveCoin(coin, i, 1);
                }
            });
            StarImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
            {
                StarImgDes.gameObject.transform.DOScale(1f, 0.02f);

            });
            StarShadowImg.gameObject.SetActive(true);
            StarShadowImg.gameObject.transform.DOScale(0.3f, 0.15f).OnComplete(() =>
            {
                StarShadowImg.gameObject.transform.DOScale(.25f, 0.05f).OnComplete(() =>
                {
                    StarShadowImg.gameObject.SetActive(false);
                });
            });
            var x = list[0];
            DOVirtual.DelayedCall(0f, () =>
            {
                x.gameObject.SetActive(false);
            });

        });
    }
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(0.5f);
        int j = 0;
        if (j < star.Count)
        {
            MoveStar(star, j); 
        }
        StartCoroutine(Close());
    }
    IEnumerator Close()
    {
        yield return new WaitForSeconds(5f);
        this.gameObject.SetActive(false);
    }
}
