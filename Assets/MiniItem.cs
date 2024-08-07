using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniItem : MonoBehaviour
{
    public Image itemImg;
    public Animator animator;
    public Transform targetPos;

    public void MoveItem()
    {
        this.transform.DOMove(UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[UIManagerNew.Instance.MiniGamePlay.selectedMinimap].itemImage.transform.position, 0.5f).OnComplete(() => {
            animator.enabled = false;
            this.gameObject.SetActive(false);
            MiniGamePlay.instance.ChangeCollectSliderValue();
        });
    }
    public void SetImage(Sprite sprite)
    {
        itemImg.sprite = sprite;
    }
}
