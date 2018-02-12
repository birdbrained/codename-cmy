using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpTest : MonoBehaviour 
{
	[SerializeField]
	private Color[] colors;
	[SerializeField]
	private SpriteRenderer[] renderers;
	[SerializeField]
	private float duration;
	private float t = 0.0f;

	// Use this for initialization
	void Start () 
	{
		//sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		for (int i = 0; i < renderers.Length; i++)
		{
			renderers[i].material.color = Color.Lerp(colors[0], colors[1], t);
		}
		if (t < 1.0f)
		{
			t += Time.deltaTime / duration;
		}
	}
}
