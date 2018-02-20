using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
	[SerializeField]
	protected GameObject bulletObj;
	[SerializeField]
	protected GameObject cursorObj;
	public GameObject CursorObj
	{
		get
		{
			return cursorObj;
		}
		set
		{
			cursorObj = value;
		}
	}
	[SerializeField]
	protected float fireDelay;
	protected float currDelay;

	[SerializeField]
	protected float chargeTime;
	public float ChargeTime
	{
		get
		{
			return chargeTime;
		}
		set
		{
			chargeTime = value;
		}
	}
	protected float currChargeTime;
	public float CurrChargeTime
	{
		get
		{
			return currChargeTime;
		}
		set
		{
			currChargeTime = value;
		}
	}

	protected Color bulletColor;
	public Color BulletColor
	{
		get
		{
			return bulletColor;
		}
		set
		{
			bulletColor = value;
		}
	}

	[SerializeField]
	protected float speed;
	public float Speed
	{
		get
		{
			return speed;
		}
		set
		{
			speed = value;
		}
	}

	public abstract void Fire();
	public abstract void ChargeFire();
}
