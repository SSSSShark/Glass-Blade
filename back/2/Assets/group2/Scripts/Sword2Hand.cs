using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword2Hand : Weapon
{
    private bool isfire = false;
    public override void Fire()
    {
        isfire = true;
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            transform.RotateAround(initPos, Vector3.up, (2.5f / destroyTime) * Time.deltaTime);
        }
    }
}
