using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoosterBar : MonoBehaviour
{
    public Button deteleBT;
    public Button UndoBT;

    public Image unscrewAddImg;
    public Image undoAddImg;

    public Image unscrewNumImg;
    public Image undoNumImg;

    public Image freeUnscrewImg;

    public Image blockUndoImage;

    public TextMeshProUGUI unscrewNumText;
    public TextMeshProUGUI undoNumText;

    public DeteleNailPanel deteleNailPanel;
    public UndoPanel undoPanel;
    public Transform[] transforms;

    private void Start()
    {
        if (LevelManagerNew.Instance.stage == 3)
        {
            freeUnscrewImg.gameObject.SetActive(false);
            unscrewNumImg.gameObject.SetActive(false);
        }
        else
        {
            freeUnscrewImg.gameObject.SetActive(false);
            unscrewNumImg.gameObject.SetActive(true);
        }
        if (LevelManagerNew.Instance.stage <= 4)
        {
            blockUndoImage.gameObject.SetActive(true);
            undoAddImg.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (LevelManagerNew.Instance.stage == 3)
        {
            unscrewAddImg.gameObject.SetActive(false);
            unscrewNumImg.gameObject.SetActive(false);
        }
        else
        {
            freeUnscrewImg.gameObject.SetActive(false);
            if (SaveSystem.instance.unscrewPoint > 0)
            {
                unscrewAddImg.gameObject.SetActive(false);
                unscrewNumImg.gameObject.SetActive(true);
                unscrewNumText.text = SaveSystem.instance.unscrewPoint.ToString();
            }
            else
            {
                unscrewAddImg.gameObject.SetActive(true);
                unscrewNumImg.gameObject.SetActive(false);
            }
        }
        if (LevelManagerNew.Instance.stage >= 4)
        {
            if (SaveSystem.instance.undoPoint > 0)
            {
                undoAddImg.gameObject.SetActive(false);
                undoNumImg.gameObject.SetActive(true);
                undoNumText.text = SaveSystem.instance.undoPoint.ToString();
            }
            else
            {
                undoAddImg.gameObject.SetActive(true);
                undoNumImg.gameObject.SetActive(false);
            }
        }
        if(Stage.Instance != null && Stage.Instance.holeToUnlock != null)
        {
            if(SaveSystem.instance.extraHolePoint >= 1)
            {
                Stage.Instance.holeToUnlock.addImage.gameObject.SetActive(false);
            }
            else
            {
                Stage.Instance.holeToUnlock.addImage.gameObject.SetActive(true);
            }
        }
    }
}
