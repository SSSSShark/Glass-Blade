using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCharacter : MonoBehaviourPun, IPunObservable
{
    /*************** 李晨昊 begin **************************/
    public Rigidbody weapon;  //武器
    public Transform muzzle;  //辅助武器旋转变量
    Rigidbody weaponInstance; //武器的实例变量
    public float attackTime;  //攻击持续时间

    public bool isAlive = true;
    bool attacking = false;
    /**************** 李晨昊 end **************************/

    /********************* 林海力 begin *******************/
    public bool isProtected = false; //无敌状态标识
    public bool isJustAlive = false; //是否刚刚复活标识
    int protecttime;
    public int protecttimeMAX = 1;
    /********************** 林海力 end *********************/

    /****************** 汪至磊 begin **********************/
    
    //死亡倒计时组件
    public Text counter;  //counter 挂 Main Camera\Canvas\Text
    //死亡倒地段数
    public int DeathRotateTimes = 10;
    //复活时间
    public int DeathTime = 10;
    //全局变量
    int rotatedtimes;
    int counttime;
    bool isHoldWeapon = false;
    //持有武器种类
    int holdWeaponIndex = 1;
    //武器对象    
    public GameObject weaponObject; //weaponObject 挂 Player\Mr Black\weapons
    public int weaponKinds = 4;
    /******************** 汪至磊 end **********************/

    #region Public Fields

    public float speed = 25;
    CharacterController cc;
    //黑白渲染组件
    BlackAndWhite baw;
    Transform[] weapons;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the scene")]
    public static GameObject LocalPlayerInstance;

    #endregion

    /*************************** 李晨昊 begin *************************/
    #region group2
    public void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!isAlive) return;
        if (attacking) return;
        attacking = true;
        if (isHoldWeapon)
        {
            // 待做攻击

            Debug.Log("attack");
            holdWeapon(holdWeaponIndex, false);
        }
        else
        {
            // 武器出现位置
            muzzle.localPosition = this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward;
            weaponInstance = Instantiate(weapon, muzzle.localPosition, this.transform.rotation) as Rigidbody;
            // 武器旋转表示攻击动作
            weaponInstance.angularVelocity = this.transform.right * 1 * 2;
        }
        Invoke("RefreshAttack", attackTime);
    }

    void RefreshAttack()
    {
        attacking = false;
    }

    [PunRPC]
    public void TakeDamage()
    {
        // 非刚复活无敌状态且活着，才能受到伤害死亡
        if (!this.isProtected && !this.isJustAlive && this.isAlive)
        {
            Debug.Log("under attack");
            this.isAlive = false;
            // Death();
        }
    }

    public void SendDamage(Player player)
    {
        this.photonView.RPC("TakeDamage", player);
    }
    /************************************ 李晨昊 end ********************/

    /***************************** 汪至磊 begin ************************/
    public void Death() {
        
        isProtected = true; //tt
        
      //  isAlive = false;
        //**倒下,先停1s,用2s分段倒下
        rotatedtimes = 0;
        InvokeRepeating("DeathRotate", 1f, (float)(2.0 / DeathRotateTimes));

        if (photonView.IsMine) {
            this.gameObject.GetComponent<movegetgromjoystick>().enabled = false;
            if ((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA)
            {
                GameObject.FindGameObjectWithTag("ScoreB").GetComponent<Scores>().SendScoreInfo();
            }
            else
            {
                GameObject.FindGameObjectWithTag("ScoreA").GetComponent<Scores>().SendScoreInfo();
            }
            //**灰屏    
            baw.setDeath();
            //**倒计时
            counttime = DeathTime;
            InvokeRepeating("CountDown", 0f, 1.0f);
        }
    }

    //**大风车吱呀吱悠悠的转~
    void DeathRotate() {
        //Debug.Log("time = "+Time.time);
        transform.Rotate(90 / DeathRotateTimes, 0, 0);
        rotatedtimes++;
        if (rotatedtimes >= DeathRotateTimes) {
            CancelInvoke("DeathRotate");
        }
    }

    //**倒计时
    void CountDown() {
        if (!counter)
        {
            counter = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<Text>();
            Debug.Log("attach counter");
        }
        if (counttime == 0)  //复活
        {
            counter.text = "";
            CancelInvoke("CountDown");
            Invoke("Relive", 1f);
        }
        else {
            counter.text = "剩余时间:" + counttime;
            counttime = counttime - 1;
        }
    }

    //在人物模型上显示或消失某一个武器
    //index为武器索引,ishold表示显示或不显示武器
    //武器没有碰撞器,因为攻击不使用碰撞实现
    void holdWeapon(int index, bool ishold)
    {
        if (ishold && !isHoldWeapon)
        { //未持有武器时可以拿一种武器
            holdWeaponIndex = index;
            isHoldWeapon = true;
            weapons[index].gameObject.SetActive(true);
        }
        else if (!ishold && isHoldWeapon)
        { //持有武器时才能用掉一种武器
            isHoldWeapon = false;
            weapons[holdWeaponIndex].gameObject.SetActive(false);
        }
    }
    /***************************** 汪至磊 end ***********************/

    /************************** 林海力 begin *************************/
    public void Relive() {
        //Debug.Log("relive time = " + Time.time);
        this.gameObject.GetComponent<movegetgromjoystick>().enabled = true;
        baw.setLive();
        isProtected = false;  // tt
        isAlive = true;
        var reliver = Random.Range(-180.0f, 180.0f);
        transform.rotation = Quaternion.Euler(0, reliver, 0);
        var relivex = Random.Range(-40.0f, 40.0f);
        var relivez = Random.Range(-40.0f, 40.0f);
        transform.position = new Vector3(relivex, 1, relivez);
        //relive-protect
        protecttime = protecttimeMAX;
        if (protecttime <= 0) {
            isJustAlive = false;
        }
        else {
            isJustAlive = true;
            InvokeRepeating("protectDecrease", 0f, 1.0f);
        }
    }
    void protectDecrease() {
        if (protecttime == 0) {
            isJustAlive = false;
            CancelInvoke("protectDecrease");
        }
        else {
            protecttime--;
        }
    }

    /*[PunRPC]*/
    public bool TakeWeapon(int weaponstatus/*, PhotonMessageInfo info*/)
    {
        //this.photonView.RPC("", info.Sender, );
        bool temp = isHoldWeapon;
        if (weaponstatus > 0)
        {
            holdWeapon(weaponstatus, true);
        }
        return temp;
    }
    /**************************** 林海力 end **********************/

    #endregion

    // Start is called before the first frame update
    void Start()
    {
 
        cc = GetComponent<CharacterController>();
        baw = GameObject.FindObjectOfType<BlackAndWhite>();
        weapons = weaponObject.GetComponentsInChildren<Transform>();
            //foreach (Transform child in weapons) child.gameObject.SetActive(false);
        for (int i = 1; i <= weaponKinds; i++)
        {
            //Debug.Log(weapons[i].name);
            weapons[i].gameObject.SetActive(false);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (!isAlive && !isProtected)
        {
            Death();
        }

    }

    #region PUN Callbacks
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(isProtected);
            stream.SendNext(isJustAlive); 
            stream.SendNext(isAlive);
        }
        else
        {
            // Network player, receive data
            this.isProtected = (bool)stream.ReceiveNext();
            this.isJustAlive = (bool)stream.ReceiveNext();
            this.isAlive = (bool)stream.ReceiveNext();
        }
    }
    #endregion
}
