using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField]
    private float speed;
    public bool canRotate = true;
    private Quaternion startRotation;

	// Use this for initialization
	void Start ()
    {
        startRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (canRotate)
            transform.Rotate(Vector3.up * Time.deltaTime * speed, Space.World);
        else
            transform.rotation = startRotation;
	}
}
