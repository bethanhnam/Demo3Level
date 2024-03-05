using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChetPanel : MonoBehaviour
{
	// Start is called before the first frame update
	public int value =0;
	public Slider[] slider = new Slider[2];
	void Start()
	{
	}
	private void OnEnable()
	{
		if (slider[0].value < slider[0].maxValue)
		{
			slider[0].value = SaveSystem.instance.level + 1;	
			slider[1].value = SaveSystem.instance.level;	
		}
		else
		{
			
		}
	}
	// Update is called once per frame
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			UIManager.instance.gamePlayPanel.Close();
			this.gameObject.SetActive(true);

		}
		if (slider[0].value == slider[0].maxValue)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.congratPanel.Open();
			slider[0].value = 0;
			slider[1].value = 0;
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			if (slider[0].value == slider[0].maxValue)
			{
				//UIManager.instance.congratPanel.Open();
			}
			else
			{
				UIManager.instance.gamePlayPanel.backFromChestPanel = true;
				UIManager.instance.gamePlayPanel.backFromPause = false;
				UIManager.instance.gamePlayPanel.Open();
			}
		}
	}
}
