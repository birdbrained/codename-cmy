using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceBetweenTwoPoints : MonoBehaviour 
{
	[SerializeField]
	private Vector3 pos1;
	[SerializeField]
	private Vector3 pos2;
	[SerializeField]
	private float speed = 2.0f;
    private float t = 0.0f;

    private bool canBounce = true;
    public bool CanBounce
    {
        get
        {
            return canBounce;
        }
        set
        {
            canBounce = value;
        }
    }
	
	// Update is called once per frame
	void Update()
	{
        if (canBounce)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * t) + 1.0f) / 2.0f);
        }
	}
}
