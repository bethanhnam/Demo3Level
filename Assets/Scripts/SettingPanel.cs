using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

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
			panel.localRotation = Quaternion.identity;
			panel.DOAnchorPos(new Vector3(-351, 479, 0), 1f, false).OnComplete(() =>
			{
				closeButton.DOAnchorPos(new Vector3(-71.5f, -207.8f, 0), .5f, false).OnComplete(() => {
					Blockpanel.gameObject.SetActive(false);
				});
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			closeButton.DOAnchorPos(new Vector2(552f, -105f), 1f, false).OnComplete(() =>
				{
					panel.DORotate(new Vector3(0, 0, -10f), .3f, RotateMode.Fast).OnComplete(() =>
					{
						panel.DOAnchorPos(new Vector2(panel.transform.position.x, -1467f), .5f, false).OnComplete(() =>
						{
							this.gameObject.SetActive(false);
							UIManager.instance.menuPanel.Open();
							Blockpanel.gameObject.SetActive(false);
						});
					});
				});
		}
	}
	public void SoundOn()
	{
		soundOff.SetActive(false);
		soundOn.SetActive(true);
	}
	public void SoundOff()
	{
		soundOn.SetActive(false);
		soundOff.SetActive(true);
	}
	public void MusicOn()
	{
		musicOff.SetActive(false);
		musicOn.SetActive(true);
	}
	public void MusicOff()
	{
		musicOn.SetActive(false);
		musicOff.SetActive(true);
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
