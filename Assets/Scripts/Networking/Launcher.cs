using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : Photon.PunBehaviour
{
    #region Public Variables
    public byte MaxPlayersPerRoom = 2;
    #endregion


    #region Private Variables
    string _gameVersion = "1";
    private bool isConnecting;
    [SerializeField]
    private float timeBeforeSwitchingScene = 3.0f;
    private float t = 0.0f;
    private bool startSwitchingScenes;
    [SerializeField]
    private PhotonLogLevel LogLevel = PhotonLogLevel.Informational;
    [SerializeField]
    private Text numPlayersText;
    private int _numPlayers = 0;
    private int numPlayersConnected = 0;
    public int NumPlayersConnected
    {
        get
        {
            return numPlayersConnected;
        }
    }
    [SerializeField]
    private GameObject connectingPanel;
    [SerializeField]
    private GameObject[] onePlayerEnableObjects;
    [SerializeField]
    private GameObject[] onePlayerDisableObjects;
    [SerializeField]
    private GameObject[] twoPlayerEnableObjects;
    [SerializeField]
    private GameObject[] twoPlayerDisableObjects;
    [SerializeField]
    private Text playerOneNameText;
    [SerializeField]
    private Text playerTwoNameText;
    #endregion


    #region MonoBehaviour CallBacks

    void Awake()
    {
        PhotonNetwork.logLevel = LogLevel;
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (numPlayersText != null && PhotonNetwork.connectedAndReady)
            if (PhotonNetwork.room != null)
                numPlayersText.text = PhotonNetwork.room.PlayerCount.ToString();

        if (PhotonNetwork.connected)
        {
            if (PhotonNetwork.room != null)
            {
                numPlayersConnected = PhotonNetwork.room.PlayerCount;
            }
        }

        //there was a change so execute some function based on number of players
        if (_numPlayers != numPlayersConnected)
        {
            _numPlayers = NumPlayersConnected;
            //execute something
            EnableAndDisableObjects(_numPlayers);
        }

        if (playerOneNameText != null && numPlayersConnected >= 1)
            playerOneNameText.text = PhotonNetwork.playerList[0].NickName;
        if (playerTwoNameText != null && numPlayersConnected >= 2)
            playerTwoNameText.text = PhotonNetwork.playerList[1].NickName;
	}

    void FixedUpdate()
    {
        if (startSwitchingScenes)
        {
            t += Time.deltaTime;
            if (t >= timeBeforeSwitchingScene)
            {
                GameManager.Instance.LoadLevelWithPhoton("online_test");
            }
        }
    }

    #endregion


    #region Public Methods

    public void EnableAndDisableObjects(int numPlayers)
    {
        switch (numPlayers)
        {
            case 1:
                foreach (GameObject obj in onePlayerEnableObjects)
                    obj.SetActive(true);
                foreach (GameObject obj in onePlayerDisableObjects)
                    obj.SetActive(false);
                break;
            case 2:
                foreach (GameObject obj in twoPlayerEnableObjects)
                    obj.SetActive(true);
                foreach (GameObject obj in twoPlayerDisableObjects)
                    obj.SetActive(false);
                break;
            default:
                //disconnect from room, too
                foreach (GameObject obj in onePlayerEnableObjects)
                    obj.SetActive(false);
                foreach (GameObject obj in onePlayerDisableObjects)
                    obj.SetActive(false);
                foreach (GameObject obj in twoPlayerEnableObjects)
                    obj.SetActive(false);
                foreach (GameObject obj in twoPlayerDisableObjects)
                    obj.SetActive(false);
                break;
        }
    }

    public void ConnectToRandomRoom()
    {
        isConnecting = true;
        if (PhotonNetwork.connected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings(_gameVersion);
        }
    }

    #endregion


    #region Photon.PunBehaviour CallBacks

    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
        }
    }

    public override void OnDisconnectedFromPhoton()
    {
        Debug.LogWarning("Launcher: OnDisconnectedFromPhoton() was called by PUN");
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        Debug.Log("Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 2}, null);");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MaxPlayersPerRoom }, null);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (connectingPanel != null)
            connectingPanel.SetActive(false);

        if (PhotonNetwork.room.PlayerCount == 2)
        {
            startSwitchingScenes = true;
        }
    }

    #endregion
}
