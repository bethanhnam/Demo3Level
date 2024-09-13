using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeeklyItemCollect : MonoBehaviour
{
    public List<Image> weeklyEventItems;

    public Transform targetTransform;

    public int numOfCollection = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [Button("test move item")]
    public void MoveCollectItem(int i)
    {
        
        SetItemImage();
        if (i < weeklyEventItems.Count)
        {
            weeklyEventItems[i].transform.position = Vector3.zero;
            weeklyEventItems[i].transform.localScale = Vector3.zero;

            DOVirtual.DelayedCall(0.1f, () =>
            {
                if (i + 1 < weeklyEventItems.Count)
                {
                    MoveCollectItem(i + 1);
                }
            });
        }
        // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
        float randomX = UnityEngine.Random.Range(-5 / 2, 5 / 2);
        float randomY = UnityEngine.Random.Range(-2 / 2, 2 / 2);

        //Tạo một vị trí mới cho object con
        Vector3 randomPosition = new Vector3(randomX, randomY, 0f);

        weeklyEventItems[i].gameObject.SetActive(true);
        weeklyEventItems[i].transform.DOScale(new Vector3(0.5f, 0.5f, 1), 1f);
        // Instantiate object con và gán nó vào object cha
        weeklyEventItems[i].transform.DOMove(randomPosition, 0.5f).OnComplete(() =>
        {
            if (i == weeklyEventItems.Count - 1)
            {
                UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
                MoveItem(weeklyEventItems[i], i, () =>
                {
                    CheckForFullSlider(numOfCollection, false);
                    ResetEverything();
                    numOfCollection = 0;
                    DOVirtual.DelayedCall(0.3f, () =>
                    {
                        UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
                    });
                });
            }
            else
            {
                UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
                MoveItem(weeklyEventItems[i], i);
            }
        });
    }

    private void CheckForFullSlider(int numOfCollection, bool checkAgain)
    {
        if (checkAgain)
        {
            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value = 0;
        }
        var value = UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value + numOfCollection;
        var max = UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue;
        var x = value - max;

        UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
        if (UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value + numOfCollection >= UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue)
        {
            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.DOValue(UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue, 0.5f).OnComplete(() =>
            {

                UIManagerNew.Instance.ButtonMennuManager.NumOfCollect.text = value.ToString() + "/" + max.ToString();

                if (EventController.instance.weeklyEvent.levelIndex == weeklyEventItems.Count - 1)
                {
                    UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value = UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue;
                }

                if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.gold)
                {
                    EventController.instance.NextStageWeeklyEvent(numOfCollection, checkAgain);
                    UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.DOMove(UIManagerNew.Instance.ButtonMennuManager.coinBar.transform.position, 1).OnComplete(() =>
                    {
                        AudioManager.instance.PlaySFX("Coins");

                        if (EventController.instance.weeklyEvent.levelIndex != EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.localScale = Vector3.zero;
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.position = UIManagerNew.Instance.ButtonMennuManager.rewardDefautTransform.position;
                        }
                        if (EventController.instance.weeklyEvent.levelIndex >= EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(false);
                        }

                        if (EventController.instance.weeklyEvent.levelIndex < EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {

                            UIManagerNew.Instance.ButtonMennuManager.ChangeRewardImage();
                            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value = 0;
                            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                            UIManagerNew.Instance.ButtonMennuManager.NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.DOScale(Vector3.one, 1).OnComplete(() =>
                            {
                                UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.DOValue(x, 0.5f).OnComplete(() =>
                                {
                                    if (x >= EventController.instance.weeklyEventControllers[0].weeklyEventPack[EventController.instance.weeklyEvent.levelIndex + 1].numToUpLevel)
                                    {
                                        CheckForFullSlider((int)x, true);
                                    }
                                    UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                                });

                            });
                        }
                    });
                }
                else
                if (UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.unscrew || UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.undo || UIManagerNew.Instance.WeeklyEventPanel.weeklyRewardController.weeklyRewardList[EventController.instance.weeklyEvent.levelIndex].rewardType1 == weeklyReward.rewardType.drill)
                {
                    EventController.instance.NextStageWeeklyEvent(numOfCollection, checkAgain);
                    UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.DOMove(UIManagerNew.Instance.ButtonMennuManager.playButton.transform.position, 1).OnComplete(() =>
                    {
                        AudioManager.instance.PlaySFX("Coins");

                        if (EventController.instance.weeklyEvent.levelIndex != EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.localScale = Vector3.zero;
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.position = UIManagerNew.Instance.ButtonMennuManager.rewardDefautTransform.position;
                        }
                        if (EventController.instance.weeklyEvent.levelIndex >= EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.gameObject.SetActive(false);
                        }
                        //EventController.instance.NextStageWeeklyEvent(numOfCollection);
                        if (EventController.instance.weeklyEvent.levelIndex < EventController.instance.weeklyEventControllers[0].weeklyEventPack.Count - 1)
                        {
                            UIManagerNew.Instance.ButtonMennuManager.ChangeRewardImage();
                            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.value = 0;
                            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.maxValue = EventController.instance.weeklyEvent.numToLevelUp;
                            UIManagerNew.Instance.ButtonMennuManager.NumOfCollect.text = EventController.instance.weeklyEvent.numOfCollection.ToString() + "/" + EventController.instance.weeklyEvent.numToLevelUp;
                            UIManagerNew.Instance.ButtonMennuManager.rewardImage.transform.DOScale(Vector3.one, 1).OnComplete(() =>
                            {
                                UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.DOValue(x, 0.5f).OnComplete(() =>
                                {
                                    if (x >= EventController.instance.weeklyEventControllers[0].weeklyEventPack[EventController.instance.weeklyEvent.levelIndex + 1].numToUpLevel)
                                    {
                                        CheckForFullSlider((int)x, true);
                                    }
                                    UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                                });

                            });

                        }
                    });
                }
            });
        }
        else
        {
            UIManagerNew.Instance.ButtonMennuManager.weeklyEventSlider.DOValue(value, 0.3f).OnComplete(() =>
            {
                EventController.instance.NextStageWeeklyEvent(numOfCollection, checkAgain);
                UIManagerNew.Instance.ButtonMennuManager.LoadSliderValue();
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                numOfCollection = 0;
            });
        }
    }

    public void MoveItem(Image image, int i, Action action = null)
    {
        image.transform.DOMove(targetTransform.position, 1f + 0.1f*i).OnComplete(() =>
        {
            action();
        });
    }

    public void ResetEverything()
    {
        foreach (var item in weeklyEventItems)
        {
            item.transform.localScale = Vector3.zero;
            item.transform.position = Vector3.zero;
            item.gameObject.SetActive(false);
        }

    }

    public void SetItemImage()
    {
        foreach (var item in weeklyEventItems)
        {
            item.sprite = UIManagerNew.Instance.WeeklyEventPanel.ItemToCollect.sprite;
        }
    }
}
