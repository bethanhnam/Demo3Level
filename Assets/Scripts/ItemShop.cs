using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemShop : MonoBehaviour
{
    public NewDataPackName namePack;
    public int unscrewPoint;
    public int undoPoint;
    public int gold;
    public TextMeshProUGUI magicValueText;
    public TextMeshProUGUI powerValueText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI PriceText;
    public bool nonADS;

    public void ClickMe()
    {
        IapControl.Instance.BuyProductID(namePack, () =>
        {
           
            SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint);
            SaveSystem.instance.addCoin(this.gold);
            SaveSystem.instance.SaveData();
            if (nonADS)
            {
                SaveSystem.instance.nonAds = 1;
                SaveSystem.instance.SaveData();
            }
            
        }); 
    }
    public void BuyByGold()
    {
        
            SaveSystem.instance.AddBooster(this.unscrewPoint, this.undoPoint);
            if (nonADS)
            {
                SaveSystem.instance.nonAds = 1;
                SaveSystem.instance.SaveData();
            }


    }

    void Start()
    {
        PriceText.text = IapControl.Instance.getPrice(namePack);
        if (unscrewPoint > 0 && magicValueText != null)
        {
            magicValueText.text = unscrewPoint.ToString();
        }
        if(undoPoint > 0 && powerValueText != null) {
            powerValueText.text = undoPoint.ToString();
        } 
        if(gold > 0 && goldText != null) {
            goldText.text = gold.ToString();
        }
    }
}
