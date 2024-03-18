using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class HintImgPanel : MonoBehaviour
{
	public RectTransform RectTransform;
	public Image hintImg;
	public List<Sprite> hintImgs = new List<Sprite>();

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			RectTransform.localPosition = Vector3.zero;
			this.gameObject.SetActive(true);
			AudioManager.instance.PlaySFX("OpenPopUp");
			GameManager.instance.hasUI = true;
			UIManager.instance.DeactiveTime();
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			RectTransform.DOAnchorPos(new Vector2(0,-2900f),.1f,false).SetEase(Ease.OutElastic).OnComplete(() => { 
			this.gameObject.SetActive(false);
				AudioManager.instance.PlaySFX("ClosePopUp");
				GameManager.instance.hasUI = false;
				 UIManager.instance.ActiveTime();
			});
		}
	}
	public void LoadHint()
	{
			hintImg.sprite = LevelManager.instance.levelInstances[0].GetComponent<Level>().stageManager.levelInstances[0].GetComponent<Stage>().hintImg;
	}
}
