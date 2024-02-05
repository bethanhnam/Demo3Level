using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="ItemShopDatabase",menuName = "Shopping/Item Shop database")]
public class ScriptableItem : ScriptableObject
{
    public string itemName;
    public int itemPrice;
    public Sprite iteamSprite;
    public int valueText;
}
