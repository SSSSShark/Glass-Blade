using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Glassblade.Group1
{
    public class OCharacterBehavior : MonoBehaviour
    {
        //队伍
        public int team;
        //角色，名字渲染
        Renderer[] character, namebar;
        //血条渲染
        CanvasRenderer[] healthbar;
        //虚拟摇杆
        public OJoystick touch;
        //速度
        public float speed = 10;
        //动画
        private Animator ani;
        //击杀数
        [HideInInspector]
        public int killTime = 0;
        //死亡数
        [HideInInspector]
        public int deathTime = 0;
        //个人得分
        [HideInInspector]
        public int score = 0;

        void Start()
        {
            //获取动画
            ani = GetComponentInChildren<Animator>();

            Shader shader = Shader.Find("Transparent/Diffuse");
            //获取角色
            character = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();
            //获取名字
            namebar = this.transform.GetChild(6).GetComponentsInChildren<Renderer>();
            //获取血条
            healthbar = this.transform.GetChild(5).GetComponentsInChildren<CanvasRenderer>();
            foreach (var render in character)
            {
                //阴影
                render.material.shader = shader;
            }
            //设为可见
            SetTransparent(1f);
        }

        //Author: wmj
        //死亡事件
        public DeathEvent deathevent = new DeathEvent();
        /// <summary>
        /// //设置可见度
        /// </summary>
        /// <param name="a">[0,1]</param>
        public void SetTransparent(float a)
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
        /// <summary>
        /// 死亡时应invoke deathevent
        /// </summary>
        public void Dead()
        {
            deathevent.Invoke(this);
        }

        //Author: Via Cytus

        /// <summary>
        /// 角色移动动画
        /// </summary>
        void Update()
        {
            //获取摇杆向量
            Vector3 direction = new Vector3(touch.Movement.x, 0, touch.Movement.y);
            //获取角色控制插件
            CharacterController controller = GetComponent<CharacterController>();
            if (direction != Vector3.zero)
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
        }
    }
}