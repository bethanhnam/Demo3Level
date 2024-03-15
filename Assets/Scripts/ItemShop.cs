using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public PackName namePack;
    public int magicTicket;
    public int powerTicket;
    public TextMeshProUGUI valueText;
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

    void Start()
    {
        if (magicTicket >0 && magicTicket > powerTicket)
        {
            valueText.text = magicTicket.ToString();
        }
        else if(powerTicket > 0 && powerTicket > magicTicket) { 
            valueText.text = powerTicket.ToString();
        }
        PriceText.text = IapControl.Instance.getPrice(namePack);
    }


}
