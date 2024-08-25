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
    public List<StarReward> star = new List<StarReward>();
    public GameObject prefabToSpawn;
    public Transform startPos;

    public Canvas canvaToMove;

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
        star[0].transform.SetParent(canvaToMove.transform);
        star[0].transform.DOScale(new Vector3(2.1f, 2.1f, 2.1f), 0.3f).OnComplete(() =>
        {
            star[0].ActiveGlowParticle(true);
            DOVirtual.DelayedCall(0.25f, () =>
            { 
                star[0].ActiveFireWorkParticle(true);
            });
            star[0].transform.DOScale(new Vector3(1.9f, 1.9f, 1.9f), 0.2f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.3f, () =>
                {
                    star[0].ActiveGlowParticle(false);
                    
                });
                star[0].transform.DOScale(new Vector3(2f, 2f, 2f), 0.2f).OnComplete(() =>
                {

                    StartCoroutine(MoveToDes());

                });
            });
        });
    }

    private void SpawnCoin(int i, float parentWidth, float parentHeight)
    {
        if (i < coin.Count)
        {
            DOVirtual.DelayedCall(0.15f, () =>
            {
                if (i + 1 < coin.Count)
                {
                    SpawnCoin(i + 1, parentWidth, parentHeight);
                }
            });
        }
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
        DOVirtual.DelayedCall(0.8f, () =>
        {
            MoveCoin(coin, i, 1);
        });
    }

    public void MoveCoin(List<CoinReward> list, int i, int value)
    {
        if (i < list.Count)
        {
            float time = .7f / list.Count;
                list[i].MoveToFix(list[i], list[i].transform.position, coinImgDes.transform.position, Vector3.one, 1, new Vector3(-1, 3, 0), () =>
                {
                   
                    coinImgDes.gameObject.transform.DOScale(.8f, 0.15f).OnComplete(() =>
                    {
                        UIManagerNew.Instance.coinTexts[0].SetText((SaveSystem.instance.coin + value+2).ToString());
                        AudioManager.instance.PlaySFX("AddCoin");
                        coinImgDes.gameObject.transform.DOScale(.7f, 0.02f);
                    });
                    var x = list[i];
                    DOVirtual.DelayedCall(0.01f, () =>
                    {
                        x.gameObject.SetActive(false);
                    });
                });
        }
    }
    public void MoveStar(List<StarReward> list, int i)
    {
        float time = .3f / list.Count;
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        Vector3 stepPos = new Vector3(list[i].transform.position.x -1, list[i].transform.position.y - 1f, list[i].transform.position.z);
        list[i].transform.DOMove(stepPos, 0.3f).OnComplete(() =>
        {
            list[0].MoveToFix(list[0], list[0].transform.position, StarImgDes.transform.position, Vector3.one, () =>
                {
                    int i = 0;
                    SpawnCoin(i, this.GetComponent<RectTransform>().rect.width, this.GetComponent<RectTransform>().rect.height);
                   
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
                    DOVirtual.DelayedCall(0.03f, () =>
                    {
                        x.gameObject.SetActive(false);
                    });

                });
        });
    }
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(0.6f);
        star[0].ActiveFireWorkParticle(false);
        int j = 0;
        if (j < star.Count)
        {
            MoveStar(star, j);
        }
        StartCoroutine(Close());
    }
    IEnumerator Close()
    {
        yield return new WaitForSeconds(4.5f);
        
        star[0].transform.SetParent(this.transform);
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
        GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level, false);
        this.gameObject.SetActive(false);
    }
}
