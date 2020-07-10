using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword2Hand : Weapon
{
    private bool isfire = false;
    public float rotateangle = 120.0f;
    public override void Start()
    {
        base.Start();
        destroyTime = 0.3f;
        transform.RotateAround(initPos, Vector3.up, -rotateangle/2);
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
            transform.RotateAround(initPos, Vector3.up, (rotateangle / destroyTime) * Time.deltaTime);
        }
    }
}
