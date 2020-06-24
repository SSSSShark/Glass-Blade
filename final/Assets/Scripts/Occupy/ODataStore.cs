// Author: Via Cytus
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.Glassblade.Group1
{
    public class ODataStore : MonoBehaviour
    {
        // 玩家数据
        public playersData[] p;
        public static ODataStore instance;
        public int scoreA;
        public int scoreB;


        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Refresh()
        {
            //玩家组件列表
            instance = this;
            //玩家组件列表
            GameObject[] Players = null;
            //实例化玩家组件列表
            Players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(Players.Length);
            p = new playersData[Players.Length];
            for (int i = 0; i < Players.Length; i++)
            {
                p[i].name = Players[i].transform.GetChild(6).GetComponentInChildren<TextMesh>().text;
                p[i].killTime = Players[i].GetComponentInChildren<PlayerCharacter>().killTime;
                p[i].deathTime = Players[i].GetComponentInChildren<PlayerCharacter>().deathTime;
                p[i].score = Players[i].GetComponentInChildren<PlayerCharacter>().score;
                p[i].team = (TeamController.Team)Players[i].GetComponentInChildren<PlayerCharacter>().team;
            }
            scoreA = GameObject.FindGameObjectWithTag("ScoreA").GetComponentInChildren<Scores>().score;
            scoreB = GameObject.FindGameObjectWithTag("ScoreB").GetComponentInChildren<Scores>().score;
        }
    }

    public struct playersData
    {
        // 玩家姓名
        public string name;
        // 击杀次数
        public int killTime;
        // 死亡次数
        public int deathTime;
        // 玩家得分
        public int score;
        // 队伍编号
        public TeamController.Team team;
    };
}