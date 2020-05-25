using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class Weapon1Colider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)
        {
            var target = col.GetComponent<PlayerCharacter>();
            var par = this.GetComponentInParent<PlayerCharacter>();

            //玩家非空、活着、未处于刚复活的保护状态、为处于无敌状态、不是攻击者本人，则攻击有效
            if (target && target.isAlive && !target.isProtected && !target.invincible && target != this.GetComponentInParent<PlayerCharacter>())
            {
               target.TakeDamage();
               target.deathTime++;
               par.killTime++;
            }
        }
        public void SetAttackTime(float x)
        {
            Invoke("Destroyself", x);
        }
        private void Destroyself()
        {
            Destroy(gameObject);
        }
    }
}
