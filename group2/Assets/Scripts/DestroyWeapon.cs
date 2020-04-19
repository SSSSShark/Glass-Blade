using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeapon : MonoBehaviour
{
  public float destroyTime = 1.0f;
    // Start is called before the first frame update
  void Start()
  {
    // 武器攻击画面消失
    GameObject.Destroy(gameObject, destroyTime);
  }

    // Update is called once per frame
  void Update()
  {
        
  }
}
