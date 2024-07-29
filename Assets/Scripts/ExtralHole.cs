using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

[RequireComponent(typeof(CanvasGroup))]
public class ExtralHole : MonoBehaviour
{
	public string layerName = "Hole";
	public ExtraHoleButton extraHoleButton;
	public RectTransform closeButton;
	public RectTransform panel;
	public rankpanel notEnoughpanel;
	public CanvasGroup canvasGroup;


    //text 
    public TextMeshProUGUI minusText;

    private void Start()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}
	private void Update()
	{
	}
	public void UseTicket()
	{
		if (SaveSystem.instance.extraHolePoint >= 1)
		{
            FirebaseAnalyticsControl.Instance.LogEventLevelItem(LevelManagerNew.Instance.stage, LevelItem.drill);
            UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(true);
            SetMinusText('-', 1);
            SaveSystem.instance.AddBooster(0,0,-1);
			SaveSystem.instance.SaveData();
			Stage.Instance.ChangeLayer();
			Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
			Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
            DOVirtual.DelayedCall(1f, () =>
            {
                UIManagerNew.Instance.BlockPicCanvas.gameObject.SetActive(false);
                this.Close();
            });
        }
		else
		{
			notEnoughpanel.ShowDialog();
		}
	}
	public void WatchAd()
	{
		AdsManager.instance.ShowRewardVideo(() =>
		{
            // load ad 
            this.Close();
			Stage.Instance.ChangeLayer();
			Stage.Instance.holeToUnlock.GetComponent<Hole>().extraHole = false;
			Stage.Instance.holeToUnlock.GetComponent<ExtraHoleButton>().myButton.gameObject.SetActive(false);
		});
		
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			canvasGroup.blocksRaycasts = false;
            panel.localScale = new Vector3(.8f, .8f, 1f);
            canvasGroup.alpha = 0;
			canvasGroup.DOFade(1, 0.1f);
            panel.transform.DOScale(new Vector3(1.05f, 1.05f, 1f), 0.2f).OnComplete(() =>
            {
                panel.transform.DOScale(Vector3.one, 0.15f).OnComplete(() =>
                {
                    ActiveCVGroup();
                    //GamePlayPanelUIManager.Instance.Close();
                });
            });
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			canvasGroup.blocksRaycasts = false;
            canvasGroup.DOFade(0, 0.1f);
            panel.DOScale(new Vector3(0.8f, 0.8f, 0), 0.1f).OnComplete(() =>
            {
                AudioManager.instance.PlaySFX("ClosePopUp");
                GamePlayPanelUIManager.Instance.ActiveTime();
                if (Stage.Instance.isWining)
                {
                    Stage.Instance.ScaleUpStage();
                }
                else
                {
                    GamePlayPanelUIManager.Instance.Appear();
					GameManagerNew.Instance.CurrentLevel.Init(GameManagerNew.Instance.Level);
                }
                Stage.Instance.checked1 = false;
                ActiveCVGroup();
                this.gameObject.SetActive(false);
				Stage.Instance.AfterPanel();
            });
        }
	}

	public void ActiveCVGroup()
	{
		if (!canvasGroup.blocksRaycasts)
		{
			canvasGroup.blocksRaycasts = true;
		}
	}
    public void SetMinusText(char t, int value)
    {
        minusText.gameObject.SetActive(true);
        minusText.text = t + value.ToString();
        StartCoroutine(DisableText());
    }
    IEnumerator DisableText()
    {
        yield return new WaitForSeconds(0.8f);
        minusText.gameObject.SetActive(false);
    }
}
