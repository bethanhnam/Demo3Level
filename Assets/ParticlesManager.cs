using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
	public static ParticlesManager instance;
	public GameObject StarTrailParticleObject;
	public GameObject StarParticleObject;
	public GameObject ItemAppearlParticleObject;
	private void Start()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
}
