//Author:Via Cytus
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Com.MyCompany.MyGame;

namespace Com.Glassblade.Group1
{
    public class ShowTheResult : MonoBehaviourPunCallbacks
    {
        private playersData[] p;
        private List<playersData> pA = new List<playersData>();
        private List<playersData> pB = new List<playersData>();
        private GameObject[] teamA;
        private GameObject[] teamB;
        private AudioSystem audioSystem;
        private bool isConnecting;

        void Start()
        {
            audioSystem = (AudioSystem)FindObjectOfType(typeof(AudioSystem));
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player)
            {
                if (player.GetPhotonView().IsMine)
                    PhotonNetwork.Destroy(player);
            }
            // 普通模式
            if (GameObject.Find("NData") != null)
            {
                p = NDataStore.instance.p;
            }
            // 占领模式
            else if (GameObject.Find("OData") != null)
            {
                p = ODataStore.instance.p;
            }
            // 两个队伍的初始化
            teamA = GameObject.FindGameObjectsWithTag("Team0").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();
            teamB = GameObject.FindGameObjectsWithTag("Team1").OrderBy(g => g.transform.GetSiblingIndex()).ToArray();
            // 两个队伍的玩家数组
            // Debug.Log(p[0].name);
            for (int k = 0; k < p.Length; k++)
            {
                //  Debug.Log(k);
                if (p[k].team == TeamController.Team.TeamA)
                {
                    pA.Add(p[k]);
                }
                else if (p[k].team == TeamController.Team.TeamB)
                {
                    pB.Add(p[k]);
                }
            }
            // 根据得分排序
            pA.OrderBy(g => g.score);
            pB.OrderBy(g => g.score);
            // 将两队玩家的游戏结果传递出来
            for (int i = 0; i < teamA.Length; i++)
            {
                if (i < pA.Count)
                {
                    teamA[i].transform.GetChild(0).GetComponentInChildren<Text>().text = pA[i].name;
                    teamA[i].transform.GetChild(1).GetComponentInChildren<Text>().text = pA[i].killTime.ToString();
                    teamA[i].transform.GetChild(2).GetComponentInChildren<Text>().text = pA[i].deathTime.ToString();
                    teamA[i].transform.GetChild(3).GetComponentInChildren<Text>().text = pA[i].score.ToString();
                }
                else
                {
                    teamA[i].SetActive(false);
                }
            }

            for (int i = 0; i < teamB.Length; i++)
            {
                if (i < pB.Count)
                {
                    teamB[i].transform.GetChild(0).GetComponentInChildren<Text>().text = pB[i].name;
                    teamB[i].transform.GetChild(1).GetComponentInChildren<Text>().text = pB[i].killTime.ToString();
                    teamB[i].transform.GetChild(2).GetComponentInChildren<Text>().text = pB[i].deathTime.ToString();
                    teamB[i].transform.GetChild(3).GetComponentInChildren<Text>().text = pB[i].score.ToString();
                }
                else
                {
                    teamB[i].SetActive(false);
                }
            }
            // 普通模式的结果结算
            if (GameObject.Find("NData") != null)
            {
                int allKill0 = 0;
                int allKill1 = 0;
                foreach (playersData g in pA)
                {
                    allKill0 += g.killTime;
                }
                foreach (playersData g in pB)
                {
                    allKill1 += g.killTime;
                }
                if (allKill0 == allKill1)
                {
                    GameObject.Find("Title").GetComponentInChildren<Text>().text = "平局";
                    audioSystem.PlayEndMusic(1);
                }
                else if (allKill0 > allKill1 && (TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA
                    || allKill0 < allKill1 && (TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamB)
                {
                    GameObject.Find("Title").GetComponentInChildren<Text>().text = "胜利";
                    audioSystem.PlayEndMusic(1);
                }
                else
                {
                    GameObject.Find("Title").GetComponentInChildren<Text>().text = "失败";
                    audioSystem.PlayEndMusic(0);
                }
            }
            // 占点模式的结果结算
            else if (GameObject.Find("OData") != null)
            {

                switch (!(((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA) ^
                (GameObject.Find("OData").GetComponentInChildren<ODataStore>().scoreA < GameObject.Find("OData").GetComponentInChildren<ODataStore>().scoreB)))
                {
                    case true:
                        GameObject.Find("Title").GetComponentInChildren<Text>().text = "失败";
                        audioSystem.PlayEndMusic(0);
                        break;

                    case false:
                        GameObject.Find("Title").GetComponentInChildren<Text>().text =
                    (GameObject.Find("OData").GetComponentInChildren<ODataStore>().scoreA == GameObject.Find("OData").GetComponentInChildren<ODataStore>().scoreB) ?
                    "平局" : "胜利";
                        audioSystem.PlayEndMusic(1);
                        break;
                }
            }
        }



        void Update()
        {

        }

        #region Public Methods

        public void ExitGames()
        {
            if (GameObject.Find("NData"))
            {
                Destroy(GameObject.Find("NData"));
            }
            if (GameObject.Find("SettingStore"))
            {
                Destroy(GameObject.Find("SettingStore"));
            }

            PhotonNetwork.Disconnect();
            PhotonNetwork.ConnectUsingSettings();

            //if (!PhotonNetwork.IsConnected)
            //{
            //    PhotonNetwork.LoadLevel(0);
            //    //PhotonNetwork.ConnectUsingSettings();

            //}
            //else
            //{
            //    PhotonNetwork.JoinLobby();
            //}
            isConnecting = true;
        }

        public override void OnJoinedLobby()
        {
            PhotonNetwork.LoadLevel("Lobby");
            Debug.Log("[ShowTheResults:OnJoinedLobby()] Lobby loaded");
        }

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinLobby();
                isConnecting = false;
            }

            Debug.Log("[ShowTheResults:OnConnectedToMaster()] Now connected to master client.");
        }

        #endregion
    }


}