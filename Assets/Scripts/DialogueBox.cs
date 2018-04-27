using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    [SerializeField]
    private string[] dialogue;
    [SerializeField]
    private Sprite[] tutSprites;
    [SerializeField]
    private float timeBuffer;
    [SerializeField]
    private Text dialogueText;
    [SerializeField]
    private Image tutImage;
    [SerializeField]
    private GameObject advanceButtonImage;

    private bool canAdvanceText;
    private int currentIndex;

	// Use this for initialization
	void Awake ()
    {
        canAdvanceText = false;
        currentIndex = 0;

        StartCoroutine(WaitBeforeAllowingTextToAdvance());
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (dialogueText != null)
        {
            dialogueText.text = dialogue[currentIndex];
        }
        if (tutImage != null)
        {
            tutImage.sprite = tutSprites[currentIndex];
        }

		if (canAdvanceText)
        {
            if (advanceButtonImage != null)
            {
                advanceButtonImage.SetActive(true);
            }

            if (Input.GetAxis("Submit") > 0)
            {
                canAdvanceText = false;
                currentIndex++;
                if (currentIndex >= dialogue.Length)
                {
                    currentIndex = 0;
                    dialogueText.gameObject.SetActive(false);
                    tutImage.gameObject.SetActive(false);
                    advanceButtonImage.gameObject.SetActive(false);
                    gameObject.SetActive(false);
                }
                StartCoroutine(WaitBeforeAllowingTextToAdvance());
            }
        }
        else
        {
            if (advanceButtonImage != null)
            {
                advanceButtonImage.SetActive(false);
            }
        }
	}

    public IEnumerator WaitBeforeAllowingTextToAdvance()
    {
        yield return new WaitForSeconds(timeBuffer);
        canAdvanceText = true;
    }
}
