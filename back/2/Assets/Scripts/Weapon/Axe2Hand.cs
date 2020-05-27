using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe2Hand : Weapon
{
    private bool isfire = false;
    private float rotaeangle=-360.0f*3.0f;
    public override void Fire()
    {
        isfire = true;
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            transform.RotateAround(initPos, Vector3.up, rotaeangle/ destroyTime * Time.deltaTime);
        }
    }
}
