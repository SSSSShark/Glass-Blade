﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Weapon : MonoBehaviour
{
  // Start is called before the first frame update
  void Start() {

  }

  // Update is called once per frame
  void Update() {

  }

  public float damageRadius; //伤害判定半径
  public LayerMask damageMask; //设置能够判定伤害的层面

  // 造成伤害的触发器
    private void OnTriggerEnter(Collider other) {
        var colliders = Physics.OverlapSphere(transform.position + this.transform.forward, damageRadius, damageMask);
        foreach(var collider in colliders) {
            var target = collider.GetComponent<PlayerCharacter>();
            if(target) {
                Debug.Log("collider detected");
                target.SendDamage(target.photonView.Controller);
            }
        }
    }
}