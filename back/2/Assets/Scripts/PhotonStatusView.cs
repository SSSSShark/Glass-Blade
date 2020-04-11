using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonStatusView : MonoBehaviour, IPunObservable
{

    #region private field
    GameObject readyObj;
    #endregion
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(readyObj.GetComponent<RawImage>().enabled);
        }
        else
        {
            readyObj.GetComponent<RawImage>().enabled = (bool)stream.ReceiveNext();
        }
    }

    #endregion
    #region MonoBehaviour CallBack
    private void Start()
    {
        readyObj = gameObject;
    }
    #endregion 
}
