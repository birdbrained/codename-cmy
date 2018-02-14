using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	[SerializeField]
	private float speed;
	[SerializeField]
	private SpriteRenderer fireSprite;
	public SpriteRenderer FireSprite
	{
		get
		{
			return fireSprite;
		}
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.down * speed);
	}
}
