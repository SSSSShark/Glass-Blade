using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private bool isfire = false;
    public float daggerAttackDistance = 2.0f;
    public override void Fire()
    {
        isfire = true;
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            transform.position += daggerAttackDistance / destroyTime * Time.deltaTime *  initForward ;
        }
    }
}