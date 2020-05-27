// ===----------------------------------------------------------------=====
// Weapon refresh:
// implement weapon refreshing in specific place
// ===----------------------------------------------------------------=====
//
// The master client decides the time to refresh the weapon and other client
// sync the weaponstatus and display the weapon accordingly.
// If one client (including master) picked up the weapon, we need to inform
// the master client and the master client decides whether the client could
// have the weapon.
//
// ===-----------------------------------------------------------------====


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class WeaponRefresh : MonoBehaviourPunCallbacks
{
  public int weaponstatus = 1;//刷新点的状态，0表示没有武器
  public float refreshTime = 10.0f;//刷新间隔
  public float startTime = 5.0f;//游戏开始的第一次刷新

  public GameObject weaponObject; //挂 Weaponrefresh\weapon

  public GameObject player; // the linked player
  Transform[] weapons;

  /// <summary>
  /// try to pick a weapon from player perspective. The player ensures that
  /// the weapon can be picked up and calls this function. The function checks if 
  /// we are the master client, and if so, we immediately pick up the weapon, and call
  /// back, let the player pick the weapon. If we are not the master client, a RPC request
  /// is sent to master client to request access
  /// </summary>
  public void tryPickWeapon(int requestedWeapon)
  {
    if (PhotonNetwork.IsMasterClient)
    {
      // we are the master client
      Debug.Log("[WeaponRefresh:tryPickWeapon()] Master client try to pick weapon " + requestedWeapon);

      if (weaponstatus != 0 && requestedWeapon == weaponstatus)
      {
        weaponstatus = 0;
        photonView.RPC("UpdateWeaponStatus", RpcTarget.All, requestedWeapon, false);
        Invoke("NewWeapon", refreshTime);
        player.GetComponent<PlayerCharacter>().TakeWeapon(requestedWeapon);
      }
    }
    else
    {
      // we are other clients
      Debug.Log("[WeaponRefresh:tryPickWeapon()] other client try to pick weapon " + requestedWeapon);

      // send RPC to the master client
      if (requestedWeapon != 0)
      {
        photonView.RPC("GetWeapon", RpcTarget.MasterClient, requestedWeapon);
      }
    }
  }

  private void NewWeapon()//刷新出武器
  {
    // only the master client decides this
    if (PhotonNetwork.IsMasterClient)
    {
      Debug.Log("[WeaponRefresh:NewWeapon()] Master client: new weapon");
      int t = Random.Range(1, 5);
      Debug.Log("[WeaponRefresh:NewWeapon()] RPC start");
      photonView.RPC("UpdateWeaponStatus", RpcTarget.All, t, true);
      weaponstatus = t;
    }
  }

  // Start is called before the first frame update
  void Start()
  {
    Debug.Log("[WeaponRefresh:Start()] Start weapon refresh");
    weapons = weaponObject.GetComponentsInChildren<Transform>();
    weapons[1].gameObject.SetActive(false);
    weapons[2].gameObject.SetActive(false);
    weapons[3].gameObject.SetActive(false);
    weapons[4].gameObject.SetActive(false);
    Invoke("NewWeapon", startTime);
  }

  // Update is called once per frame
  void Update()
  {

  }

  [PunRPC]
  /// <summary>
  /// Set the activeness of weapons accordingly
  /// </summary>
  void UpdateWeaponStatus(int requestWeaponStatus, bool activeStatus)
  {
    // TODO: we need a way to get number of available weapons
    if (requestWeaponStatus > 0 && requestWeaponStatus <= 4)
    {
      weapons[requestWeaponStatus].gameObject.SetActive(activeStatus);
    }

    if (activeStatus == true)
    {
      weaponstatus = requestWeaponStatus;
    }
    else
    {
      weaponstatus = 0;
    }
  }

  [PunRPC]
  /// <summary>
  /// master only: check if we can assign the weapon to the client
  /// This can only be called when a client CAN take a weapon without
  /// any difficulty, i.e, it already has a weapon
  /// FIXME: maybe a timestamp is needed
  /// </summary>
  void GetWeapon(int requestWeaponStatus, PhotonMessageInfo msgInfo)
  {
    if (PhotonNetwork.IsMasterClient)
    {
      if (weaponstatus == requestWeaponStatus)
      {
        // it is ok for the client to take the weapon, and naturally the client
        // will take it. So we can set the status to false NOW. And then we inform
        // the client to take the weapon
        photonView.RPC("UpdateWeaponStatus", RpcTarget.All, requestWeaponStatus, false);
        weaponstatus = 0;

        // call back
        photonView.RPC("PickWeaponCallBack", msgInfo.Sender, requestWeaponStatus);

        // refresh
        Invoke("NewWeapon", refreshTime);
      }
    }
  }

  [PunRPC]
  /// <summary>
  /// A call back function called when a player successfully picked up the weapon
  /// </summary>
  void PickWeaponCallBack(int requestedWeapon)
  {
    Debug.Log("[WeaponRefresh:PickWeaponCallBack()] Player Take Weapon " + requestedWeapon);

    if (player == null)
    {
      Debug.LogError("[WeaponRefresh:PickWeaponCallBack()] BUG: Player NULL");
      return;
    }
    PlayerCharacter target = player.GetComponent<PlayerCharacter>();

    target.TakeWeapon(requestedWeapon);
  }
}
