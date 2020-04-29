using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
  PlayerCharacter character;
  // Start is called before the first frame update
  private void Start() {
    //人物角色实例
    character = GetComponent<PlayerCharacter>();
  }

  // Update is called once per frame
  void Update() {
    if(Input.GetKeyDown(KeyCode.J)) {
      //按 J 键攻击
      character.Attack();
    }
  }

}
