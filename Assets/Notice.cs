using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
	public static Notice Instance;
	public GameObject noticeDes;
	public Vector2 defaultPos = new Vector2(1531f, 0);
	public bool canAppear = true;

	private void Start()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}
	private void OnEnable()
	{
		canAppear = true;
		NoticeAppear();
	}
	public void NoticeAppear()
	{
		if (canAppear)
		{
			canAppear = false;
			this.gameObject.SetActive(true);
			gameObject.transform.DOMove(noticeDes.transform.position, 0.3f).OnComplete(() =>
			{
			
			});
		}
	}

	public void DisableNotice()
	{
		this.gameObject.SetActive(false);
		this.gameObject.transform.localPosition = defaultPos;
		canAppear = true;
		InputManager.instance.hasShowNotice = false;
	}
}
