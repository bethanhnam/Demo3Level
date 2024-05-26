using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompletePanel : MonoBehaviour
{
	public RectTransform claimPanel;
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("Winpop");
			claimPanel.gameObject.SetActive(true);
			LevelManagerNew.Instance.ResetLevel();
			
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			claimPanel.gameObject.SetActive(false);
			if (GameManagerNew.Instance.CheckLevelStage())
			{
				LevelManagerNew.Instance.stage = LevelManagerNew.Instance.stageList.Count - 1;
				GameManagerNew.Instance.CreateLevel(LevelManagerNew.Instance.stage);

            }
			else
			{
				GameManagerNew.Instance.RecreatePicAfterCompleteGame();
			}
			this.gameObject.SetActive(false);
			SaveSystem.instance.SaveData();
			if(UIManagerNew.Instance.CongratPanel.gameObject.activeSelf == true)
			{
				UIManagerNew.Instance.CongratPanel.Close();
			}
			
		}
	}
}
