using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

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

    // 造成伤害的触发器
    private void OnTriggerEnter(Collider other)
    {
        var colliders = Physics.OverlapSphere(transform.position + this.transform.forward, damageRadius, damageMask);
        foreach (var collider in colliders)
        {
            var target = collider.GetComponent<PlayerCharacter>();
            if (target)
            {
                if (target.photonView.Controller == weaponOwner)
                {
                    Debug.Log(target.photonView.Controller.NickName + "isyourself");
                    continue;
                }
                if (!friendlyFire &&
                    (TeamController.Team)target.photonView.Controller.CustomProperties["team"] ==
                    (TeamController.Team)weaponOwner.CustomProperties["team"])
                {
                    Debug.Log("friendly fire is not allowed");
                    continue;
                }
            }
            Debug.Log("collider detected");
            target.InformDamage(target.photonView.Controller, target.photonView ,photonviewOwner);
        }
    }
}

