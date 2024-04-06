
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Purchasing;

public class UIManager : MonoBehaviour
{
	public int targetFPS = 60;
	
    // Start is called before the first frame update
    public static UIManager instance;
    public MenuPanel menuPanel;
    public GamePlayPanel gamePlayPanel;
    public ShopPanel shopPanel;
    public SettingPanel settingPanel;
   
    public Winpop winPanel;
    public CongratPanel congratPanel;
    public CompletePanel completePanel;

   

    public TextMeshProUGUI[] magicTiket;
    public TextMeshProUGUI[] powerTicket;
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = targetFPS;
	}

    // Update is called once per frame
    void Update()
    {
        foreach (var item in magicTiket)
        {
            item.text = this.GetComponent<SaveSystem>().magicTiket.ToString();
        }
		foreach (var item in powerTicket)
		{
			item.text = this.GetComponent<SaveSystem>().powerTicket.ToString();
		}
        
    }
	public void OpenSettingPanel()
	{
		settingPanel.Open();
	}
    public void CloseSettingPanel()
    {
        settingPanel.Close();
        this.menuPanel.Open();
    }
    public void DeactiveTime()
    {
		gamePlayPanel.timer.TimerOn = false;
		if (Level.instance.stage > 0)
        {
			SaveSystem.instance.playingHard = false;
		}
	}
    public void ActiveTime()
    {
		gamePlayPanel.timer.TimerOn = true;
		if (Level.instance.stage > 0)
		{
			SaveSystem.instance.playingHard = true;
		}
	}
	public void BuyGoddenStar(Product product)
    {
        SaveSystem.instance.powerTicket += (int)product.definition.payout.quantity;
        SaveSystem.instance.SaveData();
        SaveSystem.instance.LoadData();
	}
}
