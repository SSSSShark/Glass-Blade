using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private bool isfire = false;
    public float AttackDistance = 1.5f;
    public override void  Start()
    {
        base.Start();
        destroyTime = 0.5f;
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
            transform.position += AttackDistance / destroyTime * Time.deltaTime *  initForward ;
        }
    }
}