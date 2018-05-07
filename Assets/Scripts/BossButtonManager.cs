using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BossButtonManager : MonoBehaviour
{
    private EventSystem es;
    private BossSelectButton currButton;
    [SerializeField]
    private Text nameText;
    [SerializeField]
    private Text descText;
    [SerializeField]
    private Image image;

	// Use this for initialization
	void Start ()
    {
        es = EventSystem.current;
	}
	
	// Update is called once per frame
	void Update ()
    {
        currButton = es.currentSelectedGameObject.GetComponent<BossSelectButton>();
        if (currButton != null)
        {
            if (nameText != null)
            {
                nameText.text = currButton.name;
            }
            if (descText != null)
            {
                descText.text = currButton.desc;
            }
            if (image != null)
            {
                image.sprite = currButton.sprite;
            }
        }
	}
}
