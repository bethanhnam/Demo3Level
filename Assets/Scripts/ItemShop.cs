using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public PackName namePack;
    public int magicTicket;
    public int powerTicket;
    public TextMeshProUGUI magicValueText;
    public TextMeshProUGUI powerValueText;
    public TextMeshProUGUI PriceText;
    public bool nonADS;

    public void ClickMe()
    {
        IapControl.Instance.BuyProductID(namePack, () =>
        {
            SaveSystem.instance.addTiket(this.powerTicket,this.magicTicket);
            if (nonADS)
            {
                SaveSystem.instance.nonAds = 1;
                SaveSystem.instance.SaveData();
            }
            
        }); 
    }
    public void BuyByGold()
    {
        
            SaveSystem.instance.addTiket(this.powerTicket, this.magicTicket);
            if (nonADS)
            {
                SaveSystem.instance.nonAds = 1;
                SaveSystem.instance.SaveData();
            }


    }

    void Start()
    {
        PriceText.text = IapControl.Instance.getPrice(namePack);
        if (magicTicket >0 && magicValueText != null)
        {
            magicValueText.text = magicTicket.ToString();
        }
        else if(powerTicket > 0 && powerValueText != null) {
            powerValueText.text = powerTicket.ToString();
        }
    }


}
