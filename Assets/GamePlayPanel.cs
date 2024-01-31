using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayPanel : MonoBehaviour
{
	public PausePanel pausePanel;
	public RePlayPanel rePlayPanel;
	public HintPanel hintPanel;
	public DeteleNailPanel deteleNailPanel;
	public HintImgPanel hintImgPanel;

	public void OpenPausePanel()
	{
		pausePanel.Open();
	}
	public void ShowHint()
	{
		hintImgPanel.Open();
		hintImgPanel.LoadHint();
	}
	public void OpenReplayPanel()
	{
		rePlayPanel.Open();
	}
	public void OpenHintPanel()
	{
		hintPanel.Open();
	}
	public void OpenDeteleNailPanel()
	{
		deteleNailPanel.Open();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
		}
	}
}
