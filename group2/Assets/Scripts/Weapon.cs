using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{   
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

        /// <summary>
        /// 造成伤害的触发器
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            //获取触发触发器的玩家对象
            var target = other.GetComponent<PlayerCharacter>();

            //玩家非空、活着、未处于刚复活的保护状态、为处于无敌状态、不是攻击者本人，则攻击有效
            if (target && target.isAlive && !target.isProtected && !target.invincible && target != this.GetComponentInParent<PlayerCharacter>()) 
            {
                target.TakeDamage();
                target.deathTime++;
                this.GetComponentInParent<PlayerCharacter>().killTime++;
            }
        }
    }
}