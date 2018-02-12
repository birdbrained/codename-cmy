using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField]
	private float timeBeforeDestroy;

	// Use this for initialization
	void Start () 
	{
		StartCoroutine(WaitBeforeDestroying());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private IEnumerator WaitBeforeDestroying()
	{
		yield return new WaitForSeconds(timeBeforeDestroy);
		Destroy(gameObject);
	}
}
