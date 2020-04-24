using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class DestroyWeapon : MonoBehaviour
    {
        //销毁实例延迟时间
        public float destroyTime = 0.5f;

        // Start is called before the first frame update
        void Start()
        {
            // 武器攻击画面消失
            GameObject.Destroy(gameObject, destroyTime);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
