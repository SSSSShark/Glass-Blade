//Author: wmj

//modified by GaoYan to adapt to mulitiple players case
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Bush : MonoBehaviour
{
    CameraFollow cam;       //相机跟随组件
    ArrayList players;      //角色列表
    int allies_cnt;         //队友数量
    void Start()
    {
        cam= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();       //获取相机跟随组价
        players = new ArrayList();          //实例化角色列表
        allies_cnt = 0;                     //队友数量清零
    }

    private void OnTriggerEnter(Collider other)
    {
        CharacterBehavior player = other.GetComponent<CharacterBehavior>();
        if(player)
        {
            if ((int)other.GetComponent<PhotonView>().Owner.CustomProperties["team"] == (int)PhotonNetwork.LocalPlayer.CustomProperties["team"])         //如果是友军
            {
                if(allies_cnt==0)               //友军数量为0
                {
                    foreach(CharacterBehavior p in players)       //对角色列表中每个人
                    {
                        p.SetTransparent(0.5f);         //设置为半透明
                    }
                }
                allies_cnt++;       //友军数量+1
            }
            player.SetTransparent(allies_cnt == 0 ? 0f: 0.5f);      //新来的是否可见
            players.Add(player);        //加入角色列表
        }
    }
    //大致同上
    private void OnTriggerExit(Collider other)
    {
        CharacterBehavior player = other.GetComponent<CharacterBehavior>();
        if (player)
        {
            players.Remove(player);
            player.SetTransparent(1f);
            if ((int)other.GetComponent<PhotonView>().Owner.CustomProperties["team"]== (int)PhotonNetwork.LocalPlayer.CustomProperties["team"])
            {
                allies_cnt--;
                if (allies_cnt == 0)
                {
                    foreach (CharacterBehavior p in players)
                    {
                        p.SetTransparent(0f);
                    }
                }
            }
        }
    }
}
