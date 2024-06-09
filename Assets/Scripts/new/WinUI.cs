using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinUI : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;
    [SerializeField]
    private TextMeshProUGUI TimeText;

    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("Disappear");

    public GameObject coinImgDes;
    public GameObject StarImgDes;
    public GameObject StarShadowImg;

    public List<CoinReward> coin = new List<CoinReward>();
    public StarReward star = new StarReward();

    public Transform startPos;

    public Transform canvaToMove;
    public Transform canvaToBack;

    public AnimationCurve scaleCurve;


    public void Appear()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        animButton.enabled = true;
        cvButton.blocksRaycasts = false;
        animButton.Play(appearButton, 0, 0);
        //displayTime();
        //displayTime();
        DisplayPicture();

    }

    public void Close()
    {
        cvButton.blocksRaycasts = false;
        animButton.Play(disappearButton);
    }

    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void ActiveCVGroup()
    {
        if (!cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = true;
        }
    }
    public void displayTime()
    {
        int minutes = Mathf.FloorToInt((181 - GamePlayPanelUIManager.Instance.timer.TimeLeft) / 60);
        int seconds = Mathf.FloorToInt((181 - GamePlayPanelUIManager.Instance.timer.TimeLeft) % 60);
        TimeText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    public void DisplayPicture()
    {
        GameManagerNew.Instance.PictureUIManager.HiddenButton();
        GameManagerNew.Instance.PictureUIManager.Open();

    }
    public void MoveToAddRW()
    {

    }
    public void ContinueBT()
    {
        //Deactive();
        //UIManagerNew.Instance.WinUI.Close();
        animButton.Play("Disappear", 0, 0);
        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
        
        //GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level, false);


    }

    public void SpawnStar()
    {
        animButton.enabled = false;
        //star.transform.position = new Vector3(301, -449, 0);
        //DOVirtual.DelayedCall(0.25f, () =>
        //{
        //    star.ActiveFireWorkParticle(true);
        //});
        StartCoroutine(MoveToDes());
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
                else
                {
                    DOVirtual.DelayedCall(1.5f, () =>
                    {
                        startPos.transform.DOScale(0, 0.3f);
                        UIManagerNew.Instance.GamePlayLoading.appear();


                        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                    });
                    DOVirtual.DelayedCall(1.5f, () =>
                        {
                            GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level, false);
                            GameManagerNew.Instance.PictureUIManager.DisplayButton();
                            DOVirtual.DelayedCall(1.5f, () =>
                            {
                                Deactive();
                                UIManagerNew.Instance.CompleteUI.Deactive();
                                AudioManager.instance.PlayMusic("MenuTheme");
                            });
                            // code can thiet ( sau move reward )
                            if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
                            {
                                UIManagerNew.Instance.ButtonMennuManager.Appear();
                                if (GameManagerNew.Instance.PictureUIManager.picTutor != null)
                                {
                                    GameManagerNew.Instance.PictureUIManager.picTutor.CheckHasFixed();
                                }
                            }
                        });
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
        coin[i].transform.DOLocalMove(randomPosition, 0.4f).OnComplete(() =>
        {
            //DOVirtual.DelayedCall(0.8f, () =>
            //{
            MoveCoin(coin, i, i+1);
            //});
        });
        //coin[i].transform.localPosition = randomPosition;
    }
    public void MoveCoin(List<CoinReward> list, int i, int value)
    {
        if (i < list.Count)
        {
            float time = .7f / list.Count;
            list[i].MoveToFix(list[i], list[i].transform.position, coinImgDes.transform.position, Vector3.one, () =>
            {
                coinImgDes.gameObject.transform.DOScale(.8f, 0.15f).OnComplete(() =>
                {
                    UIManagerNew.Instance.coinTexts[3].SetText((SaveSystem.instance.coin -10 + value).ToString());
                    SaveSystem.instance.addCoin(1);
                    SaveSystem.instance.SaveData();
                    AudioManager.instance.PlaySFX("AddCoin");
                    coinImgDes.gameObject.transform.DOScale(.7f, 0.02f);
                });
                var x = list[i];
                DOVirtual.DelayedCall(0f, () =>
                {
                    x.gameObject.SetActive(false);
                });
            });

        }
    }
    public void MoveStar(StarReward list)
    {
        float time = .3f;
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        //Vector3 stepPos = new Vector3(list.transform.position.x + 1, list.transform.position.y - 0.5f, list.transform.position.z);
        //list.transform.DOMove(stepPos, 0.3f).OnComplete(() =>
        //{
            Vector3 rotationAngles = new Vector3(0, 0, 360);
            star.transform.DORotate(rotationAngles, 1f, RotateMode.FastBeyond360
                    //.SetLoops(-1, LoopType.Incremental)
                    /*.SetEase(Ease.InOutBack*/).OnComplete(() =>
                    {
                        star.transform.DORotate(Vector3.zero, 0.05f); ; // Đặt góc quay về 0
                    });

            list.MoveToFix(list, list.transform.position, StarImgDes.transform.position, new Vector3(.3f, .3f, 1f), () =>
                {
                    Debug.Log(list.transform.position);
                    SpawnCoin(0, 250, 250);

                    StarImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                    {
                        StarImgDes.gameObject.transform.DOScale(1f, 0.02f);
                        SaveSystem.instance.addStar(1);
                        SaveSystem.instance.SaveData();
                    });
                    StarShadowImg.gameObject.SetActive(true);
                    StarShadowImg.gameObject.transform.DOScale(0.3f, 0.15f).OnComplete(() =>
                    {
                        StarShadowImg.gameObject.transform.DOScale(.25f, 0.05f).OnComplete(() =>
                        {
                            StarShadowImg.gameObject.SetActive(false);
                        });
                    });
                    var x = list;
                    DOVirtual.DelayedCall(0.03f, () =>
                    {
                        x.gameObject.SetActive(false);
                    });

                });
        //});
    }

    IEnumerator Close1()
    {
        yield return new WaitForSeconds(4.5f);

        star.transform.SetParent(this.transform);
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
        GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level, false);
        GameManagerNew.Instance.PictureUIManager.DisplayButton();
        this.gameObject.SetActive(false);
    }

    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(0.6f);
        //star.ActiveFireWorkParticle(false);

        MoveStar(star);
    }
}
