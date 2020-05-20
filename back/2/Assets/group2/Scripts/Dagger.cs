using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : Weapon
{
    private bool isfire = false;
    public float daggerAttackDistance = 60.0f;
    public override void Fire()
    {
        isfire = true;
        DestroyWithDelay();
        gameObject.GetComponent<Rigidbody>().velocity = daggerAttackDistance/destroyTime * initForward;
    }
    private void Update()
    { 
    }
}
