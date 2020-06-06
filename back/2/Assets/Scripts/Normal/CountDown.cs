//Author: David Wang
//Function: To show countdown time, component of Canvas/Information/CountDown
//Todo:Add Canvas to the scene, then add Information.prefab,add this script in CountDown
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System;
using UnityEngine.SceneManagement;
using Com.Glassblade.Group1;
//modified by GaoYan

public class CountDown : MonoBehaviour
{
    private int minute;              //最大时间
    [SerializeField]
    private int getTime;            //游戏剩余时间
    private float timer = 0;        //计帧
    public Text countTimeText;      //时间文本
    public PhotonView photonView;

    void Start()
    {
        minute = GameObject.Find("SettingStore").GetComponent<SettingStore>().setTime;
        getTime = 60 * minute;      //获取游戏剩余时间
        countTimeText = GetComponentInChildren<Text>();         //获取时间文本组件
    }


    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {//转换场景
            if (getTime <= 0)
            {
                if (!photonView)
                {
                    photonView = GameObject.FindGameObjectWithTag("Show").GetComponent<PhotonView>();
                }
                photonView.RPC("LoadEnding", RpcTarget.All);
            }
            int M = getTime / 60;       //获取分钟
            int S = getTime % 60;       //获取秒钟
            timer += Time.deltaTime;    //帧数加
            if (timer >= 1f)             //满一秒
            {
                timer = 0;              //清零
                getTime--;             //剩余时间-1
                countTimeText.text = M + ":" + string.Format("{0:00}", S);           //设置时间文本
            }
        }
    }

}
