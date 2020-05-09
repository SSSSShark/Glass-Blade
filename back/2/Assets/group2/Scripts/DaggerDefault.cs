using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaggerDefault : Weapon
{
    private bool isfire = false;
    private int speed = -25;
    public override void Fire()
    {
        
        isfire = true;
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            transform.RotateAround(initPos, Vector3.up, speed / destroyTime * Time.deltaTime);
            speed = speed + 40;
            if (speed > 300)
            {
                speed = 300;
            }
            else if (speed > -150 && speed < 0)
            {
                speed = 400;
            }
        }
    }
}
