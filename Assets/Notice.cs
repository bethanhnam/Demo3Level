using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notice : MonoBehaviour
{
    public static Notice Instance;
    public GameObject noticeDes;
    public GameObject noticeImg;
    public GameObject defaultPos;
    public bool canAppear = true;

    private void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        canAppear = true;
    }
    private void OnEnable()
    {

    }
    public void NoticeAppear()
    {
        if (canAppear)
        {
            if (noticeImg.gameObject.activeSelf == false)
            {
                canAppear = false;
                this.gameObject.SetActive(true);
                gameObject.transform.DOMove(noticeDes.transform.position, 0.1f).OnComplete(() =>
                {
                    try
                    {
                        if (gameObject.activeSelf == true)
                        {
                            StartCoroutine(DisableNotice());
                        }
                    }
                    catch { }
                });
            }
        }
    }
    public void NoticeAppearViaButton()
    {
        try
        {
            this.gameObject.transform.position = noticeDes.transform.position;
            this.gameObject.SetActive(true);
            try
            {
                if (gameObject.activeSelf == true)
                {
                    StartCoroutine(DisableNotice());
                }
            }
            catch { }
            //        gameObject.transform.DOMove(noticeDes.transform.position, 0.3f).OnComplete(() =>
            //{

            //});
        }
        catch { };
    }
    IEnumerator DisableNotice()
    {
        yield return new WaitForSeconds(1f);
        try
        {
            noticeImg.SetActive(true);
            this.gameObject.SetActive(false);
            this.gameObject.transform.localPosition = defaultPos.transform.position;

            Stage.Instance.checked1 = false;
            canAppear = true;
        }
        catch { };
    }
    public void NoticeDisappear()
    {
        this.gameObject.SetActive(false);
        noticeImg.SetActive(false);
        canAppear = true;
        Stage.Instance.numOfHoleNotAvailable.Clear();
    }
}
