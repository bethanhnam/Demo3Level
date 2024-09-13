using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventCollectItem : MonoBehaviour
{
    public Image itemImg;
    public Animator animator;
    public Transform targetPos;

    private void OnEnable()
    {
        SetImage();
    }
    public void MoveItem()
    {
        Vector3 worldPosition = new Vector3(targetPos.position.x, targetPos.position.y - 2, 1);
        this.transform.DOMove(worldPosition, 0.5f).OnComplete(() =>
        {
            GamePlayPanelUIManager.Instance.EventCollectItemList.DestroyItem(this);
            animator.enabled = false;
            targetPos.transform.DOScale(0.8f, 0.05f).OnComplete(() =>
            {
                targetPos.transform.DOScale(0.7f, 0.01f);
            });
            AudioManager.instance.PlaySFX("AddCoin");
            UIManagerNew.Instance.GamePlayPanel.numOfCollection.text = Stage.Instance.numOfEventItem.ToString();
        });
    }
    public void SetImage()
    {
        if (EventController.instance != null && EventController.instance.weeklyEvent != null)
        {
            itemImg.sprite = UIManagerNew.Instance.WeeklyEventPanel.ItemToCollect.sprite;
        }
    }
}
