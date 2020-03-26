using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonTextView : MonoBehaviour, IPunObservable
{

    #region private field
    Text mytext;
    Toggle mytoggle;
    #endregion
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(mytext.text);
            stream.SendNext(mytoggle.isOn);
        }
        else
        {
            mytext.text=(string)stream.ReceiveNext();
            mytoggle.isOn=(bool)stream.ReceiveNext();
        }
    }

    #endregion
    #region MonoBehaviour CallBack
    private void Start()
    {
        mytext = gameObject.GetComponent<Text>();
        mytoggle = gameObject.transform.Find("Toggle").GetComponent<Toggle>();
    }
    #endregion 
}
