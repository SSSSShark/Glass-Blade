using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scores : MonoBehaviour, IPunObservable
{

    [SerializeField]
    public int score;            
    private Text scoreObj;
    

    void Start()
    {
        score = 0;
        scoreObj = GetComponentInChildren<Text>();         //获取时间文本组件
    }

    [PunRPC]
    public void GetScoreInfo(int increment)
    {
        Debug.Log("score called");
        this.score += increment;
    }

    public void SendScoreInfo(int increment = 1)
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("GetScoreInfo", RpcTarget.MasterClient, increment);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (score >= 1000)
            {
                //设置分数文本
                scoreObj.text = score.ToString();
            }
            else if (score >= 100)
            {
                //设置分数文本
                scoreObj.text = "0" + score.ToString();
            }
            else if (score >= 10)
            {
                //设置分数文本
                scoreObj.text = "00" + score.ToString();
            }
            else
            {
                //设置分数文本
                scoreObj.text = "000" + score.ToString();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else
        {
            this.score = (int)stream.ReceiveNext();
        }
    }
}
