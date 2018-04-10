using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbParticle : MonoBehaviour 
{
	private SpriteRenderer spr;

	[SerializeField]
	private float speed;
	[SerializeField]
	private float timeToLive;
	private float t = 0.0f;
	[SerializeField]
	private bool fadeOut = false;
	private float alpha = 0.0f;
	private Color baseColor;
	private Color transColor;

	void Start()
	{
		spr = GetComponent<SpriteRenderer>();
		if (spr != null)
		{
			baseColor = spr.color;
			transColor = new Color(baseColor.r, baseColor.g, baseColor.b, 0);
		}
	}

	// Update is called once per frame
	void FixedUpdate () 
	{
		transform.Translate(Vector3.up * Time.deltaTime, Space.World);

		t += Time.deltaTime;
		if (t >= timeToLive)
		{
			Destroy(gameObject);
		}

		//Debug.Log((1 -(float)t / (float)timeToLive) * 255.0f);

		if (fadeOut)
		{
			//spr.material.color = new Color(255, 255, 255, (1 - (float)t / (float)timeToLive) * 255.0f);
			spr.material.color = Color.Lerp(baseColor, transColor, alpha);
			if (alpha < 1.0f)
			{
				alpha += Time.deltaTime / timeToLive;
			}
		}
	}
}
