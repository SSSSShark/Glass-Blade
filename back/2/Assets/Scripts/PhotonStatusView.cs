using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PhotonStatusView : MonoBehaviour, IPunObservable
{

    #region private field
    GameObject readyObj;
    private Dictionary<string, Texture> texturemap;
    #endregion
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(readyObj.GetComponent<RawImage>().enabled);
            stream.SendNext(texturemap.FirstOrDefault(q => q.Value == readyObj.GetComponent<RawImage>().texture).Key);
        }
        else
        {
            readyObj.GetComponent<RawImage>().enabled = (bool)stream.ReceiveNext();
            readyObj.GetComponent<RawImage>().texture = texturemap[(string)stream.ReceiveNext()];
        }
    }

    #endregion
    #region MonoBehaviour CallBack
    private void Start()
    {
        readyObj = gameObject;
        GameObject teamController = GameObject.Find("TeamController");
        texturemap = new Dictionary<string, Texture>{
        { "ready", teamController.GetComponent<TeamController>().readyIcon},{ "host", teamController.GetComponent<TeamController>().hostIcon}};
    }
    #endregion 
}
