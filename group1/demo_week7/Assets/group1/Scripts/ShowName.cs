//Author: David Wang


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowName : MonoBehaviour
{
    private TextMesh PlayerName;        //玩家名字组件
    
    void Start()
    {
        this.PlayerName = this.GetComponentInParent<TextMesh>();        //获取组件
    }

    
    void Update()
    {
        Vector3 cameraDirection = Camera.main.transform.forward;        //获取相机位置向量
        cameraDirection.y = 0f;             //清零y轴
        this.PlayerName.transform.rotation = Quaternion.LookRotation(cameraDirection);          //设置名字朝向
    }
}
