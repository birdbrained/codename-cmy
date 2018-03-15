using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : MonoBehaviour 
{
	[Range (0,2)]
	public int colorIndex = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "player_bullet")
		{
			float damgeModifier = GameManager.Instance.DamageModifier(other.gameObject.GetComponent<Bullet>().colorIndex, colorIndex);
			Debug.Log("modifier: " + damgeModifier.ToString("0.0"));
			Destroy(other.gameObject);
		}
	}
}
