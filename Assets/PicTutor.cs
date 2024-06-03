using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PicTutor : MonoBehaviour
{
    public GameObject pointer;
    public void ShowPointer(bool status)
    {
        pointer.SetActive(status);
    }
    public  void CheckHasFixed()
    {
        if(GameManagerNew.Instance.PictureUIManager.hasFixed == true)
        {
            ShowPointer(false);
        }
        else
        {
            ShowPointer(true);
        }
    }
}
