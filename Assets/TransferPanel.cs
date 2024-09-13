using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPanel : MonoBehaviour
{
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    private int appearButton = Animator.StringToHash("Appear");
    private int disappearButton = Animator.StringToHash("Disappear");

    public void Appear()
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            
        }
        cvButton.blocksRaycasts = false;
        animButton.Play(appearButton, 0, 0);
        
    }
   public void Close()
    {
        cvButton.blocksRaycasts = false;
        PlayerPrefs.SetInt("HasTransfer", 1);
        animButton.Play(disappearButton);
    }
    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
           
        }
    }
    public void ActiveCVGroup()
    {
        if (!GameManagerNew.Instance.CheckSliderValue())
        {
            if (!cvButton.blocksRaycasts)
            {
                cvButton.blocksRaycasts = true;
            }
        }
    }
    public void DiactiveCVGroup()
    {
        if (cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = false;
        }
    }
    public void Check()
    {
        if (!UIManagerNew.Instance.ButtonMennuManager.gameObject.activeSelf)
        {
            UIManagerNew.Instance.ButtonMennuManager.Appear();
        }
    }
}
