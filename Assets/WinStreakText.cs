using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinStreakText : MonoBehaviour
{
    public Text winStreakText;

	// Use this for initialization
	void Start () {
        if (winStreakText != null)
            winStreakText.text = "Win Streak: " + GameManager.Instance.GetWinStreak().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
