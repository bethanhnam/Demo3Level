using Facebook.Unity;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static weeklyReward;

public class weeklyRewardController : MonoBehaviour
{
    public weeklyReward weeklyReward;

    public List<weeklyReward> weeklyRewardList = new List<weeklyReward>();

    public Transform content;

    public void CreateRewardList()
    {
        for (int i = 0; i < 30; i++)
        {
            var x = GameObject.Instantiate(weeklyReward, content);
            if (!weeklyRewardList.Contains(x))
            {
                weeklyRewardList.Add(x);
            }
        }
    }
    public void AddData()
    {
        for (int i = 0; i < weeklyRewardList.Count; i++)
        {
            weeklyRewardList[i].SetData(i + 1, CheckSprite(i), CheckNumOfReward(i), CheckRewardImage(i));
        }
    }

    public void UpdateData()
    {
        for (int i = 0; i < weeklyRewardList.Count; i++)
        {
            if (EventController.instance.weeklyEvent != null)
            {
                if (i < EventController.instance.weeklyEvent.levelIndex)
                {
                    weeklyRewardList[i].Claimed();
                }
                else if (i == EventController.instance.weeklyEvent.levelIndex)
                {
                    weeklyRewardList[i].Selected();
                    
                }
                else
                {
                    weeklyRewardList[i].NotClaim();
                }
            }
            else
            {
                weeklyRewardList[i].NotClaim();
            }
        }
    }

    public void CompleteEvent()
    {
        for (int i = 0; i < weeklyRewardList.Count; i++)
        {
            weeklyRewardList[i].Claimed();
        }
    }

    public Sprite CheckSprite(int i)
    {
        Sprite sprite = null;
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfGold != 0)
        {
            sprite = EventController.instance.weeklyEventControllers[0].weeklyEventPacksprite[0];
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUnscrew != 0)
        {
            sprite = EventController.instance.weeklyEventControllers[0].weeklyEventPacksprite[1];
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUndo != 0)
        {
            sprite = EventController.instance.weeklyEventControllers[0].weeklyEventPacksprite[2];
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfDrill != 0)
        {
            sprite = EventController.instance.weeklyEventControllers[0].weeklyEventPacksprite[3];
        }
        return sprite;
    }
    public int CheckNumOfReward(int i )
    {
        int x = 0;
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfGold != 0)
        {
            x = EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfGold;
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUnscrew != 0)
        {
            x = EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUnscrew;
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUndo != 0)
        {
            x = EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUndo;
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfDrill != 0)
        {
            x = EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfDrill;
        }
        return x;
    }

    public rewardType CheckRewardImage(int i)
    {
        rewardType rewardType = rewardType.gold;
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfGold != 0)
        {
            rewardType = rewardType.gold;
        }   
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUnscrew != 0)
        {
            rewardType = rewardType.unscrew;
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUndo != 0)
        {
            rewardType = rewardType.undo;
        }
        if (EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfDrill != 0)
        {
            rewardType = rewardType.drill;
        }
        return rewardType;
    }

    public void RewardClaim(int i)
    {
        if (weeklyRewardList[i].rewardType1 == rewardType.gold)
        {
            SaveSystem.instance.addCoin(EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfGold);
        }
        if (weeklyRewardList[i].rewardType1 == rewardType.unscrew)
        {
            SaveSystem.instance.AddBooster(EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUnscrew,0,0);
        }
        if (weeklyRewardList[i].rewardType1 == rewardType.undo)
        {
            SaveSystem.instance.AddBooster(0,EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfUndo,0);
        }
        if (weeklyRewardList[i].rewardType1 == rewardType.drill)
        {
            SaveSystem.instance.AddBooster(0,0,EventController.instance.weeklyEventControllers[0].weeklyEventPack[i].numOfDrill);
        }
    }
}
