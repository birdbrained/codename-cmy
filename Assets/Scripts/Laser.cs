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
		//lr.enabled = false;
		//Debug.Log(lr != null);
		lr.useWorldSpace = true;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (lr != null && laserHit != null)
		{
			/*Debug.Log("ok!");
			RaycastHit2D hit = Physics2D.Raycast(transform.position, laserHit.position);
			Debug.DrawLine(transform.position, hit.point);
			laserHit.position = hit.point;
			lr.SetPosition(0, transform.position);
			lr.SetPosition(1, laserHit.position);*/

			Ray2D ray = new Ray2D(transform.position, transform.forward);
			RaycastHit2D hit = Physics2D.Raycast(transform.position, laserHit.position);
			lr.SetPosition(0, transform.position);
			//if (Physics2D.Raycast(ray, out hit, 100))
			//{
			lr.SetPosition(1, laserHit.position);
			//}

			/*if (Input.GetKey(KeyCode.Space))
				lr.enabled = true;
			else
				lr.enabled = false;*/
		}
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
