using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour 
{
	[SerializeField]
	private Color[] colors;
	private SpriteRenderer sr;
	[SerializeField]
	private float duration;
	private float t = 0.0f;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		sr.material.color = Color.Lerp(colors[0], colors[1], t);
		if (t < 1.0f)
		{
			t += Time.deltaTime / duration;
		}
	}
}
