using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonTextView : MonoBehaviour, IPunObservable
{

    #region private field
    Text mytext;
    #endregion
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mytext.text);
        }
        else
        {
            mytext.text=(string)stream.ReceiveNext();
        }
    }

    #endregion
    #region MonoBehaviour CallBack
    private void Start()
    {
        mytext = gameObject.GetComponent<Text>();
    }
    #endregion 
}
