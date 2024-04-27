using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
	public static ParticlesManager instance;
	public GameObject StarTrailParticleObject;
	public GameObject StarParticleObject;
	public GameObject ItemAppearlParticleObject;
	public GameObject pickUpStartParticle;
	private void Start()
	{
		DontDestroyOnLoad(this.transform);
		if(instance == null)
		{
			instance = this;
		}
	}
}
