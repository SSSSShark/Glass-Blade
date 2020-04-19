using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Glassblade.Group1
{
    public class NewBehaviourScript : MonoBehaviour
    {
        private NDataStore p;

        void Start()
        {
            p = GameObject.FindWithTag("Finish").GetComponentInChildren<NDataStore>();

        }


        void Update()
        {

        }
    }
}