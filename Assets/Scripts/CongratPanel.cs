using DG.Tweening;
using DG.Tweening.Core.Easing;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CongratPanel : MonoBehaviour
{
    public RectTransform rewardImg;
    public RectTransform rewardLight;
    public RectTransform rewardOpen;
    public GameObject RewardPrefab;
    public List<GameObject> Listeward = new List<GameObject>();
    public List<TextMeshProUGUI> rewardsValue = new List<TextMeshProUGUI>();
    public List<Transform> rewardsPos = new List<Transform>();

    public RectTransform tapToOpen;
    public Button tapToClaim;
    public Animator congratuationText;

    public int typeOfReward;
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("OpenPopUp");
            rewardLight.GetComponent<CanvasGroup>().alpha = 0f;
            rewardImg.gameObject.SetActive(true);

            tapToOpen.gameObject.SetActive(true);
            rewardLight.gameObject.SetActive(false);
            rewardOpen.gameObject.SetActive(false);
            rewardOpen.localScale = Vector3.one;
            rewardLight.localScale = Vector3.one;
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            DestroyRW();
            SaveSystem.instance.SaveData();
            rewardOpen.gameObject.SetActive(false);
            tapToClaim.gameObject.SetActive(false);
            rewardImg.transform.position = rewardOpen.transform.position;
            rewardImg.GetComponent<Animator>().enabled = true;
            this.gameObject.SetActive(false);
            AudioManager.instance.PlaySFX("ClosePopUp");
            SaveSystem.instance.SaveData();
        }
    }
    public void ComPleteImgViaButton()
    {
        GameManagerNew.Instance.CompleteImgAppearViaButton(() =>
        {
            Close();
        });
    }
    public void TakeReward()
    {
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_bonus, () =>
        {
            if (PlayerPrefs.GetInt("HasRecieveRW") == 0)
            {
                takeRewardData();
                tapToOpen.gameObject.SetActive(false);
                rewardLight.gameObject.SetActive(true);
                rewardLight.GetComponent<CanvasGroup>().DOFade(1, 0.5f);
                rewardImg.DORotate(Vector3.zero,0.4f);
                rewardImg.GetComponent<Animator>().enabled = false;
                rewardLight.DOScale(2f, 1f).OnComplete(() =>
                {
                    rewardLight.gameObject.SetActive(false);
                    //rewardImg.DOMove(rewardOpen.transform.position, 1f);
                    rewardImg.DOScale(1f, 1f).OnComplete(() =>
                    {
                        AudioManager.instance.PlaySFX("OpenChest");
                        DisplayRW();
                        //if (LoadingScreen.instance.cv.blocksRaycasts == true)
                        //{
                        //    LoadingScreen.instance.cv.blocksRaycasts = false;
                        //}
                        rewardOpen.transform.position = rewardImg.transform.position;
                        rewardOpen.transform.localScale = rewardImg.transform.localScale;
                        rewardImg.gameObject.SetActive(false);
                        rewardOpen.gameObject.SetActive(true);
                    });
                });
            }
        }, null);
    }
    public void takeRewardData()
    {
        //add data to save system
        for (int i = 0; i < DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA.Length; i++)
        {
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.gold)
            {
                SaveSystem.instance.addCoin(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value);
            }
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.Unscrew)
            {
                SaveSystem.instance.AddBooster(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value, 0,0);
            }
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.Undo)
            {
                SaveSystem.instance.AddBooster(0, DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value,0);
            }
            SaveSystem.instance.SaveData();
        }
        PlayerPrefs.SetInt("HasRecieveRW", 1);
    }
    public void DisplayRW()
    {
        for (int i = 0; i < DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA.Length; i++)
        {
            var x = Instantiate(RewardPrefab, rewardOpen.transform.position, Quaternion.identity, this.transform);
            x.GetComponent<Image>().sprite = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].rwSprite;
            x.transform.localScale = Vector2.zero;
            x.gameObject.SetActive(true);
            Listeward.Add(x);
            rewardsValue.Add(x.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
            SetValue(rewardsValue[i], i);
            congratuationText.enabled = false;
            congratuationText.gameObject.SetActive(true);
        }
        StartCoroutine(MoveRW());
    }
    IEnumerator MoveRW()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < Listeward.Count; i++)
        {
            Listeward[i].transform.DOScale(Vector2.one, 0.7f);
            Listeward[i].transform.DOMove(rewardsPos[i].position, 0.5f).OnComplete(() =>
            {
                congratuationText.enabled = true;

            });
        }
        StartCoroutine(DisplayClaimBT());
    }
    IEnumerator DisplayClaimBT()
    {
        yield return new WaitForSeconds(0.7f);
        tapToClaim.interactable = false;
        tapToClaim.gameObject.SetActive(true);
        tapToClaim.transform.localScale = Vector3.zero;
        tapToClaim.transform.DOScale(Vector3.one, 1f).OnComplete(() =>
        {
            tapToClaim.interactable = true;
        });

    }
    public void close()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        ComPleteImgViaButton();
    }
    public void SetValue(TextMeshProUGUI valueText, int i)
    {
        String x = "X" + DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value.ToString();
        valueText.text = x;
    }
    public void DestroyRW()
    {
        for (int i = Listeward.Count-1; i >=0; i--)
        {
            var x = Listeward[i];
            Listeward.Remove(x);
            Destroy(x);
        }
        if (Listeward.IsNullOrEmpty())
        {
            Listeward.Clear();
        }
        rewardsValue.Clear();
    }
}
