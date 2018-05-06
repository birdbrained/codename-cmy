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
    [SerializeField]
    private PhotonLogLevel LogLevel = PhotonLogLevel.Informational;
    [SerializeField]
    private Text numPlayersText;
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
	}

    #endregion


    #region Public Methods

    public void ConnectToRandomRoom()
    {
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
        PhotonNetwork.JoinRandomRoom();
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
    }

    #endregion
}
