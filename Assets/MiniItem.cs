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
        var target = UIManagerNew.Instance.MiniGamePlay.MiniGameMaps[UIManagerNew.Instance.MiniGamePlay.selectedMinimap].collectTargetPos.position;
        Vector3 worldPosition = new Vector3(target.x, target.y - 2, 1);
        this.transform.DOMove(worldPosition, 0.3f).OnComplete(() =>
        {
            animator.enabled = false;
            this.gameObject.SetActive(false);
            if (UIManagerNew.Instance.MiniGamePlay.selectedMinimap == 1)
            {
                AudioManager.instance.PlaySFX("FastFlame");
            }
            MiniGamePlay.instance.ChangeCollectSliderValue();
        });
    }
    public void SetImage(Sprite sprite)
    {
        itemImg.sprite = sprite;
    }
}
