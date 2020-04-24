using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class Weapon1Colider : MonoBehaviour
    {
        private void OnTriggerEnter(Collider col)//角色与刷新点碰撞
        {
            var target = col.GetComponent<PlayerCharacter>();
            var par = this.GetComponentInParent<PlayerCharacter>();
            if (target)
            {
                if (target != this.GetComponentInParent<PlayerCharacter>())
                {
                    target.TakeDamage();
                    target.deathTime++;
                    par.killTime++;
                }
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
