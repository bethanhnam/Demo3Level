using DG.Tweening;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    public Transform startPos;
    public List<GameObject> stars;
    public GameObject starPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    IEnumerator Spawn(Vector3 des,Action action,int numOfStar)
    {
        for (int i = 0; i < numOfStar; i++)
        {
            yield return new WaitForSeconds(0.3f);
            var star = Instantiate(starPrefab, transform.position, Quaternion.identity, this.transform);
            star.transform.localScale = new Vector3(.5f, .5f, 1f);
            stars.Add(star);
            MoveToDes(des,action, star);
        }
    }
    public void CreateStar(Vector3 des, Action action,int numOfStar)
    {

        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
        this.gameObject.SetActive(true);
        StartCoroutine(Spawn(des,action,numOfStar));
    }
    public void MoveToDes(Vector3 des,Action action, GameObject star)
    {
        star.transform.DOScale(1, 1f);
        star.transform.DOMove(des, 1f).OnComplete(() =>
        {
            StartCoroutine(Close(action,star));
        });
    }
    IEnumerator Close(Action action,GameObject star)
    {
        yield return new WaitForSeconds(0.1f);
        stars.Remove(star);
        Destroy(star);
        if (stars.IsNullOrEmpty())
        {
            action();
        }
    }
}
