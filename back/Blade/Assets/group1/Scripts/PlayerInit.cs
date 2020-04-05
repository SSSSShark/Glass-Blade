using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//用来初始化静态的用户定义的角色差异
//by:GaoYan
public class PlayerInit : MonoBehaviour, IPunInstantiateMagicCallback
{
    [Tooltip("Name Color of different teams")]
    [SerializeField]
    private Color []Teamcolor;
   public  void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        //Debug.Log("OnInstantiate"+ Teamcolor[(int)(info.Sender.CustomProperties["team"])]);
        GetComponentInChildren<TextMesh>().text = info.Sender.NickName;
       GetComponentInChildren<TextMesh>().color = Teamcolor[(int)(info.Sender.CustomProperties["team"])];
    }
}
