using DG.Tweening;
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
        characterImage.sprite = sprites[i];
        bubbleContentText.transform.localScale = Vector3.zero;
        bubbleContentText.gameObject.SetActive(true);
        bubbleContentText.transform.DOScale(Vector3.one, 1).OnComplete(() =>
        {
            bubbleContentText.text = stringText;
            characterImage.gameObject.SetActive(true);
        });

        this.transform.gameObject.SetActive(true);
    }
    public void SetToPlayButton()
    {
        UIManagerNew.Instance.ButtonMennuManager.playButton.gameObject.SetActive(true);
        this.transform.gameObject.SetActive(false);

        bubbleContentText.transform.localScale = Vector3.one;
        characterImage.gameObject.SetActive(false);
        bubbleContentText.gameObject.SetActive(true);
        bubbleContentText.transform.DOScale(Vector3.zero, 1).OnComplete(() =>
        {
            bubbleContentText.gameObject.SetActive(false);
            characterImage.gameObject.SetActive(true);
        });
    }
}
