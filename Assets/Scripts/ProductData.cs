using UnityEngine;

[CreateAssetMenu(fileName = "ItemShopDatabase", menuName = "Shopping/Item Shop database")]
public class ProductData : ScriptableObject
{
	public string productName;
	public float price;
	public string description;
	public PackName packName;
}