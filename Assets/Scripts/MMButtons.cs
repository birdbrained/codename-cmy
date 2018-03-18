using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMButtons : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	public void EnableObj(GameObject obj)
	{
		obj.SetActive(true);
	}

	public void DisableObj(GameObject obj)
	{
		obj.SetActive(false);
	}
}
