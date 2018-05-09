using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Photon.PunBehaviour 
{
	//get the GameManager instance
	private static GameManager instance;
	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<GameManager>();
			}
			return instance;
		}
	}
	private static GameSettings gsInstance;
	public static GameSettings GSInstance
	{
		get
		{
			if (gsInstance == null)
			{
				gsInstance = new GameSettings();
			}
			return gsInstance;
		}
	}

    public static int winStreak = 0;

    [SerializeField]
    private bool enableDebugCommands = false;
    [SerializeField]
    private bool forceStandardColors = false;

    public ReviveMinigame rmInstance;

    //the pool of colors to pick from
    [SerializeField]
	private Color[] playerColors;
	public Color[] PlayerColors
	{
		get
		{
			return playerColors;
		}
	}

	//info about player's weapons: 0 = cannon, 1 = shotgun, 2 = laser
	public int playerOnePrimaryWeaponIndex;
	public int playerOneSecondaryWeaponIndex;
	public int playerTwoPrimaryWeaponIndex;
	public int playerTwoSecondaryWeaponIndex;

	//info about player's colors: 0 = cyan, 1 = magenta, 2 = yellow
	public int playerOnePrimaryColorIndex;
	public int playerOneSecondaryColorIndex;
	public int playerTwoPrimaryColorIndex;
	public int playerTwoSecondaryColorIndex;

    [SerializeField]
    private bool onlineGame = false;
	[SerializeField]
	private PlayerController player1;
	public PlayerController Player1
	{
		get
		{
			return player1;
		}
		set
		{
			player1 = value;
		}
	}
	[SerializeField]
	private PlayerController player2;
	public PlayerController Player2
	{
		get
		{
			return player2;
		}
		set
		{
			player2 = value;
		}
	}
    public PlayerOnlineController OnlinePlayer1;
    public PlayerOnlineController OnlinePlayer2;
	[SerializeField]
	private Boss boss; //might have to change this to be obj-orn later
	public Boss Boss
	{
		get
		{
			return boss;
		}
		set
		{
			boss = value;
		}
	}

    [SerializeField]
    private GameObject explosionObj;
    public GameObject ExplosionObj
    {
        get
        {
            return explosionObj;
        }
    }

	//if true, make the color indexes correspond to the weapon indexes
	//if false, randomize the color associated with each weapon
	private bool colorIndexMatchesWeaponIndex = true;
	public bool ColorIndexMatchesWeaponIndex
	{
		get
		{
			return colorIndexMatchesWeaponIndex;
		}
		set
		{
			colorIndexMatchesWeaponIndex = value;
		}
	}

	private static ControllerInfo player1ControllerInfo;
	public ControllerInfo Player1ControllerInfo
	{
		get
		{
			return player1ControllerInfo;
		}
		set
		{
			player1ControllerInfo = value;
		}
	}
	private static ControllerInfo player2ControllerInfo;
	public ControllerInfo Player2ControllerInfo
	{
		get
		{
			return player2ControllerInfo;
		}
		set
		{
			player2ControllerInfo = value;
		}
	}

	//determine which color beats which color: 
	//	positive num or 0 for cyan -> magenta -> yellow -> cyan (0 -> 1 -> 2)
	//	negative num for      cyan <- magenta <- yellow <- cyan (0 <- 1 <- 2)
	private int colorOrder;
	private static bool randomizePlayerControls = false;
	public bool RandomizePlayerControls
	{
		get
		{
			return randomizePlayerControls;
		}
		set
		{
			randomizePlayerControls = value;
		}
	}

	private static int hardMode;
	public int HardMode
	{
		get
		{
			return hardMode;
		}
		set
		{
			hardMode = value;
		}
	}

	[SerializeField][Range (0,100)]
	private int invertMovementPercentage;
	[SerializeField][Range (0,100)]
	private int invertAimingPercentage;
	[SerializeField][Range (0,100)]
	private int swapFireAndDefendPercentage;
	[SerializeField][Range (0,100)]
	private int samePlayerControlsMovementPercentage;
	[SerializeField][Range (0,100)]
	private int samePlayerControlsFiringPercentage;
	[SerializeField][Range (0,100)]
	private int switchMoveAndFirePercentage;

	/*public GameObject CritParticle;
	public GameObject ResistParticle;
	public GameObject WhiffParticle;*/

	// Use this for initialization
	void Start () 
	{
		DetermineWeapons(colorIndexMatchesWeaponIndex);
        if (onlineGame)
        {
            if (OnlinePlayer1 != null && OnlinePlayer2 != null)
            {
                OnlinePlayer1.SetCurrentColors(playerOnePrimaryColorIndex, playerOneSecondaryColorIndex);
                OnlinePlayer2.SetCurrentColors(playerTwoPrimaryColorIndex, playerTwoSecondaryColorIndex);
                OnlinePlayer1.SetCurrentWeapons(playerOnePrimaryWeaponIndex, playerOneSecondaryWeaponIndex);
                OnlinePlayer2.SetCurrentWeapons(playerTwoPrimaryWeaponIndex, playerTwoSecondaryWeaponIndex);
            }
        }
        else
        {
            if (Player1 != null && Player2 != null)
            {
                Player1.SetCurrentColors(playerOnePrimaryColorIndex, playerOneSecondaryColorIndex);
                Player2.SetCurrentColors(playerTwoPrimaryColorIndex, playerTwoSecondaryColorIndex);
                Player1.SetCurrentWeapons(playerOnePrimaryWeaponIndex, playerOneSecondaryWeaponIndex);
                Player2.SetCurrentWeapons(playerTwoPrimaryWeaponIndex, playerTwoSecondaryWeaponIndex);
            }
        }
		if (Boss != null)
		{
			DecideBossColors(true);
		}

		if (randomizePlayerControls && Player1 != null && Player2 != null)
		{
			DetermineRandomPercents(hardMode);
			RandomizeControls();
		}
        
        if (!randomizePlayerControls && Player1 != null && Player2 != null)
        {
            Player1.LoadDefaultControls();
            Player2.LoadDefaultControls();
        }

        if (PlayerOnlineController.LocalPlayerInstance == null && onlineGame)
        {
            for (int i = 0; i < PhotonNetwork.room.PlayerCount; i++)
            {
                if (PlayerPrefs.HasKey("PlayerName"))
                {
                    if (PhotonNetwork.playerList[i].NickName == PlayerPrefs.GetString("PlayerName") + " ")
                    {
                        if (i == 0)
                        {
                            PlayerOnlineController.LocalPlayerInstance = OnlinePlayer1.gameObject;
                        }
                        else
                        {
                            PlayerOnlineController.LocalPlayerInstance = OnlinePlayer2.gameObject;
                        }
                        break;
                    }
                }
            }
        }
	}

    void Update()
    {
        if (enableDebugCommands)
        {
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                PlayerController[] players = FindObjectsOfType<PlayerController>();
                foreach (PlayerController pc in players)
                {
                    StartCoroutine(pc.TakeDamage(1000, "matt"));
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.Escape))
        {
            SceneManager.LoadScene("title");
        }
    }

    /**
	 * @brief Determines if the color order should be swapped or not, 70% chance to swap
	 * @param i The base number
	 * @returns The number whether it is altered or not
	 */
    public int DetermineColorOrder(int i)
	{
		float x = Random.Range(0.0f, 1.0f);

		if (x <= 0.7f)
			return i * -1;
		return i;
	}

	public void DetermineWeapons(bool applyToColor)
	{
		int[,] playerData = new int[2, 2];

		for (int i = 0; i < 2; i++)
			for (int j = 0; j < 2; j++)
				playerData[i, j] = -1;

		for (int i = 0; i < 3; i++)
		{
			int playerIndex = Random.Range(0, 2);
			bool thereWasRoom = false;
			for (int j = 0; j < 2; j++)
			{
				if (playerData[playerIndex, j] == -1)
				{
					playerData[playerIndex, j] = i;
					thereWasRoom = true;
					break;
				}
			}
			if (!thereWasRoom)
			{
				if (playerIndex == 0)
					playerIndex = 1;
				else
					playerIndex = 0;
				
				for (int j = 0; j < 2; j++)
				{
					if (playerData[playerIndex, j] == -1)
					{
						playerData[playerIndex, j] = i;
						thereWasRoom = true;
						break;
					}
				}
			}
		}

		if (playerData[0, 1] == -1)
		{
			playerData[0, 1] = playerData[0, 0];
			while (playerData[0, 1] == playerData[0, 0])
				playerData[0, 1] = Random.Range(0, 3);
		}
		else
		{
			playerData[1, 1] = playerData[1, 0];
			while (playerData[1, 1] == playerData[1, 0])
				playerData[1, 1] = Random.Range(0, 3);
		}

        if (forceStandardColors)
        {
            playerData[0, 0] = 0;
            playerData[0, 1] = 2;
            playerData[1, 0] = 1;
            playerData[1, 1] = 0;
        }

		Debug.Log("P1 weapons: " + playerData[0, 0].ToString() + " " + playerData[0, 1].ToString());
		Debug.Log("P2 weapons: " + playerData[1, 0].ToString() + " " + playerData[1, 1].ToString());

		playerOnePrimaryWeaponIndex = playerData[0, 0];
		playerOneSecondaryWeaponIndex = playerData[0, 1];
		playerTwoPrimaryWeaponIndex = playerData[1, 0];
		playerTwoSecondaryWeaponIndex = playerData[1, 1];

		if (applyToColor)
		{
			playerOnePrimaryColorIndex = playerOnePrimaryWeaponIndex;
			playerOneSecondaryColorIndex = playerOneSecondaryWeaponIndex;
			playerTwoPrimaryColorIndex = playerTwoPrimaryWeaponIndex;
			playerTwoSecondaryColorIndex = playerTwoSecondaryWeaponIndex;
		}
	}

	public float DamageModifier(int attackingColorIndex, int defendingColorIndex)
	{
		//if colors match, it is a draw
		if (attackingColorIndex == defendingColorIndex)
			return 0.0f;

		//positive num or 0 for cyan -> magenta -> yellow -> cyan (0 -> 1 -> 2)
		if (colorOrder >= 0)
		{
			if (attackingColorIndex == 2 && defendingColorIndex == 0)
				attackingColorIndex = -1;
			if (defendingColorIndex == 2 && attackingColorIndex == 0)
				defendingColorIndex = -1;

			//return double damage if attacker wins the matchup
			if (attackingColorIndex < defendingColorIndex)
				return 2.0f;
		}
		//negative num for cyan <- magenta <- yellow <- cyan (0 <- 1 <- 2)
		else
		{
			if (attackingColorIndex == 2 && defendingColorIndex == 0)
				attackingColorIndex = -1;
			if (defendingColorIndex == 2 && attackingColorIndex == 0)
				defendingColorIndex = -1;

			//return double damage if attacker wins the matchup
			if (attackingColorIndex > defendingColorIndex)
				return 2.0f;
		}

		//return half damage if defender wins the matchup
		return 0.5f;
	}

	void DetermineRandomPercents(int modifier)
	{
		/*private int invertMovementPercentage;
		[SerializeField][Range (0,100)]
		private int invertAimingPercentage;
		[SerializeField][Range (0,100)]
		private int swapFireAndDefendPercentage;
		[SerializeField][Range (0,100)]
		private int samePlayerControlsMovementPercentage;
		[SerializeField][Range (0,100)]
		private int samePlayerControlsFiringPercentage;
		[SerializeField][Range (0,100)]
		private int switchMoveAndFirePercentage;
		 */

		switch (modifier)
		{
		case 1:
			invertMovementPercentage = 30;
			invertAimingPercentage = 30;
			//swapFireAndDefendPercentage = 0;
			samePlayerControlsMovementPercentage = 30;
			samePlayerControlsFiringPercentage = 30;
			switchMoveAndFirePercentage = 30;
			break;
		case 2:
			invertMovementPercentage = 70;
			invertAimingPercentage = 70;
			//swapFireAndDefendPercentage = 0;
			samePlayerControlsMovementPercentage = 70;
			samePlayerControlsFiringPercentage = 70;
			switchMoveAndFirePercentage = 70;
			break;
		default:
			invertMovementPercentage = 0;
			invertAimingPercentage = 0;
			//swapFireAndDefendPercentage = 0;
			samePlayerControlsMovementPercentage = 0;
			samePlayerControlsFiringPercentage = 0;
			switchMoveAndFirePercentage = 0;
			break;
		}
	}

	void RandomizeControls()
	{
		/*private int invertMovementPercentage;
		private int invertAimingPercentage;
		private int swapFireAndDefendPercentage;
		private int samePlayerControlsMovementPercentage;
		private int samePlayerControlsFiringPercentage;
		switchMoveAndFirePercentage*/

		int rando = Random.Range(1, 101);
		if (rando <= invertMovementPercentage)
		{
			Player1.horizontalInvert *= -1.0f;
			Player1.verticalInvert *= -1.0f;
			Player2.horizontalInvert *= -1.0f;
			Player2.verticalInvert *= -1.0f;
		}

		rando = Random.Range(1, 101);
		if (rando <= invertAimingPercentage)
		{
			Player1.horizontal2Invert *= -1.0f;
			Player1.vertical2Invert *= -1.0f;
			Player2.horizontal2Invert *= -1.0f;
			Player2.vertical2Invert *= -1.0f;
		}

		rando = Random.Range(1, 101);
		if (rando <= swapFireAndDefendPercentage)
		{
            //Player1.fireAndDefendInvert *= -1.0f;
            //Player2.fireAndDefendInvert *= -1.0f;
            //Player1.SwapFireAndDefend = !Player1.SwapFireAndDefend;
            //Player2.SwapFireAndDefend = !Player2.SwapFireAndDefend;
		}

		rando = Random.Range(1, 101);
		if (rando <= samePlayerControlsMovementPercentage)
		{
			int whichPlayer = Random.Range(1, 3);
			if (whichPlayer == 1)
			{
				Player2.horizontalAxis = Player1.horizontalAxis;
				Player2.verticalAxis = Player1.verticalAxis;
			} 
			else
			{
				Player1.horizontalAxis = Player2.horizontalAxis;
				Player1.verticalAxis = Player2.verticalAxis;
			}
		}

		rando = Random.Range(1, 101);
		if (rando <= samePlayerControlsFiringPercentage)
		{
			int whichPlayer = Random.Range(1, 3);
			if (whichPlayer == 1)
			{
				Player2.horizontalAxis2 = Player1.horizontalAxis2;
				Player2.verticalAxis2 = Player1.verticalAxis2;
			} 
			else
			{
				Player1.horizontalAxis2 = Player2.horizontalAxis2;
				Player1.verticalAxis2 = Player2.verticalAxis2;
			}
		}

		rando = Random.Range(1, 101);
		if (rando <= switchMoveAndFirePercentage)
		{
			string temp = Player1.horizontalAxis;
			Player1.horizontalAxis = Player1.horizontalAxis2;
			Player1.horizontalAxis2 = temp;

			temp = Player1.verticalAxis;
			Player1.verticalAxis = Player1.verticalAxis2;
			Player1.verticalAxis2 = temp;

			temp = Player2.horizontalAxis;
			Player2.horizontalAxis = Player2.horizontalAxis2;
			Player2.horizontalAxis2 = temp;

			temp = Player2.verticalAxis;
			Player2.verticalAxis = Player2.verticalAxis2;
			Player2.verticalAxis2 = temp;
		}
	}

	void DecideBossColors(bool differentColors)
	{
		int color1 = Random.Range(0, 3);
		int color2 = color1;

		if (differentColors)
		{
			while (color1 == color2)
			{
				color2 = Random.Range(0, 3);
			}
		}

		Boss.SetBossColors(color1, playerColors[color1], color2, playerColors[color2]);
	}

	void UpdatePlayerInfo()
	{
		
	}

    public void IncrementWinStreak()
    {
        winStreak++;
        if (winStreak >= 10)
            AchievementManager.Instance.AwardAchievement(3);
        else if (winStreak >= 30)
            AchievementManager.Instance.AwardAchievement(5);
    }

    public void ResetWinStreak()
    {
        winStreak = 0;
    }

    public void DestroyAllBulletsAndSpawns()
    {
        Bullet[] bullets = FindObjectsOfType<Bullet>();
        foreach (Bullet b in bullets)
        {
            Destroy(b.gameObject);
        }

        StaplerMinion[] minions = FindObjectsOfType<StaplerMinion>();
        foreach (StaplerMinion s in minions)
        {
            Destroy(s.gameObject);
        }
    }

    public void ResetAllAchievements()
    {
        AchievementManager.Instance.ResetAchievements();
    }

    public int GetWinStreak()
    {
        return winStreak;
    }

    #region PUN Stuff

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void LoadLevelWithPhoton(string levelname)
    {
        if (!PhotonNetwork.isMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.Log("PhotonNetwork : Loading Level : " + levelname);
        PhotonNetwork.LoadLevel(levelname);
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerConnected(): " + other.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerConnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
            if (PhotonNetwork.room.PlayerCount >= 2)
            {
                LoadLevelWithPhoton("online_test");
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer other)
    {
        Debug.Log("OnPhotonPlayerDisconnected(): " + other.NickName);
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("OnPhotonPlayerDisonnected isMasterClient " + PhotonNetwork.isMasterClient); // called before OnPhotonPlayerDisconnected
            if (PhotonNetwork.room.PlayerCount < 2)
            {
                LoadLevelWithPhoton("title");
            }
        }
    }

    #endregion
}
