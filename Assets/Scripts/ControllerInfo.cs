using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerInfo : MonoBehaviour 
{
	//true if controller, false if keyboard
	public bool isController;
	//if controller, should be left stick, keyboard is WASD or arrow keys
	public string horAxis;
	public string verAxis;
	//should only be populated if using a controller
	public string horAxis2 = "";
	public string verAxis2 = "";
	public string switchColor = "";


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
