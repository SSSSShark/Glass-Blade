using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class SwordTwoHandedDestroy : MonoBehaviour
    {
        private GameObject o;
        public float destroyTime;
        // Start is called before the first frame update
        void Start()
        {
            GameObject.Destroy(gameObject, destroyTime);
            o = transform.parent.gameObject;
            transform.RotateAround(o.transform.position, Vector3.up, 300);
        }

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(o.transform.position, Vector3.up, (130f / destroyTime) * Time.deltaTime);
        }
    }
}
