//Author: David Wang


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform PlayerTrans;      //

    public float Camera_Height=25;         //相机高度
    public float Camera_Distance=25;       //相机距离
    public int team;                    //队伍


    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");     //获取玩家角色
        this.PlayerTrans = player.transform;        // 获取Player位置
        team = player.GetComponent<CharacterBehavior>().team;                 //获取队伍
    }


    void Update()
    {
        this.transform.position = new Vector3 (this.PlayerTrans.position.x, this.PlayerTrans.position.y + this.Camera_Height, this.PlayerTrans.position.z - this.Camera_Distance);       // 跟随Player
        this.transform.LookAt (this.PlayerTrans);                                                       // 对准Player
    }
}
