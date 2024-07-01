using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryPicBt : MonoBehaviour
{
    // Start is called before the first frame update
    public int level;
    public GameObject pointer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateLevelViaBT()
    {
        GameManagerNew.Instance.isStory = true;
        GameManagerNew.Instance.CreateLevelForStory(level);
        this.gameObject.SetActive(false);
        pointer.SetActive(false);
    }
}
