using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;   
    public Sound[] musicSounds, sfxSounds;
    public AudioSource musicSource,sfxSource;

	
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}
	private void Start()
	{
		
	}
	public void PlayMusic( string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);
        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
	public void PlaySFX(string name)
	{
		Sound s = Array.Find(sfxSounds, x => x.name == name);
		if (s == null)
		{
			Debug.Log("Sound Not Found");
		}
		else
		{
			sfxSource.clip = s.clip;
            sfxSource.PlayOneShot(s.clip);
		}
	}

}
