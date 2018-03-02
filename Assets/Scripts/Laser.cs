using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Fire(string tag)
	{

	}

	public override void ChargeFire()
	{

	}

	public override AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol)
	{
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
}
