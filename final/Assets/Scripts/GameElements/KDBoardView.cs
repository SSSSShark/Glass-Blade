using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KDBoardView : MonoBehaviourPunCallbacks, IPunObservable
{
    public TextMeshProUGUI kdboard;
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

    public void CallUpdateBoard(string src, string dst, string weaponname, int srcteam, int dstteam)
    {
        int weaponkind = 0;
        if (weaponname.Contains("Axe"))
        {
            weaponkind = 1;
        }
        else if (weaponname.Contains("Dagger"))
        {
            weaponkind = 2;
        }
        else if (weaponname.Contains("Sword"))
        {
            weaponkind = 0;
        }
        photonView.RPC("UpdateBoard", RpcTarget.MasterClient, src, dst, weaponkind, srcteam, dstteam);
    }

    [PunRPC]
    public void UpdateBoard(string src, string dst, int weaponkind, int srcteam, int dstteam)
    {
        string srccolor, dstcolor;

        if (srcteam == 0)
        {
            srccolor = "#FF0000";
        }
        else
        {
            srccolor = "#0000FF";
        }
        if (dstteam == 0)
        {
            dstcolor = "#FF0000";
        }
        else
        {
            dstcolor = "#0000FF";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            //string kdinfo = src + " ︻$▅▆▇◤ " + target;
            string kdinfo = string.Format("<color={0}>" + src + "</color>" +
                            "  <sprite={1}>  " +
                            "<color={2}>" + dst + "</color>", srccolor, weaponkind, dstcolor);
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
