using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopup : MonoBehaviour
{
    private Achievement myAchievement;
    public Text statusText;
    public Text nameText;
    public Text descText;
    public Image achImage;
    public bool referenceAchievementList = false;
    public int index;

	public void Initialize(Achievement a)
    {
        myAchievement = a;
    }

    void Start()
    {
        if (referenceAchievementList)
            Initialize(AchievementManager.Instance.achievementList[index]);
    }

    void Update()
    {
        if (myAchievement != null)
        {
            if (nameText != null)
                nameText.text = myAchievement.name;
            if (statusText != null)
            {
                if (myAchievement.unlocked)
                    statusText.text = "CHEEVO CHEEVED!";
                else
                    statusText.text = "LOCKED";
            }
            if (descText != null)
                descText.text = myAchievement.description;
            if (achImage != null)
                achImage.sprite = myAchievement.sprite;
        }
    }
}
