using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ExtraHoleButton : MonoBehaviour
{
    // Thêm một biến public để kéo và thả nút vào trong Inspector
    public GameObject extraHole;
    public Button myButton;
    public Animator myAnimator;
    public Vector3 myScale;
    public Image addImage;
    public ParticleSystem shinningParticle;
    void Start()
    {
        //myScale = addImage.transform.localScale + new Vector3(0.1f, 0.1f, 0.1f);
    }

    public void MyFunction()
    {
        Stage.Instance.SetDefaultBeforeUnscrew();
        if (SaveSystem.instance.extraHolePoint >= 1)
        {
            if (UIManagerNew.Instance.ThresholeController.gameObject.activeSelf)
            {
                UIManagerNew.Instance.ThresholeController.Disable();
            }
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.drill);
            SaveSystem.instance.AddBooster(0, 0, -1);
            SaveSystem.instance.SaveData();
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.5f, () =>
            {
                if (!Stage.Instance.isWining)
                {
                    Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
                    Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
                    UIManagerNew.Instance.GamePlayPanel.ShowDrillEffect(() =>
                    {
                        Stage.Instance.ChangeLayer();
                    });
                }
            });
        }
        else
        {
            UIManagerNew.Instance.GamePlayPanel.OpenExtraHolePanel();

        }
    }
    public void ScaleUp()
    {
        var scaleUp = myScale + new Vector3(0.1f, 0.1f, 0.1f);
        addImage.transform.DOScale(scaleUp, 0.5f);
    }
    public void ScaleDown()
    {
        var scaleDown = myScale - new Vector3(0.1f, 0.1f, 0.1f);
        addImage.transform.DOScale(scaleDown, 0.5f).OnComplete(() =>
        {
            //shinningParticle.Play();
        });
    }
    public void NomalScale()
    {
        addImage.transform.DOScale(myScale, 0.5f);
    }
}
