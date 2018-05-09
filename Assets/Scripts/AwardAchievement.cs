using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwardAchievement : MonoBehaviour
{
    public int id;

	// Use this for initialization
	void Start ()
    {
        Award(id);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Award(int i)
    {
        AchievementManager.Instance.AwardAchievement(i);
    }
}
