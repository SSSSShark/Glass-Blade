using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon1Destroy : MonoBehaviour
{
    public GameObject o;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.Destroy(gameObject, 1.0f);
        o = transform.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(o.transform.position, Vector3.up, 360 * Time.deltaTime);
    }
}
