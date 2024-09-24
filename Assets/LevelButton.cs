using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
	public int level;
	public TextMeshProUGUI itemStar;
	public int star;
	public bool isWindow;
	private void Update()
	{
    }
	public void SetItem()
	{
        UIManagerNew.Instance.BlockPicCanvas.SetActive(true);
        GameManagerNew.Instance.Level = level;
		GameManagerNew.Instance.CheckStarValue(star,this.transform.position,this);

    }
	public void SetStarText( int star)
	{
		this.star = star;
		itemStar.text = star.ToString();
	}
	public void ActiveSecondPointer()
	{
		UIManagerNew.Instance.ThresholeController.SetSecondItemButton();
	}
}
