using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class LobbyController : MonoBehaviourPunCallbacks
{
    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 10;

    #endregion

    #region Public Fields

    public GameObject scrollView;
    public GameObject lobbyPanel;
    public GameObject waitingPanel;
    public GameObject roomItem;

    #endregion

    #region MonoBehaviour CallBacks
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
        waitingPanel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {
        // #Critical
        // this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        base.OnRoomListUpdate(roomList);

        Debug.Log("OnRoomListUpdate");

        GameObject[] a = GameObject.FindGameObjectsWithTag("OneRoom");
        foreach (var r in a)
        {
            Destroy(r);   //destroy previous UI
        }

        // update
        foreach (RoomInfo r in roomList)
        {
            if (!r.IsOpen || !r.IsVisible || r.RemovedFromList)
            {
                continue;
            }
            GameObject roomObj = GameObject.Instantiate(roomItem) as GameObject;

            string maxPlayer = r.MaxPlayers.ToString();
            string currentPlayer = r.PlayerCount.ToString();
            string roomName = r.Name;

            Text t1 = roomObj.transform.Find("RoomName").GetComponent<Text>();
            t1.text = roomName;

            Text t2 = roomObj.transform.Find("RoomStatus").GetComponent<Text>();
            t2.text = "(" + currentPlayer + "/" + maxPlayer + ")";

            roomObj.name = roomName;
            roomObj.transform.SetParent(lobbyPanel.transform);
            roomObj.transform.localScale = Vector3.one;

            // check if the room is allowed to join
            if (r.PlayerCount < r.MaxPlayers)
            {
                roomObj.transform.Find("Join").GetComponent<Button>().onClick.AddListener(delegate ()
                {
                    this.JoinSpecifiedRoom(roomObj.name);
                }
                );
            }

        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("Failed to join room");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom.ToStringFull());

        if (PhotonNetwork.InLobby)
        {
            PhotonNetwork.LeaveLobby(); //stop receiving roomlist infos
        }

        // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Grouping Room' ");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Grouping Room");
        }
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    public override void OnLeftLobby()
    {
        base.OnLeftLobby();
        Debug.Log("Left the lobby");
        PhotonNetwork.LoadLevel(0);
    }

    #endregion

    #region Public Methods

    public void CreateRoom()
    {
        Debug.Log("Create Room");
        if (PhotonNetwork.IsConnected)
        {
            waitingPanel.SetActive(true);
            scrollView.SetActive(false);
            // this imples that each player has unique Nickname
            PhotonNetwork.CreateRoom(PhotonNetwork.LocalPlayer.NickName + "'s room", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
            // 房间名怎么定？
        }
        else
        {
            Debug.LogError("No connection");
        }
    }

    public void JoinRandomRoom()
    {
        waitingPanel.SetActive(true);
        scrollView.SetActive(false);
        PhotonNetwork.JoinRandomRoom();
    }

    public void JoinSpecifiedRoom(string roomName)
    {
        waitingPanel.SetActive(true);
        scrollView.SetActive(false);
        PhotonNetwork.JoinRoom(roomName);
    }

    public void LeaveLobby()
    {
        PhotonNetwork.LeaveLobby();
    }

    #endregion
}
