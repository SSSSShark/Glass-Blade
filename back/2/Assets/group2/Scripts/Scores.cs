using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scores : MonoBehaviour
{

    [SerializeField]
    private int score;            
    private Text ScoreObj;
    

    void Start()
    {
        score = 0;
        ScoreObj = GetComponentInChildren<Text>();         //获取时间文本组件
    }

    [PunRPC]
    public void GetScoreInfo()
    {
        Debug.Log("score called");
        score += 1;
    }

    public void _GetScoreInfo()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("GetScoreInfo", RpcTarget.MasterClient);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ScoreObj.text = ("0" + score.ToString()).Substring(0, 2);
            Debug.Log(score.ToString());
        }
    }
}
