// Author: wmj
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

namespace Com.Glassblade.Group1
{
    public class CaptureGameCtrl : MonoBehaviour
    {
        static public int teamCount = 2;
        //scan period
        public float period;
        //score increment per period
        public int increment;
        //win point
        public int win;
        //all points in game
        private OMode[] points;
        //team score
        private int[] score = new int[teamCount];

        private float timer;

        private void Start()
        {
            //get all pionts
            points = FindObjectsOfType<OMode>();
            //timer starts
            timer = 0;
            //init 0 points
            for (int i = 0; i < score.Length; i++)
            {
                score[i] = 0;
            }
        }

        /// <summary>
        /// scan point every fixed cycle, add score to team
        /// </summary>
        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if ((timer += Time.deltaTime) >= period)
                {
                    timer -= period;
                    foreach (OMode p in points)
                    {
                        if (p.team != TeamController.Team.unknown)
                        {
                            score[(int)p.team] += increment;
                            if (p.team == TeamController.Team.TeamA)
                            {
                                GameObject.FindGameObjectWithTag("ScoreA").GetComponent<Scores>().SendScoreInfo(increment);
                            }
                            if (p.team == TeamController.Team.TeamB)
                            {
                                GameObject.FindGameObjectWithTag("ScoreB").GetComponent<Scores>().SendScoreInfo(increment);
                            }
                            if (score[(int)p.team] >= win)
                            {
                                // GameObject.FindWithTag("Finish").GetComponentInChildren<ODataStore>().Refresh();
                                // SceneManager.LoadSceneAsync("ending");
                                PhotonView photonView = PhotonView.Get(this);
                                photonView.RPC("LoadEnding", RpcTarget.All);
                            }
                        }
                    }
                }
            }
        }
        
        [PunRPC]
        public void LoadEnding()
        {
            GameObject.FindWithTag("Finish").GetComponentInChildren<ODataStore>().Refresh();
            SceneManager.LoadSceneAsync("ending");
        }
    }

}