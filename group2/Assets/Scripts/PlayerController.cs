using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.GlassBlade.Group2
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerCharacter character;

        // Start is called before the first frame update
        private void Start()
        {
            //人物角色实例
            character = GetComponent<PlayerCharacter>();
        }

        // Update is called once per frame
        void Update()
        {
            //按 J 键攻击
            if (Input.GetKeyDown(KeyCode.J))
            {
                character.Attack();
            }
        }
    }
}
