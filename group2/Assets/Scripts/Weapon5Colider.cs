using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class Weapon5Colider : MonoBehaviour
    {
        public float enableTime = 3.0f;

        //拥有者
        private GameObject haver;

        //地雷是否已经准备完成
        private int enable = 0;

        public void SetHaver(GameObject x)
        {
            haver = x;
        }

        private void ToEnable()
        {
            enable = 1;
        }

        private void OnTriggerEnter(Collider col)
        {
            var target = col.GetComponent<PlayerCharacter>();
            var par = haver.GetComponent<PlayerCharacter>();

            //玩家非空、活着、未处于刚复活的保护状态、为处于无敌状态，则攻击有效
            if (target && target.isAlive && !target.isProtected && !target.invincible && enable == 1)
            {
                target.TakeDamage();
                target.deathTime++;
                par.killTime++;
                Invoke("Destroyself", 0.5f);
            }

        }

        private void Destroyself()
        {
            Destroy(gameObject);
        }

        // Start is called before the first frame update
        void Start()
        {
            Invoke("ToEnable", enableTime);
        }
    }
}