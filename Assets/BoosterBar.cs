using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoosterBar : MonoBehaviour
{
    public Button deteleBT;
    public Button UndoBT;
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
}
