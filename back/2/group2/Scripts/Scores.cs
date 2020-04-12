using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Scores : MonoBehaviour, IPunObservable
{

    [SerializeField]
    public int score;            
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
        this.score += 1;
    }

    public void SendScoreInfo()
    {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("GetScoreInfo", RpcTarget.MasterClient);
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ScoreObj.text = ("0" + score.ToString()).Substring(0, 2);
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
