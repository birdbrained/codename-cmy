using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon 
{
	private bool isFiring = false;
	private LineRenderer lr;

	// Use this for initialization
	void Start()
	{
		lr = GetComponent<LineRenderer>();
		lr.enabled = false;
		//Debug.Log(lr != null);
		lr.useWorldSpace = true;

		if (fireClip != null)
		{
			fireAudio = AddAudio(fireClip, false, false, fireAudioVolume);
		}
		if (chargeClip != null)
		{
			chargeAudio = AddAudio(chargeClip, true, false, chargeAudioVolume);
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lr != null && cursorObj != null)
		{
			if (isFiring)
			{
				Ray2D ray = new Ray2D(transform.position, transform.forward);

				Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
				cursorPos.z = 5.23f;
				//Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
				//cursorPos.x = cursorPos.x - objectPos.x;
				//cursorPos.y = cursorPos.y - objectPos.y;
				//float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
				RaycastHit2D hit = Physics2D.Raycast(transform.position, cursorPos, 1.0f);
				if (controllerConnected)
					lr.SetPosition(0, transform.position);
				else
					lr.SetPosition(0, bulletSpawnPosition.transform.position);
				lr.SetPosition(1, cursorObj.transform.position);
				lr.enabled = true;
				lr.material.SetColor("_Color", bulletColor);

				currChargeTime -= 0.01f;
				if (currChargeTime <= 0.0f)
				{
					currChargeTime = 0.0f;
					isFiring = false;
					fireAudio.Play();
				}
			}
			else
			{
				chargeAudio.Stop();
				lr.enabled = false;
			}
		}
	}



	public override void Fire(string tag)
	{

	}

	public override void ChargeFire()
	{
		//StartCoroutine(LaserFire());
		currChargeTime = chargeTime;
		chargeAudio.Play();
		isFiring = true;
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
