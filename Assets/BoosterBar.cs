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

    public TextMeshProUGUI unscrewNumText;
    public TextMeshProUGUI undoNumText;

    public DeteleNailPanel deteleNailPanel;
    public UndoPanel undoPanel;
    public Transform[] transforms;

    private void Start()
    {
        if(LevelManagerNew.Instance.stage == 3 && UIManagerNew.Instance.DeteleNailPanel.hasUseTutor == false)
        {
            freeUnscrewImg.gameObject.SetActive(true);
            unscrewNumImg.gameObject.SetActive(false);
        }
        else
        {
            freeUnscrewImg.gameObject.SetActive(false);
            unscrewNumImg.gameObject.SetActive(true);
        }
    }
    private void Update()
    {
        if (!freeUnscrewImg.gameObject.activeSelf)
        {
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
}
