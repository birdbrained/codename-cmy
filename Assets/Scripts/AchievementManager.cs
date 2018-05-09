using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private static AchievementManager instance;
    public static AchievementManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AchievementManager>();
            }
            return instance;
        }
    }

    public GameObject windowPrefab;
    public Sprite[] spriteList;

    private string filename = "/acheivementData.dat";
    public Achievement ach_defeatBossCoop;
    public Achievement ach_friendlyFireKill;
    public Achievement ach_uphillBattle;
    public Achievement ach_tenStreak;
    public Achievement ach_bird;
    public Achievement ach_trueYokel;
    public Achievement[] achievementList;

    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);

        ach_defeatBossCoop = new Achievement
        (
            0,
            "Good Teamwork!",
            "Defeat any boss with a buddy (Local or Online)",
            false,
            spriteList[0]
        );

        ach_friendlyFireKill = new Achievement
        (
            1,
            "I swear that was on purpose",
            "Use an explosion caused from shooting another player to kill any boss",
            false,
            spriteList[1]
        );

        ach_uphillBattle = new Achievement
        (
            2,
            "Uphill Battle",
            "Defeat any boss using only colors that are not very effective against the boss",
            false,
            spriteList[2]
        );

        ach_tenStreak = new Achievement
        (
            3,
            "You're just on a roll aren't ya",
            "Get a streak of 10 boss kills",
            false,
            spriteList[3]
        );

        ach_bird = new Achievement
        (
            4,
            "Bird",
            "Find it",
            false,
            spriteList[4]
        );

        ach_trueYokel = new Achievement
        (
            5,
            "True Yokel",
            "Get a streak of 30 boss kills",
            false,
            spriteList[5]
        );

        achievementList = new Achievement[6];
        achievementList[0] = ach_defeatBossCoop;
        achievementList[1] = ach_friendlyFireKill;
        achievementList[2] = ach_uphillBattle;
        achievementList[3] = ach_tenStreak;
        achievementList[4] = ach_bird;
        achievementList[5] = ach_trueYokel;

        Load();
    }

    public void ResetAchievements()
    {
        for (int i = 0; i < achievementList.Length; i++)
        {
            achievementList[i].unlocked = false;
        }
        Save();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenWrite(Application.persistentDataPath + filename);

        AchievementUnlockStatus list = new AchievementUnlockStatus();
        list.unlocked0 = achievementList[0].unlocked;
        list.unlocked1 = achievementList[1].unlocked;
        list.unlocked2 = achievementList[2].unlocked;
        list.unlocked3 = achievementList[3].unlocked;
        list.unlocked4 = achievementList[4].unlocked;
        list.unlocked5 = achievementList[5].unlocked;

        bf.Serialize(file, list);
        file.Close();
    }

    public void Load()
    {
        //Debug.Log("attempting to load file");
        if (File.Exists(Application.persistentDataPath + filename))
        {
            //Debug.Log("attempting some more");
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.OpenRead(Application.persistentDataPath + filename);
            //Debug.Log("opened a file for reading");

            AchievementUnlockStatus list = (AchievementUnlockStatus)bf.Deserialize(file);
            file.Close();

            //Debug.Log(list.unlocked0);

            achievementList[0].unlocked = list.unlocked0;
            achievementList[1].unlocked = list.unlocked1;
            achievementList[2].unlocked = list.unlocked2;
            achievementList[3].unlocked = list.unlocked3;
            achievementList[4].unlocked = list.unlocked4;
            achievementList[5].unlocked = list.unlocked5;
        }
    }

    public void AwardAchievement(int id)
    {
        if (id < 0 || id > 5)
            return;

        if (!achievementList[id].unlocked)
        {
            achievementList[id].unlocked = true;
            AchievementPopup popup = Instantiate(windowPrefab, transform.position, transform.rotation).GetComponent<AchievementPopup>();
            popup.Initialize(achievementList[id]);
            Save();
        }
    }
}

[Serializable]
public class Achievement
{
    public int id;
    public string name;
    public string description;
    public bool unlocked;
    public Sprite sprite;

    public Achievement(int i, string n, string d, bool u, Sprite s)
    {
        id = i;
        name = n;
        description = d;
        unlocked = u;
        sprite = s;
    }
}

[Serializable]
public class AchievementUnlockStatus
{
    public bool unlocked0;
    public bool unlocked1;
    public bool unlocked2;
    public bool unlocked3;
    public bool unlocked4;
    public bool unlocked5;
}
