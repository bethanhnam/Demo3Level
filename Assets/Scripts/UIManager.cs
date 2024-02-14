
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
    public ChetPanel chestPanel;
    public Winpop winPanel;

    public TextMeshProUGUI[] purpleStar;
    public TextMeshProUGUI[] goldenStar;
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
        foreach (var item in purpleStar)
        {
            item.text = this.GetComponent<SaveSystem>().purpleStar.ToString();
        }
		foreach (var item in goldenStar)
		{
			item.text = this.GetComponent<SaveSystem>().goldenStar.ToString();
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
	public void BuyGoddenStar(Product product)
    {
        SaveSystem.instance.goldenStar += (int)product.definition.payout.quantity;
        SaveSystem.instance.SaveData();
        SaveSystem.instance.LoadData();
	}
}
