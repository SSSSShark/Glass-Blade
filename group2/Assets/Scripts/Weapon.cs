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

            if (target && target.isAlive && target != this.GetComponentInParent<PlayerCharacter>()) 
            {
                target.TakeDamage();
                target.deathTime++;
                this.GetComponentInParent<PlayerCharacter>().killTime++;
            }
        }
    }
}