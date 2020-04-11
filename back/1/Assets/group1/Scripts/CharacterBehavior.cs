using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehavior : MonoBehaviour
{

    Renderer[] character, namebar;      //角色，名字渲染
    CanvasRenderer[] healthbar;         //血条渲染
    public Joystick touch;              //虚拟摇杆
    public float speed = 10;            //速度
    private Animator ani;                //动画

    void Start()
    {
        ani = GetComponentInChildren<Animator>();       //获取动画

        Shader shader = Shader.Find("Transparent/Diffuse");
        character = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();     //获取角色
        namebar = this.transform.GetChild(6).GetComponentsInChildren<Renderer>();       //获取名字
        healthbar = this.transform.GetChild(5).GetComponentsInChildren<CanvasRenderer>();       //获取血条
        /*foreach (var render in character)
        {
            render.material.shader = shader;        //阴影
        }*/
        /////////debug By GaoYan
        ////动态修改shader导致贴图失效，改为静态修改

        SetTransparent(1f);                         //设为可见
    }

    //Author: wmj

    public void SetTransparent(float a)             //设置可见度
    {
        foreach (var render in character)           //设置角色可见度
        {
            render.material.color = new Color(1f, 1f, 1f, a);
        }
        foreach (var render in healthbar)           //设置血条可见度
        {
            render.SetAlpha(a);
        }
        foreach (var render in namebar)             //设置名字可见度
        {
            render.enabled = a != 0;
        }
    }
}
    //Author: Via Cytus

    /*void Update()
    {
        Vector3 direction = new Vector3(touch.Movement.x, 0, touch.Movement.y);     //获取摇杆向量
        CharacterController controller = GetComponent<CharacterController>();       //获取角色控制插件
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);      //旋转
            transform.Translate(Vector3.forward * Time.deltaTime * speed);              //移动
            ani.SetFloat("Speed", speed);                       //设置动画
            controller.SimpleMove(direction * speed);           //角色控制插件控制移动
        }
        else
        {
            ani.SetFloat("Speed", 0);       //停止动画
        }
    }
}*/
