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

    public TextMeshProUGUI unscrewNumText;
    public TextMeshProUGUI undoNumText;

    public DeteleNailPanel deteleNailPanel;
    public UndoPanel undoPanel;
    public Image pointer;
    public Transform[] transforms;
    
    public void InteractableBT(Button button)
    {
        button.interactable = true;
    }
    public void UninteractableBT(Button button)
    {
        button.interactable = false;
    }
    public void ShowPointer(bool status)
    {
        pointer.gameObject.SetActive(status);
    }
    public void SetPoiterPos(int i)
    {
        pointer.gameObject.transform.position = transforms[i].position;
    }
    public void disableDeteleWatchAdsBT()
    {
        deteleNailPanel.watchAdButton.GetComponent<Button>().interactable = false;
    }
    public void disableUndoWatchAdsBT()
    {
        undoPanel.watchAdButton.GetComponent<Button>().interactable = false;
    }
    private void Update()
    {
        if(SaveSystem.instance.unscrewPoint > 0)
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
