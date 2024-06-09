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


        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            claimPanel.gameObject.SetActive(false);
            if (GameManagerNew.Instance.CheckLevelStage())
            {
                LevelManagerNew.Instance.stage = LevelManagerNew.Instance.stageList.Count - 1;

                UIManagerNew.Instance.GamePlayLoading.appear();
                DOVirtual.DelayedCall(.7f, () =>
                {
                    UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                    GameManagerNew.Instance.CreateLevel(LevelManagerNew.Instance.stage);
                });
            }
            else
            {
                LevelManagerNew.Instance.ResetLevel(() =>
                {
                    GameManagerNew.Instance.RecreatePicAfterCompleteGame();
                });
            }
            this.gameObject.SetActive(false);
            SaveSystem.instance.SaveData();
            if (UIManagerNew.Instance.CongratPanel.gameObject.activeSelf == true)
            {
                UIManagerNew.Instance.CongratPanel.Close();
            }

        }
    }
}
