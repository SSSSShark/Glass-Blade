using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon1colider : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)//角色与刷新点碰撞
    {
        var target = col.GetComponent<PlayerCharacter>();
        if (target)
        {
            if (target != this.GetComponentInParent<PlayerCharacter>())
            {
                target.TakeDamage();
            }
        }
    }
    public void setAttackTime(float x)
    {
        Invoke("destroyself", x);
    }
    void destroyself()
    {
        Destroy(gameObject);
    }
}
