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

	public abstract void Fire();
	public abstract void ChargeFire();
}
