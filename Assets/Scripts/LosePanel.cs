using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LosePanel : MonoBehaviour
{
	public CanvasGroup canvasGroup;
	public RectTransform Blockpanel;
	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WatchAd()
    {
		// load ad 
		Close();
		UIManager.instance.gamePlayPanel.timer.SetTimer(61f);
		
	}
	public void Replay()
    {
		// load ad 
		Close();
		GameManager.instance.Replay();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			Blockpanel.gameObject.SetActive(true);
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
				UIManager.instance.gamePlayPanel.timer.TimerOn = true;
				UIManager.instance.gamePlayPanel.backFromPause = true;
				UIManager.instance.gamePlayPanel.Open();
			});
		}
	}
}
