using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class Weapon1Destroy : MonoBehaviour
    {
        private GameObject o;
        public float destroyTime = 0.5f;
        private int speed = -250;

        // Start is called before the first frame update
        void Start()
        {
            GameObject.Destroy(gameObject, destroyTime);
            o = transform.parent.gameObject;
        }

        // Update is called once per frame
        void Update()
        {
            transform.RotateAround(o.transform.position, Vector3.up, speed / destroyTime * Time.deltaTime);
            speed = speed + 40;
            if (speed > 1000)
            {
                speed = 1000;
            }
            else if (speed > -150 && speed < 0)
            {
                speed = 400;
            }
        }
    }
}
