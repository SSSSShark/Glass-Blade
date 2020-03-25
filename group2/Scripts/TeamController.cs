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

    // Team players
    // use a dictionary(map) to track use of the players
    Dictionary<Player, Team> PlayerMap;

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

            // initialize team
            PlayerMap = new Dictionary<Player, Team>();

            // add initial play for the first time
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (NumTeamB >= NumTeamA)
                {
                    PlayerMap.Add(p, Team.TeamA);
                    NumTeamA ++;
                }
                else
                {
                    PlayerMap.Add(p, Team.TeamB);
                    NumTeamB ++;
                }
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

    public override void OnPlayerEnteredRoom (Player newPlayer)
    {
        Debug.Log("Player " + newPlayer.NickName + " joined");
        if(NumTeamA <= NumTeamB)
        {
            PlayerMap.Add(newPlayer, Team.TeamA);
            NumTeamA ++;
        }
        else
        {
            PlayerMap.Add(newPlayer, Team.TeamB);
            NumTeamB ++;
        }

        // update
        Synchronize();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("Player " + otherPlayer.NickName + " leaved");

        // find the player in the place
        Team t = PlayerMap[otherPlayer];

        // TODO: use a new interface to implement this
        if(t == Team.TeamA)
        {
            NumTeamA --;
        }
        else
        {
            NumTeamB --;
        }

        PlayerMap.Remove(otherPlayer);

        // Update list
        Synchronize();
    }

    #endregion

    #region Public Methods
    public void Synchronize()
    {
        Debug.Log("Synchronize() called, with team A: " + NumTeamA + " teamB: " + NumTeamB);
        int TeamAIndex = 0;
        int TeamBIndex = 0;
        // we use the master to allocate team
        foreach (KeyValuePair<Player, Team> p in PlayerMap)
        {
            if(p.Value == Team.TeamA)
            {
                TeamAList[TeamAIndex ++].GetComponent<Text>().text = p.Key.NickName;
            }
            else
            {
                TeamBList[TeamBIndex ++].GetComponent<Text>().text = p.Key.NickName;
            }
        }

        while(TeamAIndex < 5)
        {
            TeamAList[TeamAIndex ++].GetComponent<Text>().text = "空";
        }

        while(TeamBIndex < 5)
        {
            TeamBList[TeamBIndex ++].GetComponent<Text>().text = "空";
        }
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    #endregion
}

