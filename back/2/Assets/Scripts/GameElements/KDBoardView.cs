using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KDBoardView : MonoBehaviourPunCallbacks, IPunObservable
{
    public Text kdboard;
    private Queue<string> kdqueue = new Queue<string>();


    // 初始化赋值
    void Start()
    {

    }

    // 
    void Update()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            kdboard.text = "";
            foreach (var item in kdqueue)
            {
                kdboard.text += item + "\n";
            }
        }
    }

    public void CallUpdateBoard(string src, string target)
    {
        photonView.RPC("UpdateBoard", RpcTarget.MasterClient, src, target);
    }

    [PunRPC]
    public void UpdateBoard(string src, string target)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            string kdinfo = src + " ︻$▅▆▇◤ " + target;
            kdqueue.Enqueue(kdinfo);
            if (kdqueue.Count > 5)
            {
                kdqueue.Dequeue();
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(kdboard.text);
        }
        else
        {
            this.kdboard.text = (string)stream.ReceiveNext();

        }
    }
}
