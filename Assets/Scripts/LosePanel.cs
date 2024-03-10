using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using UnityEngine.UI;
using Unity.VisualScripting;

[RequireComponent(typeof(CanvasGroup))]
public class LosePanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;
	public bool hasUse = false;
	public RectTransform watchAdButton;
	// Start is called before the first frame update
	void Start()
    {
		watchAdButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
	}

    // Update is called once per frame
    void Update()
    {
		if (hasUse)
		{
			watchAdButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
		}
		else
		{
			watchAdButton.GetComponent<UnityEngine.UI.Button>().interactable = true;
		}
    }
    public void WatchAd()
    {
		// load ad 
		if (!hasUse)
		{
			Close();
			UIManager.instance.gamePlayPanel.timer.SetTimer(61f);
			hasUse = true;
			watchAdButton.GetComponent<UnityEngine.UI.Button>().interactable = false;
		}
		
	}
	public void Replay()
    {
		// load ad 
		Close();
		GameManager.instance.Retry();
		hasUse = false;
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("LosePop");
			Blockpanel.gameObject.SetActive(true);
			GameManager.instance.hasUI = true;
			UIManager.instance.gamePlayPanel.Close();
			canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, .3f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
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
				Blockpanel.gameObject.SetActive(false);
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				UIManager.instance.gamePlayPanel.timer.TimerOn = true;
				UIManager.instance.gamePlayPanel.backFromPause = true;
				UIManager.instance.gamePlayPanel.Open();
				GameManager.instance.hasUI = false;
			});
		}
	}
}
