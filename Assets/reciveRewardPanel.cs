using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using JetBrains.Annotations;
using Sirenix.Utilities;

public class reciveRewardPanel : MonoBehaviour
{
    public RewardDaily[] rewardsDaily;
    public DailyPanel dailyPanel;

    //public ParticleSystem particle;
    public RectTransform rewardImg;
    public RectTransform rewardLight;
    public RectTransform rewardOpen;

    public RectTransform tapToOpen;
    public TextMeshProUGUI valueText;

    public Image RWImg;

    public List<Image> rewards = new List<Image>();
    public List<TextMeshProUGUI> rewardsValue = new List<TextMeshProUGUI>();
    public List<Transform> rewardsPos = new List<Transform>();
    Vector3 pos;

    public ParticleSystem[] particleSystems;
    public CanvasGroup canvasGroup;

    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            canvasGroup.alpha = 1;
            AudioManager.instance.PlaySFX("OpenPopUp");
            rewardLight.GetComponent<CanvasGroup>().alpha = 0f;

            rewardImg.gameObject.SetActive(true);
            tapToOpen.gameObject.SetActive(true);
            rewardLight.gameObject.SetActive(false);
            rewardOpen.gameObject.SetActive(false);
            rewardOpen.localScale = Vector3.one;
            rewardLight.localScale = Vector3.one;

