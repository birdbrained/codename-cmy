using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAtObject : MonoBehaviour 
{
	[SerializeField]
	private GameObject objectToPointAt;
	[SerializeField]
	private float offset;

	private bool facingRight = false;
	public bool FacingRight
	{
		get
		{
			return facingRight;
		}
		set
		{
			facingRight = value;
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if (objectToPointAt != null)
		{
			Vector3 targetPos = Camera.main.WorldToScreenPoint(objectToPointAt.transform.position);
			targetPos.z = 5.23f;
			Vector3 objectPos = Camera.main.WorldToScreenPoint(transform.position);
			targetPos.x = targetPos.x - objectPos.x;
			targetPos.y = targetPos.y - objectPos.y;
			float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

			if (facingRight)
			{
				transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));
			}
			else
			{
				transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset + 180.0f));
			}
		}
	}
}
