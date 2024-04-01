using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using DG.Tweening;

public class RestorePanel : MonoBehaviour
{
	public GameObject Blockpanel;
	public TextMeshProUGUI reportText;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			GameManager.instance.hasUI = true;
			Blockpanel.gameObject.SetActive(true);
			this.GetComponent<CanvasGroup>().alpha = 0;
			this.transform.localScale = new Vector3(.8f, .8f, 0);
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f);
			this.transform.DOScale(new Vector3(1, 1, 1), 0.1f).OnComplete(() =>
			{
				Blockpanel.gameObject.SetActive(false);
				Invoke("Close", 1f);
			});
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			Blockpanel.gameObject.SetActive(true);
			this.GetComponent<CanvasGroup>().DOFade(1, 0.1f).OnComplete(() =>
			{
				this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				GameManager.instance.hasUI = false;
				SaveSystem.instance.playingHard = true;
				Blockpanel.gameObject.SetActive(false);
			});
		}
	}
	public void getText(string text)
	{
		reportText.text = text;
	}
}
