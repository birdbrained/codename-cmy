using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		if (currDelay > 0)
			currDelay -= Time.deltaTime;
	}

	public override void Fire(string tag)
	{
		if (currDelay <= 0)
		{
			for (int i = 0; i < 8; i++)
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
				_bullet.transform.localScale *= Random.Range(0.5f, 2f);
				_bullet.GetComponent<Bullet>().ChangeBulletSpeedByPercent(Random.Range(0.8f, 1.3f));
				Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
				cursorPos.z = 5.23f;
				Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
				cursorPos.x = cursorPos.x - objectPos.x;
				cursorPos.y = cursorPos.y - objectPos.y;
				float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg + Random.Range(-10f, 11f);
				_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
				_bullet.GetComponent<Bullet>().FireSprite.material.color = bulletColor;
			}
			currDelay = fireDelay;
		}
	}

	public override void ChargeFire()
	{
		for (int i = 0; i < 16; i++)
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
			//_bullet.GetComponent<Bullet>().ChangeBulletSpeedByPercent(Random.Range(0.8f, 1.3f));
			Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
			cursorPos.z = 5.23f;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
			cursorPos.x = cursorPos.x - objectPos.x;
			cursorPos.y = cursorPos.y - objectPos.y;
			float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg + Random.Range(-10f, 11f);
			_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 22.5f * i));
			_bullet.GetComponent<Bullet>().FireSprite.material.color = bulletColor;
		}
	}
}
