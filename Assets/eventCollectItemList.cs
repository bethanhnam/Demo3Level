using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class eventCollectItemList : MonoBehaviour
{
    public List<EventCollectItem> eventCollectItems = new List<EventCollectItem>();
    public EventCollectItem eventCollectItem;

    public void SpawnItem(float PosX)
    {
        Vector2 vector = new Vector2(PosX, UIManagerNew.Instance.GamePlayPanel.EventCollectItemList.transform.position.y);

        PoolItem(vector);
    }

    public EventCollectItem PoolItem(Vector2 spawnPosition)
    {
        if (eventCollectItems.Count <= 0)
        {

            EventCollectItem a = Instantiate(eventCollectItem, spawnPosition, Quaternion.identity, transform);
            eventCollectItems.Add(a);
            return a;
        }
        else
        {
            EventCollectItem a = eventCollectItems.Where(t => !t.gameObject.activeSelf).FirstOrDefault();
            if (a == null)
            {
                a = Instantiate(eventCollectItem, spawnPosition, Quaternion.identity, transform);
                eventCollectItems.Add(a);
                return a;
            }
            else
            {
                a.transform.position = spawnPosition;
                a.gameObject.SetActive(true);
                eventCollectItems.Remove(a);
                a.animator.enabled = true;
                return a;
            }
        }

    }
    public void DestroyItem(EventCollectItem item)
    {
        item.gameObject.SetActive(false);
        eventCollectItems.Add(item);
    }
}
