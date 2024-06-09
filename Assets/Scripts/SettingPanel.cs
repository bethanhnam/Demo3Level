using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class SettingPanel : MonoBehaviour
{
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject musicOn;
    public GameObject musicOff;
    public GameObject alertOn;
    public GameObject alertOff;
    public RectTransform closeButton;
    public RectTransform panel;
    public CanvasGroup canvasGroup;
    public GameObject panelBoard;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Open()
    {
        if (!this.gameObject.activeSelf)
        {
            this.gameObject.SetActive(true);
            panelBoard.gameObject.SetActive(false);
            panelBoard.transform.localScale = new Vector3(.9f, .9f, 1f);
            AudioManager.instance.PlaySFX("OpenPopUp");
            canvasGroup.alpha = 0;
            canvasGroup.DOFade(1, .3f);
            panelBoard.gameObject.SetActive(true);
            panelBoard.transform.DOScale(new Vector3(1.1f, 1.1f, 1f), 0.2f).OnComplete(() =>
            {
                panelBoard.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {

                });
            });
        }
    }
    public void Close()
    {
        if (this.gameObject.activeSelf)
        {
            canvasGroup.alpha = 1;
            canvasGroup.DOFade(0, .3f).OnComplete(() =>
            {
                this.gameObject.SetActive(false);
                AudioManager.instance.PlaySFX("ClosePopUp");
            });
            panelBoard.transform.DOScale(new Vector3(.9f, .9f, 1f), 0.3f);
            Invoke("ActivehomeUI", 0.1f);
        }
    }

    private void ActivehomeUI()
    {
        UIManagerNew.Instance.ButtonMennuManager.Appear();
    }

    public void SoundOn()
    {
        soundOff.SetActive(false);
        soundOn.SetActive(true);
        AudioManager.instance.sfxSource.enabled = true;
    }
    public void SoundOff()
    {
        soundOn.SetActive(false);
        soundOff.SetActive(true);
        AudioManager.instance.sfxSource.enabled = false;
    }
    public void MusicOn()
    {
        musicOff.SetActive(false);
        musicOn.SetActive(true);
        AudioManager.instance.musicSource.enabled = true;
    }
    public void MusicOff()
    {
        musicOn.SetActive(false);
        musicOff.SetActive(true);
        AudioManager.instance.musicSource.enabled = false;
    }
    public void AlertOn()
    {
        alertOff.SetActive(false);
        alertOn.SetActive(true);
    }
    public void AlertOff()
    {
        alertOn.SetActive(false);
        alertOff.SetActive(true);
    }
}
