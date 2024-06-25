using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorPointer : MonoBehaviour
{
    public Transform[] pointerPos;
    public Animator pointerAnm;
    public GameObject goobJob;
    public Transform goobJobPos;
    public void SetPos(int i)
    {
        this.gameObject.SetActive(true);
        this.transform.DOMove(pointerPos[i].position, 0.5f).OnComplete(()=>{
            pointerAnm.enabled = true;  
        });
    }
    public void DisablePointer()
    {
        this.gameObject.SetActive(false);
        if (!GameManagerNew.Instance.isStory)
        {
            GamePlayPanelUIManager.Instance.goodJob.gameObject.SetActive(true);
            GamePlayPanelUIManager.Instance.goodJob.transform.DOMoveY(GamePlayPanelUIManager.Instance.goodJob.transform.position.y + 2, 1.3f);
            DOVirtual.DelayedCall(2f, () =>
            {
                GamePlayPanelUIManager.Instance.goodJob.transform.position = new Vector2(GamePlayPanelUIManager.Instance.goodJob.transform.position.x, GamePlayPanelUIManager.Instance.goodJob.transform.position.y - 2);
                GamePlayPanelUIManager.Instance.goodJob.gameObject.SetActive(false);
            });
        }
    }
}
