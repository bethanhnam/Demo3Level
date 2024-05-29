using DG.Tweening;
using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardMove : MonoBehaviour
{
    public GameObject coinImgDes;
    public GameObject StarImgDes;
    public List<GameObject> coin = new List<GameObject>();
    public List<GameObject> star = new List<GameObject>();
    public GameObject prefabToSpawn;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnEnable()
    {
        SpawnObjects();
    }
    private void SpawnObjects()
    {
        // Kích thước của object cha (giả sử là 400x400)
        float parentWidth = this.GetComponent<RectTransform>().rect.width;
        float parentHeight = this.GetComponent<RectTransform>().rect.height;

        for (int i = 0; i < coin.Count; i++)
        {
            coin[i].transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = Random.Range(-parentHeight / 2, parentHeight / 2);

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            var coinIndex = coin[i];
            coin[i].SetActive(true);
            coin[i].transform.DOScale(Vector3.one, 0.3f).OnComplete(() =>
            {
                float randomSpeed = Random.Range(0.5f, 2f);
                coinIndex.GetComponent<Animator>().speed = randomSpeed;
            });

            // Instantiate object con và gán nó vào object cha
            coin[i].transform.localPosition = randomPosition;
        }
        for (int i = 0; i < star.Count; i++)
        {
            star[i].transform.localScale = Vector3.zero;

            // Tạo tọa độ ngẫu nhiên trong phạm vi kích thước của object cha
            float randomX = Random.Range(-parentWidth / 2, parentWidth / 2);
            float randomY = Random.Range(-parentHeight / 2, parentHeight / 2);

            // Tạo một vị trí mới cho object con
            Vector3 randomPosition = new Vector3(randomX, randomY, 0f);
            star[i].SetActive(true);
            star[i].transform.DOScale(Vector3.one, 0.3f);

            // Instantiate object con và gán nó vào object cha
            star[i].transform.localPosition = randomPosition;
        }
        StartCoroutine(MoveToDes());
    }
    IEnumerator MoveToDes()
    {
        yield return new WaitForSeconds(1.1f);
        for (int i = 0; i < coin.Count; i++)
        {
            //Destroy(coin[i],i * .1f);
            coin[i].transform.DOMove(coinImgDes.transform.position, i * .1f).OnComplete(() =>
            {
                coinImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                {
                    coinImgDes.gameObject.transform.DOScale(1f, 0f);
                });
                StartCoroutine(Close(i));
            });
        }
        for (int i = 0; i < star.Count; i++)
        {
            star[i].GetComponent<Animator>().enabled = true;
            //Destroy(star[i], i * .1f);
            star[i].transform.DOMove(StarImgDes.transform.position, 0.3f).OnComplete(() =>
            {
                StarImgDes.gameObject.transform.DOScale(1.2f, 0.15f).OnComplete(() =>
                {
                    StarImgDes.gameObject.transform.DOScale(1f, 0f);
                });
            });
        }
        //this.gameObject.SetActive(false);
    }
    IEnumerator Close(int time)
    {
        yield return new WaitForSeconds(time * 0.08f);
        for (int i = 0; i < coin.Count; i++)
        {
            coin[i].SetActive(false);
        }

        for (int i = 0; i < star.Count; i++)
        {
            star[i].SetActive(false);

        }
        yield return new WaitForSeconds(3f);
        this.gameObject.SetActive(false);
    }
}
