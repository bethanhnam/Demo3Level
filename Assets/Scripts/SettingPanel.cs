using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    public GameObject soundOn;
    public GameObject soundOff;
	public GameObject musicOn;
	public GameObject musicOff;
	public GameObject alertOn;
	public GameObject alertOff;
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
            this.gameObject.SetActive(true);
        }
    }
    public void Close()
    {
		if (this.gameObject.activeSelf)
		{
			this.gameObject.SetActive(false);
			UIManager.instance.menuPanel.Open();
		}
	}
    public void SoundOn()
    {
		soundOff.SetActive(false);
        soundOn.SetActive(true);
    }
	public void SoundOff()
	{
		soundOn.SetActive(false);
		soundOff.SetActive(true);
	}
	public void MusicOn()
	{
		musicOff.SetActive(false);
		musicOn.SetActive(true);
	}
	public void MusicOff()
	{
		musicOn.SetActive(false);
		musicOff.SetActive(true);
	}
	public void AlertOn()
	{
		alertOff.SetActive(false);
		alertOn.SetActive(true);
	}
	public void AlertOff()
	{
		alertOn.SetActive(false);
		alertOff.SetActive(true);
	}
}
