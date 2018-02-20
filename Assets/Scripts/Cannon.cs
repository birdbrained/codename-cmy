using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : Weapon 
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate() 
	{
		if (currDelay > 0)
		{
			currDelay -= Time.deltaTime;
		}
	}

	public override void Fire()
	{
		if (currDelay <= 0)
		{
			GameObject _bullet = Instantiate(bulletObj, transform.position, transform.rotation);
			Vector3 cursorPos = Camera.main.WorldToScreenPoint(cursorObj.transform.position);
			cursorPos.z = 5.23f;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(_bullet.transform.position);
			cursorPos.x = cursorPos.x - objectPos.x;
			cursorPos.y = cursorPos.y - objectPos.y;
			float angle = Mathf.Atan2(cursorPos.y, cursorPos.x) * Mathf.Rad2Deg;
			_bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 90.0f));
			_bullet.GetComponent<Bullet>().FireSprite.material.color = bulletColor;

			currDelay = fireDelay;
		}
	}

	public override void ChargeFire()
	{
		GameObject _bullet = Instantiate(bulletObj, transform.position, transform.rotation);
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
	}
}
