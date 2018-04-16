using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuPointer : MonoBehaviour
{
    [SerializeField]
    private Button[] options;
    private int currentlySelected = 0;
    private RectTransform rect;
    [SerializeField]
    private EventSystem es;

	// Use this for initialization
	void Start ()
    {
        rect = GetComponent<RectTransform>();
        //es = FindObjectOfType<EventSystem>();
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        /*if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            currentlySelected--;
            if (currentlySelected < 0)
                currentlySelected = options.Length - 1;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            currentlySelected++;
            if (currentlySelected >= options.Length)
                currentlySelected = 0;
        }*/

		if (rect != null && es != null)
        {
            //rect.position = new Vector3(rect.position.x, es.currentSelectedGameObject.gameObject.transform.position.y, rect.position.z);
            GameObject currSelected = es.currentSelectedGameObject;
            //Debug.Log("curr selected is null? " + currSelected == null);
            if (currSelected != null)
            {
                rect.position = new Vector3(rect.position.x, currSelected.transform.position.y, rect.position.z);
            }
        }
	}
}
