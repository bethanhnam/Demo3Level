using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WelcomePresent : MonoBehaviour
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
        animButton.Play(disappearButton);
    }
    public void Deactive()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        UIManagerNew.Instance.ButtonMennuManager.CheckForMinigame();
    }
    public void ActiveCVGroup()
    {
        if (!cvButton.blocksRaycasts)
        {
            cvButton.blocksRaycasts = true;
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
    public void ClanimPresent()
    {
        AudioManager.instance.PlaySFX("FillUpSlider");
        SaveSystem.instance.AddBooster(2, 2, 2);
        SaveSystem.instance.SaveData();
    }
}
