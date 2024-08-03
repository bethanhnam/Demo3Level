using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FixItemUI : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;

    [SerializeField]
    private Animator animImage;
    [SerializeField]
    private Image imgPic;

    private int appearButton = Animator.StringToHash("appear");
    private int appear = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("Disappear");

    private int appearImage = Animator.StringToHash("appear");
    private int idleImage = Animator.StringToHash("idle");
    public void Appear(Sprite spr)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }
        animButton.Play(appearButton, 0, 0);
        //imgPic.transform.localScale = Vector3.zero;
        imgPic.sprite = spr;
        AudioManager.instance.PlaySFX("ItemAppear");
        animImage.Play(appear);
        DisplayPicture();
        ContinueBT();
    }


    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    public void DisplayPicture()
    {
        GameManagerNew.Instance.PictureUIManager.HiddenButton();
        GameManagerNew.Instance.PictureUIManager.Open();
    }

    public void ContinueBT()
    {
        StartCoroutine(Fix());
    }
    IEnumerator Fix()
    {
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        Deactive();
        GameManagerNew.Instance.ItemMoveControl.MoveToFix(imgPic.transform.position, GameManagerNew.Instance.PictureUIManager.GetCurrentPosItem(), imgPic.sprite, () =>
        {
            
            if (GameManagerNew.Instance.PictureUIManager.hasWindow )
            {
                if (GameManagerNew.Instance.PictureUIManager.LevelButton != null)
                {
                    if(GameManagerNew.Instance.PictureUIManager.GetcurrentLevelButton() == GameManagerNew.Instance.PictureUIManager.LevelButton)
                    {
                        GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, "tremble_happy", false, GameManagerNew.Instance.PictureUIManager.hasWindow);
                        AudioManager.instance.PlaySFX("Laugh");
                        if (LevelManagerNew.Instance.LevelBase.Level == 0)
                        {
                            GameManagerNew.Instance.conversationController.StartConversation(1, 3, "4AafterFirstFix", () =>
                            {
                            });
                        }
                    }
                }
            }
            else
            {
                GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, "sad-happy", false, GameManagerNew.Instance.PictureUIManager.hasWindow);
                AudioManager.instance.PlaySFX("Laugh");
            }
            UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
            GameManagerNew.Instance.CreateParticleEF();
            GameManagerNew.Instance.ItemMoveControl.gameObject.SetActive(false);
            GameManagerNew.Instance.PictureUIManager.ChangeItemOnly(LevelManagerNew.Instance.LevelBase.Level);
            for (int i = 0; i < GameManagerNew.Instance.PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].listObjLock[GameManagerNew.Instance.Level].objunLock.Count; i++)
            {
                GameManagerNew.Instance.PictureUIManager.ChangeItem(GameManagerNew.Instance.PictureUIManager.Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].listObjLock[GameManagerNew.Instance.Level].objunLock[i]);
            }
            //AudioManager.instance.PlayMusic("MenuTheme");
            UIManagerNew.Instance.ChestSLider.ChangeValue(() =>
            {
                GameManagerNew.Instance.SetCompleteStory();
                float time = 2;
                if (UIManagerNew.Instance.ChestSLider.currentValue == UIManagerNew.Instance.ChestSLider.maxValue1)
                {
                    time = 2;
                }
                else
                {
                    time = 0.9f;
                }
                DOVirtual.DelayedCall(time, () =>
                {
                    UIManagerNew.Instance.BlockPicCanvas.SetActive(false);
                });
            });
            GameManagerNew.Instance.PictureUIManager.EnableCV();
            DataLevelManager.Instance.SaveData();
            LevelManagerNew.Instance.SaveData();
        });
    }
}
