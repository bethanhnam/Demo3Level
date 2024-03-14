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


    public void ClickMe()
    {
        IapControl.Instance.BuyProductID(namePack, () =>
        {
            SaveSystem.instance.addTiket(this.powerTicket,this.magicTicket);
            
        }); 
    }

    void Start()
    {
        if (magicTicket > powerTicket)
        {
            valueText.text = magicTicket.ToString();
        }
        else
        {
            valueText.text = powerTicket.ToString();
        }
        PriceText.text = IapControl.Instance.getPrice(namePack);
    }


}
