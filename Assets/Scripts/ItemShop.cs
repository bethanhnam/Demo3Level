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

    void Start()
    {
        if (magicTicket >0)
        {
            magicValueText.text = magicTicket.ToString();
        }
        else if(powerTicket > 0) {
            powerValueText.text = powerTicket.ToString();
        }
        PriceText.text = IapControl.Instance.getPrice(namePack);
    }


}
