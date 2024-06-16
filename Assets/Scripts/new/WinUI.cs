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
    public GameObject coinIconDes;
    public GameObject StarImgDes;
    public GameObject StarShadowImg;

    public List<CoinReward> coin = new List<CoinReward>();
    public StarReward star = new StarReward();

    public Transform startPos;
    public Transform spawnPos;

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
        animButton.Play("Disappear", 0, 0);
        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
    }

    public void SpawnStar()
    {
        animButton.enabled = false;
        MoveStar(star);
    }

    private void SpawnCoin(int i)
    {
        if (i < coin.Count)
        {
            coin[i].transform.position = spawnPos.position;
            coin[i].transform.localScale = Vector3.zero;

            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (i + 1 < coin.Count)
                {
                    SpawnCoin(i + 1);
                }
                else
                {
                    DOVirtual.DelayedCall(0f, () =>
                    {
                        startPos.transform.DOScale(0, 0.3f);

                    });
                }
            });
        }
        // Tạo một vị trí mới cho object con
        Vector3 randomPosition = new Vector3(coin[i].transform.position.x, coin[i].transform.position.y, coin[i].transform.position.z);
        var coinIndex = coin[i];
        coin[i].gameObject.SetActive(true);
        coin[i].transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
        {
            float randomSpeed = UnityEngine.Random.Range(0.5f, 2f);
            coinIndex.GetComponent<Animator>().speed = randomSpeed;
        });
        // Instantiate object con và gán nó vào object cha
        coin[i].transform.DOMove(randomPosition, 0.15f).OnComplete(() =>
        {
            MoveCoin(coin, i, i + 1);
        });
    }
    public void MoveCoin(List<CoinReward> list, int i, int value)
    {
        if (i < list.Count)
        {
            float time = .7f / list.Count;
            list[i].MoveToFix(list[i], list[i].transform.position, coinIconDes.transform.position, Vector3.one, () =>
            {
                coinImgDes.gameObject.transform.DOScale(1.1f, 0.15f).OnComplete(() =>
                {
                    UIManagerNew.Instance.coinTexts[3].SetText((SaveSystem.instance.coin - 10 + value).ToString());
                    SaveSystem.instance.addCoin(1);
                    SaveSystem.instance.SaveData();
                    AudioManager.instance.PlaySFX("AddCoin");
                    coinImgDes.gameObject.transform.DOScale(1f, 0.02f);
                });
                var x = list[i];
                DOVirtual.DelayedCall(0.05f, () =>
                {
                    x.gameObject.SetActive(false);
                });
                if (i == list.Count - 1)
                {
                    UIManagerNew.Instance.GamePlayLoading.appear();
                    UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                    DOVirtual.DelayedCall(2f, () =>
                    {
                        Stage.Instance.canInteract = true;
                        Stage.Instance.isWining = false;
                        GameManagerNew.Instance.PictureUIManager.Open();
                        GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level, false);
                        GameManagerNew.Instance.CloseLevel(true);
                        GameManagerNew.Instance.PictureUIManager.DisplayButton();
                        Deactive();
                        UIManagerNew.Instance.CompleteUI.Deactive();
                        AudioManager.instance.PlayMusic("MenuTheme");
                        // code can thiet ( sau move reward )
                        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.Appear();
                            if (GameManagerNew.Instance.PictureUIManager.picTutor != null)
                            {
                                GameManagerNew.Instance.PictureUIManager.picTutor.CheckHasFixed();
                            }
                        }
                        //DOVirtual.DelayedCall(1f, () =>
                        //{

                        //});

                    });
                }
            });

        }
    }
    public void MoveStar(StarReward list)
    {
        float time = .3f;
        UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
        Vector3 rotationAngles = new Vector3(0, 0, 360);
        star.transform.DORotate(rotationAngles, 1f, RotateMode.FastBeyond360
                //.SetLoops(-1, LoopType.Incremental)
                /*.SetEase(Ease.InOutBack*/).OnComplete(() =>
                {
                    star.transform.DORotate(Vector3.zero, 0.05f); ; // Đặt góc quay về 0
                });
        SpawnCoin(0);
        AudioManager.instance.PlaySFX("StarRecieve");
        Debug.Log("StarImgDes " + StarImgDes.transform.position);
        list.MoveToFix(list, list.transform.position, StarImgDes.transform.position, new Vector3(.3f, .3f, 1f), () =>
            {
                Debug.Log("list " + list.transform.position);
                Debug.Log("Đã đến nơi ");
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


    }
}
