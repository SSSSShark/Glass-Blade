using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;
using Photon.Pun;
using System;

public class TeamController : MonoBehaviour
{

    #region Public Fields
    public GameObject[] TeamAList;
    public GameObject[] TeamBList;
    public GameObject[] AddOrderList;
    #endregion
    // Start is called before the first frame update
    #region MonoBehaviour CallBack
    void Start()
    {
        Debug.Log("TeamController::start work");
        TeamAList = GameObject.FindGameObjectsWithTag("TeamA").OrderBy(a => -a.GetComponent<RectTransform>().position.y).ToArray();
        TeamBList = GameObject.FindGameObjectsWithTag("TeamB").OrderBy(a => -a.GetComponent<RectTransform>().position.y).ToArray();
        AddOrderList = new GameObject[TeamAList.Length + TeamBList.Length];
        int j = 0;

        for (int i = 0; i < (TeamAList.Length > TeamBList.Length ? TeamAList.Length : TeamBList.Length); i++)
        {
            if (i < TeamAList.Length)
            {
                AddOrderList[j] = TeamAList[i];
                j++;
            }
            if (i < TeamBList.Length)
            {
                AddOrderList[j] = TeamBList[i];
                j++;
            }
        }
        Invoke("synchronize", 0.3f);
    

    }

    // Update is called once per frame
    void Update()
    {
        
    }
#endregion

public void transtest()
{
    AddOrderList[Convert.ToInt32(PhotonNetwork.LocalPlayer.NickName)].GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
    AddOrderList[Convert.ToInt32(PhotonNetwork.LocalPlayer.NickName)].GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
    return;
}
 public void synchronize()
    {
        foreach (GameObject obj in AddOrderList)
        {
            if (obj.GetComponent<PhotonViewOwnerFixup>().ownernum != 0)
            {
                Debug.Log("transform");
                obj.GetComponent<PhotonView>().TransferOwnership(obj.GetComponent<PhotonViewOwnerFixup>().ownernum);
            }
        }


        foreach (GameObject obj in AddOrderList)
        {
            Debug.Log("before change" + obj.GetComponent<PhotonView>().OwnerActorNr);
            if (obj.GetComponent<PhotonView>().OwnerActorNr == 0)
            {
                obj.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.LocalPlayer);
                Debug.Log(obj.GetComponent<PhotonView>().OwnerActorNr);
                obj.GetComponent<Text>().text = PhotonNetwork.LocalPlayer.NickName;
                return;
            }
        }

    }
}

