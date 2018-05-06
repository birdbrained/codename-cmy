using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MMButtons : MonoBehaviour 
{
    private EventSystem es;

	// Use this for initialization
	void Start ()
    {
        es = EventSystem.current;
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

    public void ChangeSelectedOption(GameObject obj)
    {
        es.SetSelectedGameObject(obj);
    }
}
