using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    private bool isfire = false;
    private bool attackBack = false;
    public float AttackDistance = 1.5f;
    public override void  Start()
    {
        base.Start();
        destroyTime = 0.5f;
    }
    public override void Fire()
    {
        isfire = true;
        attackBack = false;
        Invoke("SetBack", 0.3f * destroyTime);
        DestroyWithDelay();
    }
    private void Update()
    {
        if (isfire)
        {
            if (!attackBack)
            {
                transform.position += AttackDistance / destroyTime * Time.deltaTime * initForward;
            }
            else
            {
                transform.position -= 0.5f * AttackDistance / destroyTime * Time.deltaTime * initForward;
            }
        }
    }

    private void SetBack()
    {
        attackBack = true;
    }
}