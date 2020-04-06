using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRefresh : MonoBehaviour
{
    public int weaponstatus = 0;//刷新点的状态，0表示没有武器
    public float refreshTime = 10.0f;//刷新间隔
    public float startTime = 5.0f;//游戏开始的第一次刷新

    public GameObject weaponObject; //挂 Weaponrefresh\weapon
    Transform[] weapons;

    private void OnTriggerEnter(Collider col)//角色与刷新点碰撞
    {
        if (weaponstatus != 0)
        {
                var target = col.GetComponent<PlayerCharacter>();
                if(target)
                {
                    var temp = target.TakeWeapon(weaponstatus);
                    if(!temp)//捡起武器
                    {
                        weapons[weaponstatus].gameObject.SetActive(false);
                        weaponstatus = 0;
                        Invoke("NewWeapon", refreshTime);
                    }
                }
        }
    }

    private void OnTriggerStay(Collider col)//角色在刷新点等待
    {
        if (weaponstatus != 0)
        {
                var target = col.GetComponent<PlayerCharacter>();
                if (target)
                {
                    var temp = target.TakeWeapon(weaponstatus);
                    if (!temp)
                    {
                        weapons[weaponstatus].gameObject.SetActive(false);
                        weaponstatus = 0;
                        Invoke("NewWeapon", refreshTime);
                    }
                }
        }
    }

    private void NewWeapon()//刷新出武器
    {
        var relivex = Random.Range(-10.0f, 10.0f);
        var relivez = Random.Range(-10.0f, 10.0f);
        transform.position = new Vector3(relivex, 1, relivez);//每次刷新都移动
        int t = Random.Range(1, 5);
        weaponstatus = t;
        weapons[weaponstatus].gameObject.SetActive(true);
    }


    // Start is called before the first frame update
    void Start()
    {
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
}
