using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.GlassBlade.Group2
{
    public class CharacterBehavior : MonoBehaviour
    {
        /***********************************************/
        // 重力大小,影响下落速度
        public float gravity = 9;
        // 跳跃垂直速度
        public float jumpSpeed = 7f;
        // 角色垂直速度
        private float yspeed = 0f;
        /***********************************************/

        //队伍
        //public int team;
        //角色，名字渲染
        //private Renderer[] character, namebar;
        //血条渲染
        //private CanvasRenderer[] healthbar;
        //虚拟摇杆
        public Joystick touch;
        //速度
        public float speed = 10;
        //动画
        private Animator ani;

        void Start()
        {
            //获取动画
            ani = GetComponentInChildren<Animator>();
            Shader shader = Shader.Find("Transparent/Diffuse");
            //获取角色
            //character = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();
            //获取名字
            //namebar = this.transform.GetChild(6).GetComponentsInChildren<Renderer>();
            //获取血条
            //healthbar = this.transform.GetChild(5).GetComponentsInChildren<CanvasRenderer>();  
           // foreach (var render in character)
            //{
                //阴影
                //render.material.shader = shader;
            //}
            //设为可见
           // SetTransparent(1f);
        }

        //Author: wmj
        //设置可见度
      /*  public void SetTransparent(float a)
        {
            //设置角色可见度
            foreach (var render in character)
            {
                render.material.color = new Color(1f, 1f, 1f, a);
            }
            //设置血条可见度
            foreach (var render in healthbar)
            {
                render.SetAlpha(a);
            }
            //设置名字可见度
            foreach (var render in namebar)      
            {
                render.enabled = a != 0;
            }
        }
        */
        //Author: Via Cytus
        void Update()
        {
            //获取摇杆向量
            Vector3 direction = new Vector3(touch.Movement.x, 0, touch.Movement.y);
            //获取角色控制插件
            CharacterController controller = GetComponent<CharacterController>();
            PlayerCharacter playerCharacter = GetComponent<PlayerCharacter>();
            if (direction != Vector3.zero && playerCharacter.isAlive && !playerCharacter.attacking)
            {
                //旋转
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
                //移动
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
                //设置动画
                ani.SetFloat("Speed", speed);   
                //角色控制插件控制移动
                controller.SimpleMove(direction * speed);   
            }
            else
            {
                //停止动画
                ani.SetFloat("Speed", 0);
            }
            /*******************************************************************/
            //当在空中时, 不能跳跃
            if (!controller.isGrounded)
            {
                //Debug.Log("g" + gravity);
                yspeed -= gravity * Time.deltaTime;
                controller.Move(new Vector3(0, yspeed * Time.deltaTime, 0));
            }
            //已经在地面上,可以跳跃了
            else
            {
                //按 K 键跳跃
                if (Input.GetKeyDown(KeyCode.K))
                {
                    yspeed = jumpSpeed;
                    controller.Move(new Vector3(0, yspeed * Time.deltaTime, 0));
                }
                //或就待在地面
                else
                {
                    yspeed = 0;
                }
            }
            /*******************************************************************/
        }
    }
}
