using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Glassblade.Group1
{
    public struct playersData
    {
        public string name;
        public int killTime;
        public int deathTime;
        public int score;
        public int team;
    };

    public class NDataStore : MonoBehaviour
    {
        public playersData[] p;
        public static NDataStore instance;

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }

        public void Refresh()
        {
            //玩家组件列表
            GameObject[] Players = null;
            //实例化玩家组件列表
            Players = GameObject.FindGameObjectsWithTag("Player");
            p = new playersData[Players.Length];
            for(int i=0;i<Players.Length;i++)
            {
                p[i].name = Players[i].transform.GetChild(6).GetComponentInChildren<TextMesh>().text;
                p[i].killTime = Players[i].GetComponentInChildren<NCharacterBehavior>().killTime;
                p[i].deathTime = Players[i].GetComponentInChildren<NCharacterBehavior>().deathTime;
                p[i].score = Players[i].GetComponentInChildren<NCharacterBehavior>().score;
                p[i].team = Players[i].GetComponentInChildren<NCharacterBehavior>().team;
            }
        }
    }
}