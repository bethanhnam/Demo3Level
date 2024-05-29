using DG.Tweening;
using DG.Tweening.Core.Easing;
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
    public Image RewardPrefab;
    public List<Image> Listeward = new List<Image>();
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
            takeRewardData();
            tapToOpen.gameObject.SetActive(false);
            rewardLight.gameObject.SetActive(true);
            rewardLight.GetComponent<CanvasGroup>().DOFade(1, 0.6f);
            rewardLight.DOScale(1.3f, 0.5f).OnComplete(() =>
            {
                rewardLight.gameObject.SetActive(false);
                rewardImg.GetComponent<Animator>().enabled = false;
                rewardImg.DOMove(new Vector2(-.5f, -2.5f), 1f);
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
        }, null);
    }
    public void takeRewardData()
    {
        //add data to save system
        for (int i = 0; i < DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA.Length; i++) {
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.gold) {
                SaveSystem.instance.addCoin(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value);
            }
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.Unscrew)
            {
                SaveSystem.instance.addTiket(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value,0);
            }
            if (DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].type == ItemPicture.type.Undo)
            {
                SaveSystem.instance.addTiket(0,DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value);
            }
            SaveSystem.instance.SaveData();
        }

    }
    public void DisplayRW()
    {
        for (int i = 0; i < DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA.Length; i++)
        {
            var x = Instantiate(RewardPrefab, rewardOpen.transform.position, Quaternion.identity, this.transform);
            x.sprite = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].rwSprite;
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
        yield return new WaitForSeconds(1);
        for(int i = 0;i < Listeward.Count;i++)
        {
            Listeward[i].transform.DOScale(Vector2.one, 0.7f);
            Listeward[i].transform.DOMove(rewardsPos[i].position, 0.7f).OnComplete(() =>
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
        ComPleteImgViaButton();
    }
    public void SetValue(TextMeshProUGUI valueText, int i)
    {
        String x = "X"+ DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].PresentA[i].value.ToString();
        valueText.text = x;
    }
    public void DestroyRW()
    {
        for (int i = 0;i<Listeward.Count;i++)
        {
            Destroy(Listeward[i]);
            Listeward.RemoveAt(i);
            
        }
        rewardsValue.Clear();
    }
}
