using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Weapon 
{
	// Use this for initialization
	void Start () 
	{
		if (fireClip != null)
		{
			fireAudio = AddAudio(fireClip, false, false, fireAudioVolume);
		}
		if (chargeClip != null)
		{
			chargeAudio = AddAudio(chargeClip, false, false, chargeAudioVolume);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		if (currDelay > 0)
		{
			currDelay -= Time.deltaTime;
		}
	}

	public override void Fire(string tag)
	{
		if (currDelay <= 0)
		{
			GameObject _bullet;
			if (controllerConnected)
			{
				_bullet = Instantiate(bulletObj, transform.position, transform.rotation);
			}
			else
			{
				_bullet = Instantiate(bulletObj, bulletSpawnPosition.transform.position, transform.rotation);
			}
			_bullet.tag = tag;
			Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
			cursorPos.z = 5.23f;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
			cursorPos.x = cursorPos.x - objectPos.x;
			cursorPos.y = cursorPos.y - objectPos.y;
			float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
			_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
			_bullet.GetComponent<Bullet>().FireSprite.material.color = bulletColor;

			currDelay = fireDelay;
			if (fireAudio != null)
				fireAudio.Play();
		}
	}

	public override void ChargeFire()
	{
		GameObject _bullet;
		if (controllerConnected)
		{
			_bullet = Instantiate(bulletObj, transform.position, transform.rotation);
		}
		else
		{
			_bullet = Instantiate(bulletObj, bulletSpawnPosition.transform.position, transform.rotation);
		}
		_bullet.tag = tag;
		_bullet.transform.localScale *= 3.0f;
		_bullet.GetComponent<Bullet>().ChangeBulletSpeedByPercent(1.5f);
		Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
		cursorPos.z = 5.23f;
		Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
		cursorPos.x = cursorPos.x - objectPos.x;
		cursorPos.y = cursorPos.y - objectPos.y;
		float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
		_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
		_bullet.GetComponent<Bullet>().FireSprite.material.color = bulletColor;

		//currDelay = fireDelay;
		currChargeTime = 0.0f;
		if (chargeAudio != null)
			chargeAudio.Play();
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
