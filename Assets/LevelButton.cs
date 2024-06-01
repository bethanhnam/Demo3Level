using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
	public int level;
	public TextMeshProUGUI itemStar;
	[SerializeField]
	private int star;
	public bool isWindow;
	private void Update()
	{
    }
	public void SetItem()
	{
		GameManagerNew.Instance.Level = level;
		GameManagerNew.Instance.CheckStarValue(star,this.transform);

    }
	public void SetStarText( int star)
	{
		this.star = star;
		itemStar.text = star.ToString();
	}
}
