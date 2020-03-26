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

    #endregion

    #region Private Fields

    public enum Team
    {
        TeamA,
        TeamB,
        unknown
    };

    int NumTeamA;
    int NumTeamB;

    #endregion

    // Start is called before the first frame update
    #region MonoBehaviour CallBack
    void Start()
    {
        Debug.Log("Team controller start()");

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Master client: initialize obj table");

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

            Debug.Log("Master client: initialize team");

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

                p.SetCustomProperties(props);
            }

            Debug.Log("Master client: display the player's nick name");

            Synchronize();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // do nothing
    }

    #endregion

    #region PUN Callback
    public override void OnLeftRoom()
    {
        Debug.Log("Left the room");
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
            Debug.Log("Player " + newPlayer.NickName + " joined");

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
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Player " + otherPlayer.NickName + " leaved");

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

    #endregion

    #region Public Methods

    /// <summary>
    /// Called by the master client ONLY, update the team display info
    /// </summary>
    public void Synchronize()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Synchronize() called, with team A: " + NumTeamA + " teamB: " + NumTeamB);
            int TeamAIndex = 0;
            int TeamBIndex = 0;
            // we use the master to allocate team
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if ((Team)p.CustomProperties["team"] == Team.TeamA)
                {
                    TeamAList[TeamAIndex++].GetComponent<Text>().text = p.NickName;
                }
                else
                {
                    TeamBList[TeamBIndex++].GetComponent<Text>().text = p.NickName;
                }
            }

            while (TeamAIndex < 5)
            {
                TeamAList[TeamAIndex++].GetComponent<Text>().text = "空";
            }

            while (TeamBIndex < 5)
            {
                TeamBList[TeamBIndex++].GetComponent<Text>().text = "空";
            }
        }
        else
        {
            Debug.LogError("synchronize called by client other than mastr client!");
        }
    }

    /// <summary>
    /// Called when a player clicked the leave button
    /// </summary>
    /// TODO: if the master client wants to leave, we need to transfer the ownership,
    ///       which is not yet implemented!
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion

}

