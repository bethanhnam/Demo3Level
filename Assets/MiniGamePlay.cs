using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGamePlay : MonoBehaviour
{
    public static MiniGamePlay instance;

    public MiniItem[] miniItem;

    public Slider[] sliders;
    public Sprite[] sprites;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public void Appear()
    {
        this.gameObject.SetActive(true);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetItem(int indexSprite)
    {
        for (int i = 0; i < miniItem.Length; i++)
        {
            miniItem[i].SetImage(sprites[indexSprite]);
            miniItem[i].itemImg.SetNativeSize();
        }
    }
}
