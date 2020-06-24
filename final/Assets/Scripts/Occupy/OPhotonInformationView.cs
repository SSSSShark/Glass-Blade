using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//author :GaoYan
public class OPhotonInformationView : MonoBehaviour, IPunObservable
{
    private Text scoreA;
    private Text scoreB;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(scoreA.text);
            stream.SendNext(scoreB.text);
        }
        else
        {
            scoreA.text = (string)stream.ReceiveNext();
            scoreB.text = (string)stream.ReceiveNext();

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        scoreA = this.transform.GetChild(0).gameObject.GetComponent<Text>();
        scoreB = this.transform.GetChild(1).gameObject.GetComponent<Text>();
    }
}
