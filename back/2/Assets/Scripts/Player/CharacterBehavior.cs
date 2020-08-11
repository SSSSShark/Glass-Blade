using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.Glassblade.Group1;
using Photon.Pun;
using Photon.Realtime;
using System.Configuration;

public class CharacterBehavior : MonoBehaviourPun, IPunObservable
{
    // 角色，名字渲染
    Renderer[] character, namebar;
    // 血条渲染
    CanvasRenderer[] healthbar;
    // 虚拟摇杆
    public Joystick touch;
    // 速度
    public float speed = 10;
    [SerializeField]
    private int skillNumber = 2;
    // 动画
    private Animator ani;
    //隐身技能生效时间
    public float invisibleDuration = 8;
    //隐身技能剩余时间
    public float invisibleTime = 0;
    //突进技能时间
    private double marchForwardTime = 0;
    //突进距离
    [SerializeField]
    private float distance = 5;
    //加速技能持续时间
    [SerializeField]
    private float accelerateDuration = 5;
    //加速技能剩余时间
    private float accelerateTime = 0;
    //加速幅度
    [SerializeField]
    private float speedUp = 0.8F;
    //无敌技能持续时间
    [SerializeField]
    private float unbeatableDuration = 2;
    //无敌技能剩余时间
    private float unbeatableTime = 0;
    //是否可以控制转向
    private bool rotationEnable = true;
    //是否无敌
    public bool invincible = false;

    public bool inBush = false;

    public GameObject gamePlayer;

    public enum Team
    {
        TeamA,
        TeamB,
        unknown
    };

