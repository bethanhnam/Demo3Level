using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayPanelUIManager : MonoBehaviour
{
    public static GamePlayPanelUIManager Instance;
    [SerializeField]
    private Animator animButton;
    [SerializeField]
    private CanvasGroup cvButton;

    private int appearButton1 = Animator.StringToHash("await");
    private int appearButton = Animator.StringToHash("appear");
    private int disappearButton = Animator.StringToHash("disappear");

    //PopUp Button
    public Button ReplayButton;
    public Button UndoButton;
    public Button DeteleButton;
    public Button PauseButton;
    public Button BoomButton;

    // anim Button
    public Animator UndoButtonAim;
    public Animator DeteleButtonAim;

    //text 
    public TextMeshProUGUI levelText;

    //time
    public Timer timer;

    //notice
    public Notice notice;

    //boosterbar
    public BoosterBar boosterBar;

    //blackPic
    public Image blackPic;
    public Image blockPic;

    //pointer
    public GameObject goodJob;
    public bool hasOpen;

    //Drill Effect
    public SkeletonGraphic drillEffect;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            AudioManager.instance.PlayMusic("GamePlayTheme");
        });
    }
    public void setText(int level)
    {
        if (level != 0)
        {
            levelText.text = level.ToString();
        }
        else
        {
            levelText.text = DataLevelManager.Instance.GetLevel().ToString();
        }
    }
    private void OnEnable()
    {
        if (LevelManagerNew.Instance.stage >= 3)
        {
           boosterBar.gameObject.SetActive(true);
        }
        else
        {
            boosterBar.gameObject.SetActive(false);
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
        animButton.Play(appearButton1, 0, 0);
        cvButton.blocksRaycasts = false;
        Settimer(181);
        DOVirtual.DelayedCall(1f, () =>
        {
            DOVirtual.DelayedCall(1f, () =>
            {
                if (LevelManagerNew.Instance.stage != 1 && LevelManagerNew.Instance.stage != 3)
                {
                    ActiveTime();
                }
            });
            animButton.Play(appearButton, 0, 0);
        });
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
        Close();
        UIManagerNew.Instance.RePlayPanel.Open();
    }
    public void OpenDetelePanel()
    {

        DeactiveTime();
        GameManagerNew.Instance.CloseLevel(false);
        Close();
        UIManagerNew.Instance.DeteleNailPanel.Open();
    }
    public void OpenExtraHolePanel()
    {

        DeactiveTime();
        Close();
        GameManagerNew.Instance.CloseLevel(false);
        UIManagerNew.Instance.ExtralHolePanel.Open();
    }
    public void OpenUndoPanel()
    {

        DeactiveTime();
        Close();
        GameManagerNew.Instance.CloseLevel(false);
        UIManagerNew.Instance.UndoPanel.Open();
    }
    public void OpenPausePanel()
    {

        DeactiveTime();
        Close();
        GameManagerNew.Instance.CloseLevel(false);
        UIManagerNew.Instance.PausePanel.Open();
    }
    public void OpenLosePanel()
    {
        DeactiveTime();
        Close();
        GameManagerNew.Instance.CloseLevel(false);
        UIManagerNew.Instance.LosePanel.Open();
    }
    public void PlayButton()
    {
        int level = DataLevelManager.Instance.GetLevel();
        //GameManagerNew.Instance.CreateLevel(level);
        if (GameManagerNew.Instance.CheckLevelStage())
        {
            UIManagerNew.Instance.ButtonMennuManager.OpenCompletePanel();
        }
        else
        {
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            UIManagerNew.Instance.GamePlayLoading.appear();
            DOVirtual.DelayedCall(.7f, () =>
            {
                UIManagerNew.Instance.GamePlayPanel.AppearForCreateLevel();
                if (PlayerPrefs.GetInt("HasCompleteLastLevel") == 1)
                {
                    int replayLevel = UnityEngine.Random.Range(5, 29);
                    GameManagerNew.Instance.CreateLevel(replayLevel);
                }
                else
                {
                    GameManagerNew.Instance.CreateLevel(level);
                }
            });
            GameManagerNew.Instance.CloseLevel(false);
            Close();
        }
    }

    public void ButtonOff()
    {
        ReplayButton.interactable = false;
        DeteleButton.interactable = false;
        UndoButton.interactable = false;
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
        blockPic.gameObject.SetActive(true);
        timer.TimerOn = false;
    }
    public void ActiveTime()
    {
        blockPic.gameObject.SetActive(false);
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
    public void ActiveBlackPic(bool status)
    {
        blackPic.gameObject.SetActive(status);
    }
    public void activeAnimation(Animator button,bool status)
    {
        button.enabled = status;
    }
    public void ShowDrillEffect(Action action)
    {
        drillEffect.transform.position = new Vector3(Stage.Instance.holeToUnlock.transform.position.x, Stage.Instance.holeToUnlock.transform.position.y-0.3f, Stage.Instance.holeToUnlock.transform.position.z);
        drillEffect.gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("ExtraHole");
        DOVirtual.DelayedCall(1.5f, () =>
        {
            drillEffect.gameObject.SetActive(false);
            action();
        });
    }
}
