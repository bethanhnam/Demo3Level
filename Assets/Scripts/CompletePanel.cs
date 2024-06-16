using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePanel : MonoBehaviour
{
    public RectTransform claimPanel;
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            AudioManager.instance.PlaySFX("LosePop");
            claimPanel.gameObject.SetActive(true);
            PlayerPrefs.SetInt("HasCompleteLastLevel", 1);
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            claimPanel.gameObject.SetActive(false);
            if (GameManagerNew.Instance.CheckLevelStage())
            {
                int replayLevel = Random.Range(0, 29);
                LevelManagerNew.Instance.stage = replayLevel;
                UIManagerNew.Instance.GamePlayLoading.appear();
                DOVirtual.DelayedCall(.7f, () =>
                {
                    UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                    GameManagerNew.Instance.CreateLevel(replayLevel);
                });
            }
            else
            {
                LevelManagerNew.Instance.ResetLevel(() =>
                {
                    UIManagerNew.Instance.CongratPanel.Close();
                    UIManagerNew.Instance.CompleteImg.Disablepic();
                    GameManagerNew.Instance.PictureUIManager.ChangeReaction(0, "idle_happy", true, GameManagerNew.Instance.PictureUIManager.hasWindow);
                    if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
                    {
                        UIManagerNew.Instance.ButtonMennuManager.Appear();
                    }
                });
            }
        }
        this.gameObject.SetActive(false);
        SaveSystem.instance.SaveData();
        if (UIManagerNew.Instance.CongratPanel.gameObject.activeSelf == true)
        {
            UIManagerNew.Instance.CongratPanel.Close();
        }

    }
}
