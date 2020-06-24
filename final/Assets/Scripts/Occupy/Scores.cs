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
        Debug.Log("[Scores:GetScoreInfo()] score called");
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
            scoreObj.text = score.ToString();
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