            if (rewardsDaily[dailyPanel.lastDate].rewardImg.Length == 1)
            {
                var x = Instantiate(RWImg, rewardImg.transform.position, Quaternion.identity, transform);
                x.sprite = rewardsDaily[dailyPanel.lastDate].rewardImg[0].sprite;
                x.transform.position = rewardImg.transform.position;
                x.transform.localScale = new Vector2(2, 2);
                rewards.Add(x);
                rewardsValue.Add(x.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                SetValue(rewardsValue[0], 0);
            }
            else
            {
                for (int i = 0; i < rewardsDaily[dailyPanel.lastDate].rewardImg.Length; i++)
                {
                    var x = Instantiate(RWImg, rewardImg.transform.position, Quaternion.identity, transform);
                    x.sprite = rewardsDaily[dailyPanel.lastDate].rewardImg[i].sprite;
                    x.transform.position = rewardImg.transform.position;
                    rewards.Add(x);
                    rewardsValue.Add(x.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
                    SetValue(rewardsValue[i], i);
                }
            }
        }
    }
    IEnumerator Close1()
    {
        yield return new WaitForSeconds(1f);
        if (this.gameObject.activeSelf)
        {
            SaveSystem.instance.SaveData();
            rewardOpen.gameObject.SetActive(false);
            this.gameObject.SetActive(false);
            AudioManager.instance.PlaySFX("ClosePopUp");
            SaveSystem.instance.SaveData();
        }
    }
    public void TakeReward(Action action)
    {
        AdsManager.instance.ShowInterstial(AdsManager.PositionAds.endgame_bonus, () =>
        {
            takeReward();
            tapToOpen.gameObject.SetActive(false);
            rewardLight.gameObject.SetActive(true);
            rewardLight.GetComponent<CanvasGroup>().DOFade(1, 0.6f);
            rewardLight.DOScale(1.3f, 0.5f).OnComplete(() =>
            {
                rewardLight.gameObject.SetActive(false);
                rewardImg.GetComponent<Animator>().enabled = false;
                rewardImg.DOMove(new Vector2(0, -5), 1f);
                rewardImg.DOScale(0.5f, 1f).OnComplete(() =>
                {
                    AudioManager.instance.PlaySFX("OpenChest");
                    rewardOpen.transform.position = rewardImg.transform.position;
                    rewardOpen.transform.localScale = rewardImg.transform.localScale;
                    rewardImg.gameObject.SetActive(false);
                    rewardOpen.gameObject.SetActive(true);
                    RewardAppear(action);
                });
            });
        }, null);
    }
    public void RewardAppear(Action action)
    {
        if (rewards.Count == 1)
        {
            rewards[0].transform.position = rewardOpen.position;
            rewards[0].transform.localScale = Vector2.zero;
            rewards[0].gameObject.SetActive(true);
            rewards[0].transform.DOScale(2, 1);
            rewards[0].transform.DOMove(setPos(0), 1f).OnComplete(() =>
            {
                action();
                //rewardOpen.gameObject.SetActive(false);
            });
        }
        else
        {

            for (int i = 0; i < rewards.Count; i++)
            {

                if (i != rewards.Count - 1)
                {
                    rewards[i].transform.position = rewardOpen.position;
                    rewards[i].transform.localScale = Vector2.zero;
                    rewards[i].gameObject.SetActive(true);
                    rewards[i].transform.DOScale(1, 1);
                    rewards[i].transform.DOMove(setPos(i), 1f);
                }
                else
                {
                    rewards[i].transform.position = rewardOpen.position;
                    rewards[i].transform.localScale = Vector2.zero;
                    rewards[i].gameObject.SetActive(true);
                    rewards[i].transform.DOScale(1, 1);
                    rewards[i].transform.DOMove(setPos(i), 1f).OnComplete(() =>
                    {
                        action();
                        //rewardOpen.gameObject.SetActive(false);
                    });
                }
            }
        }
        for(int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].gameObject.SetActive(true);
        }
    }

    private Vector3 setPos(int i)
    {

        if (rewards.Count == 1)
        {
            pos = rewardsPos[i].position;
        }
        if (rewards.Count == 2)
        {
            pos = rewardsPos[i + 1].position;
            return pos;
        }
        if (rewards.Count == 3)
        {

            pos = rewardsPos[i + 2].position;
            return pos;

        }

        return pos;
    }

    public void takeReward()
    {
        if (dailyPanel.isClaim == true)
        {
            AudioManager.instance.PlaySFX("GetReward");
            rewardsDaily[dailyPanel.lastDate].isClaim = true;
            SaveSystem.instance.days = dailyPanel.lastDate + 1;
            SaveSystem.instance.addTiket(rewardsDaily[dailyPanel.lastDate].powerTicket, rewardsDaily[dailyPanel.lastDate].magicTiket);
            SaveSystem.instance.addCoin(rewardsDaily[dailyPanel.lastDate].gold);
            SaveSystem.instance.SaveData();
        }
        else if (dailyPanel.isClaimX2)
        {
            AudioManager.instance.PlaySFX("GetReward");
            rewardsDaily[dailyPanel.lastDate].isClaim = true;
            SaveSystem.instance.days = dailyPanel.lastDate + 1;
            SaveSystem.instance.addTiket(rewardsDaily[dailyPanel.lastDate].powerTicket * 2, rewardsDaily[dailyPanel.lastDate].magicTiket * 2);
            SaveSystem.instance.addCoin(rewardsDaily[dailyPanel.lastDate].gold * 2);
            SaveSystem.instance.SaveData();
        }
    }
    public void SetValue(TextMeshProUGUI valueText, int i)
    {
        if (dailyPanel.isClaim == true)
        {
            if (i == 0)
            {
                if (rewardsDaily[dailyPanel.lastDate].magicTiket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].magicTiket;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].powerTicket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].powerTicket;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold;
                    valueText.text = value;
                    return;
                }
            }
            if (i == 1)
            {
                if (rewardsDaily[dailyPanel.lastDate].magicTiket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].powerTicket;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold;
                    valueText.text = value;
                    return;
                }
            }
            if (i == 2)
            {
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold;
                    valueText.text = value;
                }
            }
        }
        else if (dailyPanel.isClaimX2 == true)
        {
            if (i == 0)
            {
                if (rewardsDaily[dailyPanel.lastDate].magicTiket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].magicTiket * 2;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].powerTicket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].powerTicket * 2;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold * 2;
                    valueText.text = value;
                    return;
                }
            }
            if (i == 1)
            {
                if (rewardsDaily[dailyPanel.lastDate].magicTiket > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].powerTicket * 2;
                    valueText.text = value;
                    return;
                }
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold * 2;
                    valueText.text = value;
                    return;
                }
            }
            if (i == 2)
            {
                if (rewardsDaily[dailyPanel.lastDate].gold > 0)
                {
                    string value = "X" + rewardsDaily[dailyPanel.lastDate].gold * 2;
                    valueText.text = value;
                }
            }
        }
    }
    public void Close()
    {
        for (int i = 0; i < rewards.Count; i++)
        {
            Destroy(rewards[i]);
            rewards.Remove(rewards[i]);
        }
        for (int i = 0; i < particleSystems.Length; i++)
        {
            particleSystems[i].gameObject.SetActive(false);
        }
        canvasGroup.DOFade(0, .8f).OnComplete(() =>
        {
            this.gameObject.SetActive(false);    
        });
    }
}
