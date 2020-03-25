using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform PlayerTrans;

    public float Camera_Height;
    public float Camera_Distance;


    // Start is called before the first frame update
    void Start()
    {
        this.PlayerTrans = GameObject.FindGameObjectWithTag("Player").transform;        // 获取Player位置
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3 (this.PlayerTrans.position.x, this.PlayerTrans.position.y + this.Camera_Height, this.PlayerTrans.position.z - this.Camera_Distance);       // 跟随Player
        this.transform.LookAt (this.PlayerTrans);                                                                 // 对准Player
    }
}