    void Awake()
    {
        skillNumber = (int)GameObject.Find("SettingStore").GetComponent<SettingStore>().myskill;
        // 获取动画
        ani = GetComponentInChildren<Animator>();

        Shader shader = Shader.Find("Transparent/Diffuse");
        // 获取角色
        character = this.transform.GetChild(0).GetComponentsInChildren<Renderer>();
        // 获取名字
        namebar = this.transform.GetChild(6).GetComponentsInChildren<Renderer>();
        // 获取血条
        healthbar = this.transform.GetChild(5).GetComponentsInChildren<CanvasRenderer>();
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

    /// <summary>
    /// Manually refresh transparency
    /// </summary>
    public void CallRefreshTransparent()
    {
        RefreshTransparent();
    }

    private int bushStatus;
    /// <summary>
    /// 根据当前状态（目前为草丛、隐身技能）刷新透明度
    /// </summary>
    private void RefreshTransparent()
    {
        // Debug.Log("[CharacterBehavior:RefreshTransparent()] Invisible time: " + invisibleTime);
        if (invisibleTime <= 0)
        { // 隐身技能未生效
            // Debug.Log("[CharacterBehavior:RefreshTransparent()] Invisibility not viable.");
            if (!inBush)
            {
                SetTransparent(1.0f);
            }
        }
        // 隐身技能生效
        else if ((Team)photonView.Owner.CustomProperties["team"] == (Team)PhotonNetwork.LocalPlayer.CustomProperties["team"])
        {//是队友
            Debug.Log("[CharacterBehavior:RefreshTransparent()] Set team mate " + photonView.Owner.NickName + " invisible.");
            Debug.Log("[CharacterBehavior:RefreshTransparent()] Local Player is " + PhotonNetwork.LocalPlayer.NickName);
            SetTransparent(0.5f);
        }
        else
        {//不是队友
            Debug.Log("[CharacterBehavior:RefreshTransparent()] Enermy " + photonView.Owner.NickName + " invisible.");
            SetTransparent(0f);
        }
    }

    /// <summary>
    /// Refresh invinsible according to the state
    /// </summary>
    private void RefreshInvinsible()
    {
        if (unbeatableTime > 0)
        {
            Debug.Log("INVINCIBLE");
            foreach (var render in character)
            {
                Debug.Log(render.material.GetFloat("Invincible enable"));
                render.material.SetFloat("_Invincible", 1);
            }
        }
        else
        {
            foreach (var render in character)
            {
                render.material.SetFloat("_Invincible", 0);
            }
        }
    }
    
    //Author: Via Cytus

    /// <summary>
    /// 角色移动动画
    /// </summary>
    void Update()
    {
        RefreshTransparent();
        RefreshInvinsible();

        // 技能更新失败？
        if (GameObject.Find("SettingStore") && skillNumber != (int)GameObject.Find("SettingStore").GetComponent<SettingStore>().myskill)
        {
            skillNumber = (int)GameObject.Find("SettingStore").GetComponent<SettingStore>().myskill;
        }

        //移动

        //获取角色控制插件
        CharacterController controller = GetComponent<CharacterController>();

        //Author: wmj
        //隐身

        if (this.invisibleTime > 0)
        {
            Debug.Log("[CharacterBahavior : Update()] Player " + this.photonView.Owner.NickName + " is in invisible state");
            //更新隐身时长

            this.invisibleTime -= Time.deltaTime;

            if (this.invisibleTime < 0)
            {
                this.invisibleTime = 0;
            }
        }
        else
        {
            // invisible time < 0
            this.invisibleTime = 0;
        }

        //Author Via Cytus
        //突进
        if (marchForwardTime > 0)
        {
            //突进
            transform.Translate(Vector3.forward * Time.deltaTime * distance / (float)0.2);
            //角色控制插件控制移动
            controller.SimpleMove(transform.forward * distance / (float)0.2);
            //技能持续时间减少
            marchForwardTime -= Time.deltaTime;
            if (marchForwardTime <= 0)
            {
                //允许转向
                rotationEnable = true;
            }
        }
        //加速
        if (accelerateTime > 0)
        {
            accelerateTime -= Time.deltaTime;
            //加速结束
            if (accelerateTime < 0)
            {
                gamePlayer.GetComponent<movegetgromjoystick>().speed =
                    gamePlayer.GetComponent<movegetgromjoystick>().speed / (1 + speedUp);
                accelerateTime = 0;
            }
        }
        //无敌
        if (unbeatableTime > 0)
        {
            unbeatableTime -= Time.deltaTime;
            //无敌
            invincible = true;
            //无敌结束
            if (unbeatableTime <= 0)
            {
                invincible = false;
            }
        }
    }

    //Author Via Cytus
    public void SkillTrigger()
    {
        if (photonView.IsMine)
        {
            switch (skillNumber)
            {
                case 1: MarchForward(); break;
                case 2: Accelerate(); break;
                case 3: Invisible(); break;
                case 4: Unbeatable(); break;
            }
        }
    }

    /// <summary>
    /// 突进技能
    /// </summary>
    private void MarchForward()
    {
        if (photonView.IsMine)
        {
            //添加音效
            AudioSource music;
            music = transform.GetComponentsInChildren<AudioSource>()[0];
            music.Play();
            marchForwardTime = 0.2;
            //禁止转向
            rotationEnable = false;
        }
    }

    /// <summary>
    /// 加速技能
    /// </summary>
    private void Accelerate()
    {
        //加速
        if (photonView.IsMine)
        {
            //添加音效
            AudioSource music;
            music = transform.GetComponentsInChildren<AudioSource>()[1];
            music.Play();
            float modiSpeed = gamePlayer.GetComponent<movegetgromjoystick>().speed;
            gamePlayer.GetComponent<movegetgromjoystick>().speed =
                gamePlayer.GetComponent<movegetgromjoystick>().speed * (1 + speedUp);
            accelerateTime = accelerateDuration;
        }
    }

    /// <summary>
    /// 使用隐身技能时调用此方法
    /// </summary>
    private void Invisible()
    {
        if (photonView.IsMine)
        {
            //添加音效
            AudioSource music;
            music = transform.GetComponentsInChildren<AudioSource>()[2];
            music.Play();
            invisibleTime = invisibleDuration;
            RefreshTransparent();
        }
    }

    /// <summary>
    /// 无敌技能
    /// </summary>
    private void Unbeatable()
    {
        if (photonView.IsMine)
        {
            unbeatableTime = unbeatableDuration;
        }
    }

    /// <summary>
    /// Get the skill index of current player
    /// </summary>
    /// <returns>this player's skill number.</returns>
    public int getSkillNumber()
    {
        return skillNumber;
    }

    #region PUN Callbacks
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(invisibleDuration);
            stream.SendNext(invisibleTime);
            stream.SendNext(unbeatableDuration);
            stream.SendNext(unbeatableTime);
            stream.SendNext(invincible);
        }
        else
        {
            // Network player, receive data
            this.invisibleDuration = (float)stream.ReceiveNext();
            this.invisibleTime = (float)stream.ReceiveNext();
            this.unbeatableDuration = (float)stream.ReceiveNext();
            this.unbeatableTime = (float)stream.ReceiveNext();
            this.invincible = (bool)stream.ReceiveNext();
        }
    }
    #endregion
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
