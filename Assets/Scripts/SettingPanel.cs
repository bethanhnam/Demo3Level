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
	public RectTransform Blockpanel;
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
			Blockpanel.gameObject.SetActive(true);
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			panel.localRotation = Quaternion.identity;
			this.GetComponent<CanvasGroup>().alpha = 0;
			GameManager.instance.hasUI = true;
			panel.localPosition = new Vector3(-351, 479, 0);
			panel.localScale = new Vector3(.8f, .8f, 0);
			closeButton.localPosition = new Vector3(364, 277.600006f, 0);
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
			panel.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			closeButton.DOAnchorPos(new Vector2(552f, -105f), .1f, false).OnComplete(() =>
				{
					panel.DORotate(new Vector3(0, 0, -10f), 0.25f, RotateMode.Fast).OnComplete(() =>
					{
						panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), 0.25f, false).OnComplete(() =>
						{
							this.gameObject.SetActive(false);
							UIManager.instance.menuPanel.Open();
							Blockpanel.gameObject.SetActive(false);
							AudioManager.instance.PlaySFX("ClosePopUp");
							GameManager.instance.hasUI = false;
						});
					});
				});
		}
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
