using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public abstract class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    public virtual  void Start()
    {
        photonviewOwner = this.GetComponent<PhotonView>();
        friendlyFire = GameObject.Find("SettingStore").GetComponent<SettingStore>().friendlyFire;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public float damageRadius; //伤害判定半径
    public LayerMask damageMask; //设置能够判定伤害的层面
    public Player weaponOwner;  //武器持有者
    public PhotonView photonviewOwner;
    public bool friendlyFire = false;  //允许友军伤害
    public PlayerCharacter Pc;
    public float destroyTime = 2.0f;
    public Vector3 initPos,initForward;

    /// <summary>
    /// If the weapon hit a player, the player should take damage.
    /// We are going to call the player itself's take damage function.
    /// </summary>
    /// <param name="other">The collider, useless in this case</param>
    private void OnTriggerEnter(Collider other)
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            var target = other.GetComponent<PlayerCharacter>();
            if (target)
            {
                if (target.photonView.Controller == weaponOwner)
                {
                    Debug.Log("[Weapon:OnTriggerEnter()] " + target.photonView.Controller.NickName + "(yourself) triggered, ignored");
                    return;
                }
                if (!friendlyFire &&
                    (TeamController.Team)target.photonView.Controller.CustomProperties["team"] ==
                    (TeamController.Team)weaponOwner.CustomProperties["team"])
                {
                    Debug.Log("[Weapon:OnTriggerEnter()] Friend fire is not allowed");
                    return;
                }
                Debug.Log("[Weapon:OnTriggerEnter()] Player " + target.photonView.Controller.NickName + " Takes damage.");
                Pc.killTime++;
                target.CallTakeDamage(target.photonView.Owner, photonviewOwner);
            }
        }
    }
    public  abstract void Fire();
    protected void DestroyWithDelay()
    {
        Invoke("PhotonDestroy",destroyTime);
    }
    private void PhotonDestroy()
    {
        PhotonNetwork.Destroy(gameObject);
    }

  
}

