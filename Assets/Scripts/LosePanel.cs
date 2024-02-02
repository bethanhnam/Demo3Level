using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LosePanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WatchAd()
    {
		// load ad 
		Close();
		UIManager.instance.gamePlayPanel.timer.SetTimer(61f);
	}
	public void Replay()
    {
		// load ad 
		Close();
		GameManager.instance.Replay();
	}
	public void Open()
	{
		if (!this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(true);
		}
	}
	public void Close()
	{
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.gamePlayPanel.timer.TimerOn = true;
		}
	}
}
