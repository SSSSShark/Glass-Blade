using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class WeaponMove : MonoBehaviour
    {
        float f = 0.025f;
        float ty = 1.0f;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition = new Vector3(0, ty, 0);
            ty = ty + f;
            if (ty >= 1.25 || ty <= 0.75)
            {
                f = -f;
            }
        }
    }
}