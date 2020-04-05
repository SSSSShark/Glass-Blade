using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//author :GaoYan
public class PhotonInformationView : MonoBehaviour, IPunObservable
{
    private Text timetext;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(timetext.text);
        }
        else
        {
            timetext.text =(string) stream.ReceiveNext();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        timetext = this.transform.GetChild(0).gameObject.GetComponent<Text>();
    }
}
