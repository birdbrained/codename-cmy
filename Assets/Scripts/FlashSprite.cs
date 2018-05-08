using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashSprite : MonoBehaviour
{
    private SpriteRenderer sr;
    public float flashTime;
    private float t = 0.0f;
    public bool canFlash = false;
    private bool isFlashing = false;

	// Use this for initialization
	void Start()
    {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (canFlash)
        {
            isFlashing = true;
            t += Time.deltaTime;
            if (t >= flashTime)
            {
                canFlash = false;
            }
        }
        else
        {
            isFlashing = false;
            sr.enabled = false;
            t = 0.0f;
        }
	}

    public void StartFlash()
    {
        canFlash = true;
        isFlashing = true;
        StartCoroutine(FlashTimer());
    }

    IEnumerator FlashTimer()
    {
        while (isFlashing)
        {
            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
        }
    }
}
