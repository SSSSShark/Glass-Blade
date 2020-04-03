using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform PlayerTrans;

    public float Camera_Height;
    public float Camera_Distance;
    public int team;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        this.PlayerTrans = player.transform;        // 获取Player位置
        team = player.GetComponent<PlayerCharacter>().team;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = new Vector3 (this.PlayerTrans.position.x, this.PlayerTrans.position.y + this.Camera_Height, this.PlayerTrans.position.z - this.Camera_Distance);       // 跟随Player
        this.transform.LookAt (this.PlayerTrans);                                                                 // 对准Player
    }
}
