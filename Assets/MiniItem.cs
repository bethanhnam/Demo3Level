using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniItem : MonoBehaviour
{
    public Image itemImg;
    public Animator animator;

    public void MoveItem()
    {
        itemImg.transform.DOMove(UIManagerNew.Instance.MiniGamePlay.sliders[0].transform.position, 0.5f).OnComplete(() => {
            //animator.enabled =false;
            this.gameObject.SetActive(false);
        });
    }
    public void SetImage(Sprite sprite)
    {
        itemImg.sprite = sprite;
    }
}
