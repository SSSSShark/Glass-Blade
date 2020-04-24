using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class WeaponRefresh : MonoBehaviour
    {
        //刷新点的状态，0表示没有武器
        public int weaponStatus = 0;

        //刷新间隔
        public float refreshTime = 10.0f;

        //游戏开始的第一次刷新时间
        public float startTime = 5.0f;

        //挂 Weaponrefresh\weapon
        public GameObject weaponObject; 

        Transform[] weapons;

        /// <summary>
        /// 角色与刷新点碰撞
        /// </summary>
        private void OnTriggerEnter(Collider col)
        {
            if (weaponStatus != 0)
            {
                var target = col.GetComponent<PlayerCharacter>();
                if (target)
                {
                    var temp = target.TakeWeapon(weaponStatus);
                    //捡起武器
                    if (!temp)
                    {
                        weapons[weaponStatus].gameObject.SetActive(false);
                        weaponStatus = 0;
                        Invoke("NewWeapon", refreshTime);
                    }
                }
            }
        }

        /// <summary>
        /// 角色在刷新点等待
        /// </summary>
        private void OnTriggerStay(Collider col)
        {
            if (weaponStatus != 0)
            {
                var target = col.GetComponent<PlayerCharacter>();
                if (target)
                {
                    var temp = target.TakeWeapon(weaponStatus);
                    if (!temp)
                    {
                        weapons[weaponStatus].gameObject.SetActive(false);
                        weaponStatus = 0;
                        Invoke("NewWeapon", refreshTime);
                    }
                }
            }
        }

        /// <summary>
        /// 刷新出武器,武器编号随机
        /// </summary>
        private void NewWeapon()
        {
            int t = Random.Range(1, 5);
            weaponStatus = t;
            weapons[weaponStatus].gameObject.SetActive(true);
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
}
