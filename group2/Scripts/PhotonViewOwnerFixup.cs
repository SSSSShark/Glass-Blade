using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PhotonViewOwnerFixup : MonoBehaviour, IPunObservable
{

    #region public field
    public int ownernum = 0;

    
    #endregion
    #region IPunObservable implementation


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(ownernum);
        }
        else
        {
            ownernum = (int)stream.ReceiveNext();
        }
    }

    #endregion
    private void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        ownernum = GetComponent<PhotonView>().OwnerActorNr;
    }
}
