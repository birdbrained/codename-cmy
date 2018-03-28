using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbParticle : MonoBehaviour 
{
	[SerializeField]
	private float speed;
	[SerializeField]
	private float timeToLive;
	private float t = 0.0f;
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.Translate(Vector3.up * Time.deltaTime, Space.World);

		t += Time.deltaTime;
		if (t >= timeToLive)
		{
			Destroy(gameObject);
		}
	}
}
