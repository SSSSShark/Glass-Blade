﻿// Author: Via Cytus
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.Glassblade.Group1
{
    //public struct playersData
    //{
    //    // 玩家姓名
    //    public string name;
    //    // 击杀次数
    //    public int killTime;
    //    // 死亡次数
    //    public int deathTime;
    //    // 玩家得分
    //    public int score;
    //    // 队伍编号
    //    public int team;
    //};

    public class NDataStore : MonoBehaviour
    {
        public playersData[] p;
        public static NDataStore instance;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Refresh()
        {
            instance = this;
            //玩家组件列表
            GameObject[] Players = null;
            //实例化玩家组件列表
            Players = GameObject.FindGameObjectsWithTag("Player");
            Debug.Log(Players.Length);
            p = new playersData[Players.Length];
            // 将玩家的姓名、击杀次数、死亡次数、得分、队伍编号储存
            for (int i = 0; i < Players.Length; i++)
            {
                p[i].name = Players[i].transform.GetChild(6).GetComponentInChildren<TextMesh>().text;
                p[i].killTime = Players[i].GetComponentInChildren<PlayerCharacter>().killTime;
                p[i].deathTime = Players[i].GetComponentInChildren<PlayerCharacter>().deathTime;
                p[i].score = Players[i].GetComponentInChildren<PlayerCharacter>().score;
                p[i].team = (TeamController.Team)Players[i].GetComponentInChildren<PlayerCharacter>().team;
            }
        }
    }
}