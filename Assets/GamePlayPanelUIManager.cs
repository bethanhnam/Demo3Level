using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelUIManager : MonoBehaviour
{
	public static GamePlayPanelUIManager Instance;
	[SerializeField]
	private Animator animButton;
	[SerializeField]
	private CanvasGroup cvButton;

	private int appearButton = Animator.StringToHash("appear");
	private int disappearButton = Animator.StringToHash("disappear");


	//PopUp Button
	 public Button ReplayButton;
	 public Button UndoButton;
	 public Button DeteleButton;
	 public Button PauseButton;
	 public Button BoomButton;

	//text 
	public TextMeshProUGUI levelText;

	//time
	public Timer timer;

	//notice
	public Notice notice;
	private void Awake()
	{
		Instance = this;
	}
	private void Start()
	{
		AudioManager.instance.PlayMusic("GamePlayTheme");
	}
	public int TakeLevelText()
	{
		int level = 0;
		for (int i = 0; i < DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item.Length;i++) {
			if(DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[i].Level.ToString() == GameManagerNew.Instance.CurrentLevel.ToString())
			{
				level = DataLevelManager.Instance.DatatPictureScriptTableObjects[LevelManagerNew.Instance.LevelBase.Level].Stage[DataLevelManager.Instance.DataLevel.Data[LevelManagerNew.Instance.LevelBase.Level].IndexStage].Item[i].Id;
			}
			else
			{
				level = DataLevelManager.Instance.GetLevel();
			}
		}
		return level;
	}
	public void setText(int level)
	{ 
		if( level != 0)
		{
			levelText.text = level.ToString();
		}
        else
        {
			levelText.text = DataLevelManager.Instance.GetLevel().ToString();
        }
	}
	public void Appear()
	{
		gameObject.SetActive(true);
		cvButton.blocksRaycasts = false;
		ActiveTime();
		animButton.Play(appearButton, 0, 0);
	}
	public void AppearForCreateLevel()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.blocksRaycasts = false;
		Settimer(181);
		ActiveTime();
		animButton.Play(appearButton, 0, 0);
	}
	public void AppearForReOpen()
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		cvButton.blocksRaycasts = false;
		//GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
		ActiveTime();
		animButton.Play(appearButton, 0, 0);
	}

	public void Close(bool _destroy = false)
	{
		GameManagerNew.Instance.CloseLevel(_destroy);
		cvButton.blocksRaycasts = false;
		animButton.Play(disappearButton);
	}

	public void Deactive()
	{
		if (gameObject.activeSelf)
		{
			gameObject.SetActive(false);
		}
	}

	public void ActiveCVGroup()
	{
		if (!cvButton.blocksRaycasts)
		{
			cvButton.blocksRaycasts = true;
		}
	}


	public void OpenReplayPanel()
	{
		
		DeactiveTime();
		//Close();
		UIManagerNew.Instance.RePlayPanel.Open();
	}
	public void OpenDetelePanel()
	{
		
		DeactiveTime();
		//Close();
		UIManagerNew.Instance.DeteleNailPanel.Open();
	}
	public void OpenExtraHolePanel()
	{
		
		DeactiveTime();
		//Close();
		UIManagerNew.Instance.ExtralHolePanel.Open();
	}
	public void OpenUndoPanel()
	{
		
		DeactiveTime();
		//Close();
		UIManagerNew.Instance.UndoPanel.Open();
	}
	public void OpenPausePanel()
	{
		
		DeactiveTime();
		Close();
		UIManagerNew.Instance.PausePanel.Open();
	}
	public void OpenLosePanel()
	{
		DeactiveTime();
		Close();
		UIManagerNew.Instance.LosePanel.Open();
	}
	public void PlayButton()
	{
		int level = DataLevelManager.Instance.GetLevel();
		GameManagerNew.Instance.CreateLevel(level);
		Close();
	}
	public void ButtonOff()
	{
		ReplayButton.interactable = false;
		DeteleButton.interactable = false;
		UndoButton.	interactable = false;
	}
	public void ButtonOn()
	{
		ReplayButton.interactable = true;
		//BoomButton.interactable = true;
		DeteleButton.interactable = true;
	}
	public void Settimer(float time)
	{
		timer.SetTimer(time);
	}
	public void DeactiveTime()
	{
		timer.TimerOn = false;
	}
	public void ActiveTime()
	{
		timer.TimerOn = true;
	}
	public void ShowNotice(bool status)
	{
		if (status)
		{
			notice.canAppear = true;
			notice.NoticeAppear();
		}
		else
		{
			notice.canAppear = false;
			notice.NoticeDisappear();
		}
	}
}
