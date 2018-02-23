using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTest : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("XboxOneTrigger") != 0)
		{
			Debug.Log("Trigger, " + Input.GetAxis("XboxOneTrigger").ToString());
		}
	}
}
