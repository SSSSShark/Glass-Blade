using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDefault : Weapon
{
    private bool isfire = false;
    private int rotateangle=110;
    public override void Start()
    {
        base.Start();
        destroyTime = 0.5f;
        transform.RotateAround(gameObject.transform.position, initForward, 90);
        transform.RotateAround(gameObject.transform.position, Vector3.Cross(Vector3.up, initForward), -90);
        transform.position += 0.5f * Vector3.up;
        // Debug.Log("here");
    }
    public override void Fire()
    {
        isfire = true;
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            transform.RotateAround(gameObject.transform.position, Vector3.Cross(Vector3.up,initForward), rotateangle/ destroyTime * Time.deltaTime);
        }
    }
}
