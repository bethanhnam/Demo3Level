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
	private Image completeImg;
	[SerializeField]
	private CompleteUI completeUI;
	[SerializeField]
	private Ratting rattingPanel;
	[SerializeField]
	private CongratPanel congratPanel;
	[SerializeField]
	private CompletePanel completePanel;

	public TextMeshProUGUI[] powerTexts;
	public TextMeshProUGUI[] magicTexts;

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
	public Image CompleteImg { get => completeImg; set => completeImg = value; }
	public NonAdsPanel NonAdsPanel { get => nonAdsPanel; set => nonAdsPanel = value; }
	public ShopPanel ShopPanel { get => shopPanel; set => shopPanel = value; }
	public RestorePanel RestorePanel { get => restorePanel; set => restorePanel = value; }
	public CompleteUI CompleteUI { get => completeUI; }
	public Ratting RattingPanel { get => rattingPanel; set => rattingPanel = value; }
	public CongratPanel CongratPanel { get => congratPanel; set => congratPanel = value; }
	public CompletePanel CompletePanel { get => completePanel; set => completePanel = value; }

	private void Awake()
	{
		Instance = this;
	}
	private void Update()
	{
		LoadTicket(SaveSystem.instance.magicTiket, SaveSystem.instance.powerTicket);
	}
	public void LoadTicket(int magicTiket,int powerTicket)
	{
		for (int i = 0;i < magicTexts.Length;i++)
		{
			magicTexts[i].SetText(magicTiket.ToString());
			powerTexts[i].SetText(powerTicket.ToString());
		}
	}
}
