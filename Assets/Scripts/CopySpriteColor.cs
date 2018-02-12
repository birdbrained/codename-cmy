using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopySpriteColor : MonoBehaviour 
{
	private SpriteRenderer sr;
	[SerializeField]
	private GameObject otherObj;
	private SpriteRenderer otherSr;

	// Use this for initialization
	void Start () 
	{
		sr = GetComponent<SpriteRenderer>();

		if (otherObj != null)
		{
			otherSr = otherObj.gameObject.GetComponent<SpriteRenderer>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (otherSr != null)
		{
			sr.material.color = otherSr.material.color;
		}
	}
}
