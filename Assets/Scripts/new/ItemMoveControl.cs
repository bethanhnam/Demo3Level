using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ItemMoveControl : MonoBehaviour
{
	[SerializeField]
	private Image img;

	[SerializeField]
	private RectTransform rectTransform;

	[SerializeField]
	private AnimationCurve curveScale;

	public RectTransform RectTransform { get => rectTransform; set => rectTransform = value; }

	public void Init(Vector3 pos, Sprite spr, Vector3 _scale, Vector3 targetAnchor)
	{
		if (!gameObject.activeSelf)
		{
			gameObject.SetActive(true);
		}
		AudioManager.instance.PlaySFX("Winpop");
		img.sprite = spr;
		img.SetNativeSize();

		transform.position = pos;
		transform.localScale = _scale/2f;

		Color c = Color.white;
		c.a = 0.5f;

		img.color = c;
		img.DOFade(1, 0.3f);

		Vector3 targetScale = _scale / 1.1f;
		transform.DOScale(targetScale, 0.8f).SetEase(Ease.InQuad);
		transform.DOPath(new Vector3[] { transform.position, new Vector3(-2, Mathf.Min(transform.position.y, targetAnchor.y) - 1f), targetAnchor }, 0.4f, PathType.CatmullRom).SetEase(Ease.InQuad);

		//transform.DOScale(targetScale+ new Vector3(0.05f,0.05f,0), 0.5f).OnComplete(() =>
		//{
		//	transform.DOScale(targetScale - new Vector3(0.01f, 0.01f, 0), 0.3f).OnComplete(() =>
		//	{
		//		transform.DOScale(targetScale, 0.3f);
		//	});
		//});
	}
	public void MoveToFix(Vector3 _scale, Vector3 targetAnchor,Action action)
	{
		Vector3 targetScale = _scale;
		transform.DOScale(targetScale, 0.8f).SetEase(curveScale);

		transform.DOPath(new Vector3[] { transform.position, targetAnchor }, 0.6f, PathType.CatmullRom).SetEase(Ease.InQuad).OnComplete(() =>
		{
			action();
		});
	}
}
