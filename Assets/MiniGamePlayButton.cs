using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamePlayButton : MonoBehaviour
{
    public GameObject questButton;
    public GameObject bubbleText;
    public string[] ContentText;
    public TextMeshProUGUI bubbleContentText;
    public Sprite[] sprites;
    public Image characterImage;

    public void SetQuestButton(int i,string stringText)
    {
        UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(false);
        bubbleContentText.gameObject.SetActive(true);
        bubbleContentText.text = stringText;
        characterImage.sprite = sprites[i];
        this.transform.gameObject.SetActive(true);
    }
    public void DisableBubbleText()
    {
        bubbleText.gameObject.SetActive(false);
    }
}
