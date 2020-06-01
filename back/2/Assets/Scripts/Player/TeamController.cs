using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using System;

public class TeamController : MonoBehaviourPunCallbacks
{

    #region Public Fields

    public GameObject[] TeamAList;
    public GameObject[] TeamBList;
    public GameObject[] AddOrderList;
    public GameObject startDialog;
    public Button settingbtn;
    public Button changeTeambtn;
    public Button skillSettingbtn;
    public GameObject messagebox;
    

    #endregion

    #region Private Fields

    public enum Team
    {
        TeamA,
        TeamB,
        unknown
    };

    public enum Status
    {
        Wait,
        Ready,
        Host,
        unknown
    };

    int NumTeamA;
    int NumTeamB;
    #endregion

    // Start is called before the first frame update
    #region MonoBehaviour CallBack
    void Start()
    {
        settingbtn.gameObject.SetActive(false);

        //this is only for MasterClient
        if (PhotonNetwork.IsMasterClient)
        {
            settingbtn.gameObject.SetActive(true); 
            Button btnObj = GameObject.FindGameObjectWithTag("Start").GetComponent<Button>();
            btnObj.transform.Find("Text").GetComponent<Text>().text = "开始游戏";

            Debug.Log("[TeamController:Start()] Master client: initialize obj table");

            TeamAList = GameObject.FindGameObjectsWithTag("TeamA").OrderBy(a => -a.GetComponent<RectTransform>().position.y).ToArray();
            TeamBList = GameObject.FindGameObjectsWithTag("TeamB").OrderBy(a => -a.GetComponent<RectTransform>().position.y).ToArray();
            AddOrderList = new GameObject[TeamAList.Length + TeamBList.Length];
            int j = 0;

            for (int i = 0; i < (TeamAList.Length > TeamBList.Length ? TeamAList.Length : TeamBList.Length); i++)
            {
                if (i < TeamAList.Length)
                {
                    AddOrderList[j] = TeamAList[i];
                    j++;
                }
                if (i < TeamBList.Length)
                {
                    AddOrderList[j] = TeamBList[i];
                    j++;
                }
            }

            Debug.Log("[TeamController:Start()] Master client: initialize team");

            // add initial play for the first time
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
                // we set the property of the player
                if (NumTeamB >= NumTeamA)
                {
                    NumTeamA++;
                    props.Add("team", Team.TeamA);   
                }
                else
                {
                    NumTeamB++;
                    props.Add("team", Team.TeamB);
                }
                if (p == PhotonNetwork.MasterClient)
                {
                    props.Add("status", Status.Host);
                }
                else
                {
                    props.Add("status", Status.Wait);
                }
                p.SetCustomProperties(props);
            }

            Debug.Log("[TeamController:Start()] Master client: display the player's nick name");

            //Synchronize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // do nothing
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[TeamController:OnPlayerPropertiesUpdate()] Update after property update");
            Synchronize();
        }
        
    }

    #endregion

    #region PUN Callback
    public override void OnLeftRoom()
    {
        Debug.Log("[TeamController:OnLeftRoom()] " + PhotonNetwork.NickName + " Left the room");
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinLobby();
        }
        PhotonNetwork.LoadLevel(0);
    }

    /// <summary>
    /// Called when a player entered the room, if we are the
    /// master client, we update the PlayerMap, automatically
    /// determine a team for the player and update the display,
    /// otherwise we do nothing
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[TeamController:OnPlayerEnteredRoom()] Player " + newPlayer.NickName + " joined");

            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable();
            if (NumTeamA <= NumTeamB)
            {
                NumTeamA++;
                props.Add("team", Team.TeamA);
            }
            else
            {
                NumTeamB++;
                props.Add("team", Team.TeamB);
            }
            props.Add("status", Status.Wait);
            newPlayer.SetCustomProperties(props);

            // update
            Synchronize();
        }
    }

    /// <summary>
    /// Called when a player left the room, if we are the master
    /// client, we delete the player from the PlayerMap and update
    /// the display.
    /// </summary>
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        //if the host leaves, then the player with the minimum ID will be the next host automatically
        if (PhotonNetwork.IsMasterClient)
        {

            Debug.Log("[TeamController:OnPlayerLeftRoom()] Player " + otherPlayer.NickName + " leaved");
            if ((Status)otherPlayer.CustomProperties["status"] == Status.Host)
            {
                Start();
            }

            else
            {
                if ((Team)otherPlayer.CustomProperties["team"] == Team.TeamA)
                {
                    NumTeamA--;
                }
                else
                {
                    NumTeamB--;
                }
                // Update list
                Synchronize();
            }
            
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogWarning("[TeamController:OnDisconnected()] lose connection");
        PhotonNetwork.LoadLevel(0);
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Called by the master client ONLY, update the team display info
    /// </summary>
    public void Synchronize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("[TeamController:Synchronize()] Synchronizing, with team A: " + NumTeamA + " teamB: " + NumTeamB);
            int TeamAIndex = 0;
            int TeamBIndex = 0;
            // we use the master to allocate team
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if ((Team)p.CustomProperties["team"] == Team.TeamA)
                {
                    TeamAList[TeamAIndex].GetComponent<Text>().text = p.NickName + (p == PhotonNetwork.MasterClient ? "-host" : "");
                    if ((Status)p.CustomProperties["status"] == Status.Ready)
                    {
                        TeamAList[TeamAIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = true;
                        Debug.LogFormat("[TeamController:Synchronize()] Toggle for TeamA[{0}] is on", TeamAIndex);
                    }
                    else
                    {
                        TeamAList[TeamAIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = false;
                    }
                }
                else
                {
                    TeamBList[TeamBIndex].GetComponent<Text>().text = p.NickName + (p == PhotonNetwork.MasterClient ? "-host" : "");
                    if ((Status)p.CustomProperties["status"] == Status.Ready)
                    {
                        TeamBList[TeamBIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = true;
                        Debug.LogFormat("[TeamController:Synchronize()] Toggle for TeamB[{0}] is on", TeamBIndex);
                    }
                    else
                    {
                        TeamBList[TeamBIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = false;
                    }
                }
                NumTeamA = TeamAIndex;
                NumTeamB = TeamBIndex;
                Debug.Log("[TeamController:Synchronize()] Infomation updated, with team A: " + NumTeamA + " teamB: " + NumTeamB);
            }

            while (TeamAIndex < 5)
            {
                TeamAList[TeamAIndex].GetComponent<Text>().text = "空";
                TeamAList[TeamAIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = false;
            }

            while (TeamBIndex < 5)
            {
                TeamBList[TeamBIndex].GetComponent<Text>().text = "空";
                TeamBList[TeamBIndex++].transform.Find("Ready").GetComponent<RawImage>().enabled = false;
            }
        }
        else
        {
            Debug.LogError("[TeamController:Synchronize()] BUG: synchronize called by client other than mastr client!");
        }
    }

    public void changeTeam()
    {
        ExitGames.Client.Photon.Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
        if ((Team)props["team"] == Team.TeamA)
        {
            props["team"] = Team.TeamB;
        }
        else
        {
            props["team"] = Team.TeamA;
        }
        PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        //the Numteam will be updated during sync

        Debug.Log("[TeamController:changeTeam()] Change team already");
    }

    public void CancelReady()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            changeTeambtn.enabled = true;
            skillSettingbtn.enabled = true;
            changeTeambtn.transform.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
            skillSettingbtn.transform.GetComponent<Image>().color = new Color(172.0f/255.0f, 217.0f/255f, 219.0f/255.0f);
            ExitGames.Client.Photon.Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
            props["status"] = Status.Wait;
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            Button btnObj = GameObject.FindGameObjectWithTag("Start").GetComponent<Button>();
            btnObj.transform.Find("Text").GetComponent<Text>().text = "准备";
            try
            {
                btnObj.onClick.RemoveListener(CancelReady);
            }
            catch
            {
                Debug.LogWarning("[TeamController:CancelReady()] RemoveListener Failed");
            }
            finally
            {
                btnObj.onClick.AddListener(ReadytoGame);
            }
        }
    }

    public void ReadytoGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p != PhotonNetwork.MasterClient && (Status)p.CustomProperties["status"] == Status.Wait)
                {
                    if (IsInvoking("DialogVanish"))
                    {
                        CancelInvoke("DialogVanish");
                    }
                    startDialog.SetActive(true);
                    Invoke("DialogVanish", 1);
                    Debug.Log("[TeamController:ReadytoGame()] Someone is not ready yet");
                    return;
                }
            }
            LoadArena();
        }
        else
        {
            changeTeambtn.enabled = false;
            skillSettingbtn.enabled = false;
            changeTeambtn.transform.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);
            skillSettingbtn.transform.GetComponent<Image>().color = new Color(0.8f, 0.8f, 0.8f);

            ExitGames.Client.Photon.Hashtable props = PhotonNetwork.LocalPlayer.CustomProperties;
            props["status"] = Status.Ready;

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);       

            Button btnObj = GameObject.FindGameObjectWithTag("Start").GetComponent<Button>();
            btnObj.transform.Find("Text").GetComponent<Text>().text = "取消准备"; 

            try
            {
                btnObj.onClick.RemoveListener(ReadytoGame);
            }
            catch
            {
                Debug.LogWarning("[TeamController:ReadytoGame()] RemoveListener Failed");
            }
            finally
            {
                btnObj.onClick.AddListener(CancelReady);
            }
            
            Debug.Log("[TeamController:ReadytoGame()] modify status to ready");
        }
    }

    /// <summary>
    /// Called when a player clicked the leave button
    /// </summary>
    /// TODO: if the master client wants to leave, we need to transfer the ownership,
    ///       which is not yet implemented!
    public void LeaveRoom()
    {
        if (GameObject.Find("AudioSystem"))
        {
            Destroy(GameObject.Find("AudioSystem"));
        }
        PhotonNetwork.LeaveRoom();
    }
    #endregion

    #region Private Methods


    void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("[TeamController:LoadArena()] BUG: PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        Debug.LogFormat("[TeamController:LoadArena()] PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.CurrentRoom.IsOpen = false;  //can't be joined
        switch (messagebox.GetComponent<SettingBox>().mode) {
            case SettingBox.GameMode.Normal: PhotonNetwork.LoadLevel("NormalMode"); break;
            case SettingBox.GameMode.Occupation: PhotonNetwork.LoadLevel("OccupationPattern"); break;
        }

        //PhotonNetwork.LoadLevel("SampleScene");
        //PhotonNetwork.LoadLevel("NormalMode");
    }

    void DialogVanish()
    {
        if (startDialog.activeInHierarchy)
        {
            startDialog.SetActive(false);
        }
    }

    #endregion

}

