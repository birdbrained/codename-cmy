using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
	[SerializeField]
	private float timeBeforeDestroy;
    private ParticleSystem ps;

	// Use this for initialization
	void Start () 
	{
        ps = GetComponent<ParticleSystem>();
		StartCoroutine(WaitBeforeDestroying());
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	private IEnumerator WaitBeforeDestroying()
	{
		yield return new WaitForSeconds(timeBeforeDestroy);
        if (ps != null)
            ps.Stop();
		Destroy(gameObject);
	}
}
