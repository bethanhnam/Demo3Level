using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerNew : MonoBehaviour
{
    public static UIManagerNew Instance;

    [SerializeField]
    private ButtonMennuManager buttonMennuManager;
    [SerializeField]
    private DailyRWUI dailyRWUI;
    [SerializeField]
    private RePlayPanel rePlayPanel;
    [SerializeField]
    private DeteleNailPanel deteleNailPanel;
    [SerializeField]
    private UndoPanel undoPanel;
    [SerializeField]
    private ExtralHole extralHolePanel;
    [SerializeField]
    private NonAdsPanel nonAdsPanel;
    [SerializeField]
    private SettingPanel settingPanel;
    [SerializeField]
    private PausePanel pausePanel;
    [SerializeField]
    private ShopPanel shopPanel;
    [SerializeField]
    private LosePanel losePanel;
    [SerializeField]
    private GamePlayPanelUIManager gamePlayPanel;
    [SerializeField]
    private RestorePanel restorePanel;
    [SerializeField]
    private WinUI winUI;
    [SerializeField]
    private ChestSLider chestSLider;
    [SerializeField]
    private CompletePicImg completeImg;
    [SerializeField]
    private CompleteUI completeUI;
    [SerializeField]
    private Ratting rattingPanel;
    [SerializeField]
    private CongratPanel congratPanel;
    [SerializeField]
    private CompletePanel completePanel;
    [SerializeField]
    private NotEnoughStar notEnoughStarPanel;
    [SerializeField]
    private FixItemUI fixItemUI;
    [SerializeField]
    private TransferPanel transferPanel;
    [SerializeField]
    private videoLoading videoLoaingPanel;
    [SerializeField]
    private NewBooster newBooster;
    [SerializeField]
    private ThresholeController thresholeController;
    [SerializeField]
    private MiniGamePlay miniGamePlay;
    [SerializeField]
    private StartMiniGamePanel startMiniGamePanel;
    [SerializeField]
    private WinMiniGamePanel winMiniGamePanel;
    [SerializeField]
    private SkipPanel skipPanel;
    [SerializeField]
    private GameObject PointerObject;
    [SerializeField]
    private StartWeeklyEvent startWeeklyEvent;

    // weekly event
    [SerializeField]
    private TreasureClimbPanel treasureClimbPanel;
    [SerializeField]
    private WeeklyEventPanel weeklyEventPanel;


    public TextMeshProUGUI[] DrillTexts;
    public TextMeshProUGUI[] UnscrewTexts;
    public TextMeshProUGUI[] UndoTexts;
    public TextMeshProUGUI[] coinTexts;
    public TextMeshProUGUI[] starTexts;
    public TextMeshProUGUI playBTLevelTexts;

    [SerializeField]
    private GameObject onjBlockAds;

    [SerializeField]
    private GameObject blockPicCanvas;

    [SerializeField]
    private GameObject highCanvas;

    [SerializeField]
    private GamePlayLoading gamePlayLoading;

    [SerializeField]
    private StoryItem storyItem;

    [SerializeField]
    private BackGroundFooter backGroundFooter;

    [SerializeField]
    private Material highLightObj;

    public ButtonMennuManager ButtonMennuManager { get => buttonMennuManager; }
    public DailyRWUI DailyRWUI { get => dailyRWUI; }
    public GamePlayPanelUIManager GamePlayPanel { get => gamePlayPanel; }
    public WinUI WinUI { get => winUI; }
    public RePlayPanel RePlayPanel { get => rePlayPanel; set => rePlayPanel = value; }
    public DeteleNailPanel DeteleNailPanel { get => deteleNailPanel; set => deteleNailPanel = value; }
    public UndoPanel UndoPanel { get => undoPanel; set => undoPanel = value; }
    public SettingPanel SettingPanel { get => settingPanel; set => settingPanel = value; }
    public PausePanel PausePanel { get => pausePanel; set => pausePanel = value; }
    public ExtralHole ExtralHolePanel { get => extralHolePanel; set => extralHolePanel = value; }
    public LosePanel LosePanel { get => losePanel; set => losePanel = value; }
    public ChestSLider ChestSLider { get => chestSLider; set => chestSLider = value; }
    public CompletePicImg CompleteImg { get => completeImg; set => completeImg = value; }
    public NonAdsPanel NonAdsPanel { get => nonAdsPanel; set => nonAdsPanel = value; }
    public ShopPanel ShopPanel { get => shopPanel; set => shopPanel = value; }
    public RestorePanel RestorePanel { get => restorePanel; set => restorePanel = value; }
    public CompleteUI CompleteUI { get => completeUI; }
    public Ratting RattingPanel { get => rattingPanel; set => rattingPanel = value; }
    public CongratPanel CongratPanel { get => congratPanel; set => congratPanel = value; }
    public CompletePanel CompletePanel { get => completePanel; set => completePanel = value; }
    public FixItemUI FixItemUI { get => fixItemUI; set => fixItemUI = value; }
    public NotEnoughStar NotEnoughStarPanel { get => notEnoughStarPanel; set => notEnoughStarPanel = value; }
    public TransferPanel TransferPanel { get => transferPanel; set => transferPanel = value; }
    public GameObject BlockPicCanvas { get => blockPicCanvas; set => blockPicCanvas = value; }
    public GamePlayLoading GamePlayLoading { get => gamePlayLoading; set => gamePlayLoading = value; }
    public videoLoading VideoLoaingPanel { get => videoLoaingPanel; set => videoLoaingPanel = value; }
    public GameObject HighCanvas { get => highCanvas; set => highCanvas = value; }
    public StoryItem StoryItem { get => storyItem; set => storyItem = value; }
    public NewBooster NewBooster { get => newBooster; set => newBooster = value; }
    public ThresholeController ThresholeController { get => thresholeController; set => thresholeController = value; }
    public MiniGamePlay MiniGamePlay { get => miniGamePlay; set => miniGamePlay = value; }
    public StartMiniGamePanel StartMiniGamePanel { get => startMiniGamePanel; set => startMiniGamePanel = value; }
    public WinMiniGamePanel WinMiniGamePanel { get => winMiniGamePanel; set => winMiniGamePanel = value; }
    public SkipPanel SkipPanel { get => skipPanel; set => skipPanel = value; }
    public GameObject PointerObject1 { get => PointerObject; set => PointerObject = value; }
    public BackGroundFooter BackGroundFooter { get => backGroundFooter; set => backGroundFooter = value; }
    public Material HighLightObj { get => highLightObj; set => highLightObj = value; }

    public TreasureClimbPanel TreasureClimbPanel { get => treasureClimbPanel; set => treasureClimbPanel = value; }
    public WeeklyEventPanel WeeklyEventPanel { get => weeklyEventPanel; set => weeklyEventPanel = value; }
    public StartWeeklyEvent StartWeeklyEvent { get => startWeeklyEvent; set => startWeeklyEvent = value; }

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        
    }
    private void Start()
    {
        LoadData(SaveSystem.instance.unscrewPoint, SaveSystem.instance.undoPoint,SaveSystem.instance.extraHolePoint, SaveSystem.instance.coin, SaveSystem.instance.star);
    }
    public void LoadData(int unscrew, int undo,int drill,  int coin, int star)
    {
        for (int i = 0; i < UnscrewTexts.Length; i++)
        {
            UnscrewTexts[i].SetText(unscrew.ToString());
            UndoTexts[i].SetText(undo.ToString());
            DrillTexts[i].SetText(drill.ToString());
        }
        for (int j = 0; j < coinTexts.Length; j++)
        {
            coinTexts[j].SetText(coin.ToString());
        }
        for (int k = 0; k < starTexts.Length; k++)
        {
            starTexts[k].SetText(star.ToString());
        }
    }

    public void ActiveBlockFaAds(bool value)
    {
        if (onjBlockAds.activeInHierarchy != value)
        {
            onjBlockAds.SetActive(value);
        }
    }

    public void ShowPoiner(Transform targetTransform)
    {
        PointerObject.gameObject.SetActive(true);
        PointerObject.transform.position = new Vector3(targetTransform.position.x - 1, targetTransform.position.y - 1, 1);
    }
    public void DisablePointer()
    {
        PointerObject.gameObject.SetActive(false);
    }
}