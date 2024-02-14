using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ChetPanel : MonoBehaviour
{
	// Start is called before the first frame update
	public int value =0;
	public Slider slider;
	void Start()
	{

	}
	private void OnEnable()
	{
		if (slider.value < slider.maxValue)
		{
			slider.value = SaveSystem.instance.level + 1;
		}
		else
		{
			UIManager.instance.winPanel.Open();
		}
	}
	// Update is called once per frame
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			UIManager.instance.gamePlayPanel.Close();
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			if (slider.value == slider.maxValue)
			{
				UIManager.instance.winPanel.Open();
			}
			else
			{
				UIManager.instance.gamePlayPanel.backFromChestPanel = true;
				UIManager.instance.gamePlayPanel.Open();
			}
		}
	}
}
