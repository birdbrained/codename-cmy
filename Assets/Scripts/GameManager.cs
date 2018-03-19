using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
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

	//the pool of colors to pick from
	[SerializeField]
	private Color[] playerColors;

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

	//determine which color beats which color: 
	//	positive num or 0 for cyan -> magenta -> yellow -> cyan (0 -> 1 -> 2)
	//	negative num for      cyan <- magenta <- yellow <- cyan (0 <- 1 <- 2)
	private int colorOrder;
	public bool randomizePlayerControls = false;

	// Use this for initialization
	void Start () 
	{
		DetermineWeapons(colorIndexMatchesWeaponIndex);
		if (Player1 != null && Player2 != null)
		{
			Player1.SetCurrentColors(playerOnePrimaryColorIndex, playerOneSecondaryColorIndex);
			Player2.SetCurrentColors(playerTwoPrimaryColorIndex, playerTwoSecondaryColorIndex);
			Player1.SetCurrentWeapons(playerOnePrimaryWeaponIndex, playerOneSecondaryWeaponIndex);
			Player2.SetCurrentWeapons(playerTwoPrimaryWeaponIndex, playerTwoSecondaryWeaponIndex);
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
			playerData[0, 1] = Random.Range(0, 3);
		else
			playerData[1, 1] = Random.Range(0, 3);

		Debug.Log("P1 weapons: " + playerData[0, 0].ToString() + " " + playerData[0, 1].ToString());
		Debug.Log("P2 weapons: " + playerData[1, 0].ToString() + " " + playerData[1, 1].ToString());

		playerOnePrimaryWeaponIndex = playerData[0, 0];
		playerOneSecondaryWeaponIndex = playerData[0, 1];
		playerTwoPrimaryWeaponIndex = playerData[1, 0];
		playerTwoSecondaryWeaponIndex = playerData[1, 1];

		if (applyToColor)
		{
			playerOnePrimaryColorIndex = playerData[0, 0];
			playerOneSecondaryColorIndex = playerData[0, 1];
			playerTwoPrimaryColorIndex = playerData[1, 0];
			playerTwoSecondaryColorIndex = playerData[1, 1];
		}
	}

	public float DamageModifier(int attackingColorIndex, int defendingColorIndex)
	{
		//if colors match, it is a draw
		if (attackingColorIndex == defendingColorIndex)
			return 1.0f;

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

	void RandomizeControls()
	{
		
	}

	void UpdatePlayerInfo()
	{
		
	}
}
