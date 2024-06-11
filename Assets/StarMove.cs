using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StarMove : MonoBehaviour
{
    public Transform startPos;
    public List<starMoveObj> stars;
    public starMoveObj starPrefab;
    public Canvas spawnCanvas;

    private Vector3 endPos;
    private Vector3 stepPos;
    private bool isMove;
    private Action a;
    [SerializeField]
    private float startTime;
    [SerializeField]
    private float timeMove;
    [SerializeField]
    private float baseTimeMove;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator Spawn(Vector3 des, Action action, int numOfStar, LevelButton levelButton)
    {
        for (int i = 0; i < numOfStar; i++)
        {
            yield return new WaitForSeconds(0.3f);
            starMoveObj star = Instantiate(starPrefab, transform.position, Quaternion.identity, spawnCanvas.transform);
            star.transform.localScale = new Vector3(.5f, .5f, 1f);
            stars.Add(star);
            MoveToDes(des, action, star, levelButton);
        }
    }
    public void CreateStar(Vector3 des, Action action, int numOfStar, LevelButton levelButton)
    {
        UIManagerNew.Instance.starTexts[0].color = Color.red;
        DOVirtual.Float(SaveSystem.instance.star + numOfStar, SaveSystem.instance.star, 0.5f, (x) =>
        {
            UIManagerNew.Instance.starTexts[0].color = Color.white;
            UIManagerNew.Instance.starTexts[0].text = Mathf.CeilToInt(x).ToString();

        });
        UIManagerNew.Instance.ButtonMennuManager.DiactiveCVGroup();
        this.gameObject.SetActive(true);
        StartCoroutine(Spawn(des, action, numOfStar,levelButton));
        endPos = des;
    }
    public void MoveToDes(Vector3 des, Action action, starMoveObj star,LevelButton levelButton)
    {
        star.transform.DOScale(1, 1f);
        Vector3 rotationAngles = new Vector3(0, 0, 360);
        star.transform.DORotate(rotationAngles, 0.7f, RotateMode.FastBeyond360)
                .SetLoops(-1 , LoopType.Incremental)
                .SetEase(Ease.Linear).OnComplete(() =>
                {
                    star.transform.DORotate(Vector3.zero, 0.05f); ; // Đặt góc quay về 0
                });
        AudioManager.instance.PlaySFX("StarRecieve");
        star.MoveToFix(star, startPos.position, des, (() =>
        {
            
            levelButton.transform.DOScale(1.2f, .1f).OnComplete(() =>
            {
                levelButton.transform.DOScale(1f, 0.05f);
            });
            stars.Remove(star);
            Destroy(star.gameObject);
            if (stars.IsNullOrEmpty())
            {
                action();
            }
        }));
    }
    IEnumerator Close(Action action, starMoveObj star)
    {
        yield return new WaitForSeconds(0.1f);
        
    }
}
