using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "item", menuName = "ScriptableObjects/item", order = 1)]
public class Item : ScriptableObject
{
    public Sprite itemImg;
    public string itemName;

}
