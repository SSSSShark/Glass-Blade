using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Com.Glassblade.Group1;
using TMPro;

public class PlayerCharacter : MonoBehaviourPun, IPunObservable
{
    public List<float> attackdelaytime;
    private int viewID;
    /*************** 李晨昊 begin **************************/
    public Transform muzzle;  //辅助武器旋转变量
    GameObject weaponInstance; //武器的实例变量
    public float attackTime;  //攻击持续时间

    public bool isAlive = true;
    bool attacking = false;
    /**************** 李晨昊 end **************************/

    /********************* 林海力 begin *******************/
    public bool isProtected = false; //无敌状态标识
    public bool isJustAlive = false; //是否刚刚复活标识
    int protectTime;
    public int protectTimeMax = 1;
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
    public List<string> fireWeapenprefab; //挂所有武器prefab，顺序应该与weaponObject中的顺序对应(投掷出的武器)
    public string DefalutWeapenprefab; //挂默认武器
    public int weaponKinds = 4;
    /******************** 汪至磊 end **********************/
    bool isinvincible = false;

    #region Public Fields

    public float speed = 25;
    CharacterController cc;
    //黑白渲染组件
    BlackAndWhite baw;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the scene")]
    public static GameObject LocalPlayerInstance;

    public GameObject AttackBtn;

    // The game manager attached to the scene
    public GameManager GM;

    public GameObject gamePlayer;

    public Button skillBtn;

    public GameObject KDboard;

    // Weapons
    public GameObject weapon_sword;
    public GameObject weapon_dagger;
    public GameObject weapon_two_handed_axe;
    public GameObject weapon_two_handed_sword;

    private Transform[] weapons;

    #endregion

    #region occupy mode
    public TextMeshProUGUI KDtext;

    public OMode OM = null;

    [HideInInspector]
    public int killTime = 0;   //击杀数

    [HideInInspector]
    public int deathTime = 0;  //死亡数

    [HideInInspector]
    public int score = 0;  //个人得分

    [HideInInspector]
    public TeamController.Team team;

    [PunRPC]
    public void UpdateScore(int increase)
    {
        score += increase;
    }

    public void CallUpdateScore(Player player, int increase)
    {
        this.photonView.RPC("UpdateScore", player, increase);
    }

    [PunRPC]
    public void UpdateKillTime(int increase)
    {
        this.killTime += increase;
        Debug.Log("[PlayerCharacter:UpdateKillTime()] Update " + this.photonView.Owner.NickName + "'s killTIme");
        Debug.Log("[PlayerCharacter:UpdateKillTime()] " + this.photonView.Owner.NickName + ": " + this.killTime);
    }

    #endregion

    /*************************** 李晨昊 begin *************************/
    #region group2

    // Locally cached weapon refresh position
    // should be filled by game manager
    public GameObject[] refresh_places;

    private void OnTriggerEnter(Collider col)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        //Debug.Log("collide something...");

        // check if we entered the weapon refresh site
        WeaponRefresh target = col.GetComponent<WeaponRefresh>();

        if (target)
        {
            if (isHoldWeapon)
            {
                return;
            }
            Debug.Log("[PlayerCharacter:OnTriggerEnter()] client: it is weapon refresh site, weapon status : " + target.weaponstatus);
            target.tryPickWeapon(target.weaponstatus);
        }
    }

    public void Attack()
    {
        if (!photonView.IsMine)
        {
            return;
        }
        if (!isAlive)
        {
            return;
        }

        if (attacking)
        {
            return;
        }

        this.gameObject.GetComponent<movegetgromjoystick>().moveEnable = false;

        attacking = true;
        if (photonView.IsMine)
        {
            CharacterBehavior CB = gamePlayer.GetComponent<CharacterBehavior>();
            if (CB.invisibleTime > 0)
            {
                CB.invisibleTime = 0;
                CB.CallRefreshTransparent();
                Debug.Log("[PlayCharacter:Attack()] Player " + photonView.Owner.NickName + " is no longer invisible");
            }
            if (isHoldWeapon)
            {
                weaponInstance = PhotonNetwork.Instantiate(fireWeapenprefab[holdWeaponIndex - 1], this.transform.position + new Vector3(0, 1, 0) + this.transform.forward, transform.rotation * Quaternion.Euler(0.0f, 0.0f, 90.0f) * Quaternion.Euler(90.0f, 0.0f, 0.0f));

                Debug.Log("[PlayCharacter:Attack()] Player " + photonView.Owner.NickName + " Attack");

                // play sound
                switch (holdWeaponIndex)
                {
                    // attack using sword
                    case 1: PlayAudio(4, true); break;

                    // attack using dagger
                    case 2: PlayAudio(6, true); break;

                    // attack using 2-handed axe
                    case 3: PlayAudio(5, true); break;

                    // attack using 2 handed sword
                    case 4: PlayAudio(3, true); break;
                }
                photonView.RPC("holdWeapon", RpcTarget.All, holdWeaponIndex, false);
                // holdWeapon(holdWeaponIndex, false);
            }
            else  //使用默认weapon
            {
                // 武器出现位置
                PlayAudio(7, true);



                weaponInstance = PhotonNetwork.Instantiate(DefalutWeapenprefab, this.transform.position + new Vector3(0, 1, 0) + this.transform.forward, transform.rotation * Quaternion.Euler(0.0f, 0.0f, 90.0f) * Quaternion.Euler(90.0f, 0.0f, 0.0f));
                // 武器旋转表示攻击动作
            }
            //Debug.Log(this.photonView.ViewID);
            weaponInstance.GetComponent<Weapon>().weaponOwner = PhotonNetwork.LocalPlayer;
            weaponInstance.GetComponent<Weapon>().photonviewOwner = PhotonView.Get(this);
            weaponInstance.GetComponent<Weapon>().initPos = transform.position;
            weaponInstance.GetComponent<Weapon>().Pc = GetComponent<PlayerCharacter>();
            //Debug.Log(weaponInstance.GetComponent<Weapon>().initPos);
            weaponInstance.GetComponent<Weapon>().initForward = transform.forward;
            weaponInstance.GetComponent<Weapon>().destroyTime = attackTime;
            Invoke("AttackAfterDelay", attackdelaytime[holdWeaponIndex]);
        }
    }

    void AttackAfterDelay()
    {
        weaponInstance.GetComponent<Weapon>().Fire();
        Invoke("RefreshAttack", attackTime);
    }

    void RefreshAttack()
    {
        attacking = false;
        if (isAlive)
        {
            this.gameObject.GetComponent<movegetgromjoystick>().moveEnable = true;
        }
    }

    /// <summary>
    /// This function plays audio on the owner part and sends RPC to other client if needed
    /// </summary>
    /// <param name="index">The audio source index</param>
    /// <param name="RPCRequired">If we need RPC or not, if true, we will send RPC</param>
    public void PlayAudio(int index, bool RpcRequired)
    {
        if (!photonView.IsMine)
        {
            return;
        }
        Debug.Log("[PlayerCharacter:PlayAudio()] We are the owner, play audio, index = " + index);
        AudioSource music = transform.GetComponentsInChildren<AudioSource>()[index];
        if (music.isActiveAndEnabled)
        {
            music.Play();

            if (RpcRequired)
            {
                Debug.Log("[PlayerCharacter:PlayAudio()] RpcRequired flag set, send RPC to other client.");
                photonView.RPC("PlayAudioOnPlayer", RpcTarget.Others, index);
            }
        }
    }


    /// <summary>
    /// Some other clients notified us that they killed this client, so we are
    /// dead.
    /// </summary>
    [PunRPC]
    public void TakeDamage(int srcviewID, string weaponname, TeamController.Team srcteam)
    {
        if (!isProtected && !isJustAlive && !gamePlayer.GetComponent<CharacterBehavior>().invincible && isAlive)
        {
            this.gameObject.GetComponent<movegetgromjoystick>().moveEnable = false;
            isAlive = false;
            deathTime += 1;
            PhotonView photonView = PhotonView.Find(srcviewID);
            //为什么RPCtartget怎么改都不影响结果？？？
            photonView.RPC("UpdateKillTime", RpcTarget.All, 1);

            if (KDboard)
            {
                KDboard.GetComponent<KDBoardView>().CallUpdateBoard(photonView.Owner.NickName, this.photonView.Owner.NickName, weaponname, (int)srcteam, (int)team);
            }
            //非占点模式
            if (!GameObject.Find("OData"))
            {
                photonView.RPC("UpdateScore", RpcTarget.All, 100);
                if ((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA)
                {
                    GameObject.FindGameObjectWithTag("ScoreB").GetComponent<Scores>().SendScoreInfo();
                }
                else
                {
                    GameObject.FindGameObjectWithTag("ScoreA").GetComponent<Scores>().SendScoreInfo();
                }
            }
            Death();
        }
    }

    [PunRPC]
    /// <summary>
    /// This RPC is called to play audio on the player
    /// </summary>
    public void PlayAudioOnPlayer(int index, PhotonMessageInfo info)
    {
        if (photonView.Owner == info.Sender)
        {
            // play music
            AudioSource music = transform.GetComponentsInChildren<AudioSource>()[index];
            music.Play();
        }
    }

    /// <summary>
    /// The player does not know that it takes damage, so we need to RPC to the client
    /// to notify him.
    /// TODO: Another possible solution would be using photonview in weapon, so that the client
    /// can determine whether it is attacked.
    /// </summary>
    public void CallTakeDamage(Player targetPlayer, PhotonView srcview)
    {
        this.photonView.RPC("TakeDamage", targetPlayer, srcview.ViewID);

    }

    // public void CallTakeDamage(Player targetplayer, PhotonView srcview)
    // {      
    //     this.photonView.RPC("TakeDamage", targetplayer, srcview.ViewID);   
    // }
    /************************************ 李晨昊 end ********************/

    /***************************** 汪至磊 begin ************************/
    //[PunRPC]
    //public void CallDeathEvent(PhotonMessageInfo info)
    //{
    //    if (info.photonView)
    //    {
    //        info.photonView.GetComponent<PlayerCharacter>().OM.DeathEvent(info.photonView.GetComponent<PlayerCharacter>());
    //    }
    //}

    public void Death()
    {

        PlayAudio(9, false);
        isProtected = true; //tt

        isHoldWeapon = false;
        weapons[holdWeaponIndex].gameObject.SetActive(false);

        // 将两个碰撞体移出地图
        //this.transform.GetComponent<CharacterController>().center = new Vector3(180.0f, 1.34f, -10.0f);
        //this.transform.GetComponent<CapsuleCollider>().center = new Vector3(180.0f, 0.0f, -10.0f);

        //  isAlive = false;
        //**倒下,先停1s,用2s分段倒下
        rotatedtimes = 0;
        InvokeRepeating("DeathRotate", 1f, (float)(2.0 / DeathRotateTimes));
        //deathevent.Invoke(this); // 占点模式引发

        //**灰屏    
        baw.setDeath();
        //**倒计时
        counttime = DeathTime;
        InvokeRepeating("CountDown", 0f, 1.0f);
        //}
    }

    //**大风车吱呀吱悠悠的转~
    void DeathRotate()
    {
        //Debug.Log("time = "+Time.time);
        transform.Rotate(90 / DeathRotateTimes, 0, 0);
        rotatedtimes++;
        if (rotatedtimes >= DeathRotateTimes)
        {
            CancelInvoke("DeathRotate");
            //Vector3 CurPosition = transform.position;
            //CurPosition.y = -100;
            //transform.position = CurPosition;
            transform.position = new Vector3(180.0f, 0.0f, -10.0f);
            //this.transform.GetComponent<CharacterController>().center = new Vector3(0.0f, 1.34f, 0.0f);
            //this.transform.GetComponent<CapsuleCollider>().center = new Vector3(0.0f, 0.0f, 0.0f);
        }
    }

    //**倒计时
    void CountDown()
    {
        if (!counter)
        {
            //counter = GameObject.FindGameObjectWithTag("ScreenText").GetComponent<Text>();
            counter = GameObject.Find("DeathCounter").GetComponent<Text>();
            Debug.Log("[PlayerCharacter:CountDown()] attach counter");
        }
        if (counttime == 0)  //复活
        {
            counter.text = "";
            CancelInvoke("CountDown");
            Invoke("Relive", 1f);
        }
        else
        {
            counter.text = "剩余时间:" + counttime;
            counttime -= 1;
        }
    }

    [PunRPC]
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
    /// <summary>
    /// Relive is called when local player is out of death, and we place
    /// the player to the relive position, with protection.
    /// </summary>
    public void Relive()
    {
        if (photonView.IsMine)
        {
            // Debug.Log("relive time = " + Time.time);
            PlayAudio(10, false);

            baw.setLive();
            isProtected = false;  // tt
            isAlive = true;
            //var reliver = Random.Range(-180.0f, 180.0f);
            //transform.rotation = Quaternion.Euler(0, reliver, 0);

            //float reliveX;
            //float reliveZ;
            //float reliveY = 1;
            //if ((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA)
            //{
            //    Debug.Log("[PlayerCharacter:Relive()] Team A player relive, placed to team A position");
            //    reliveX = -24;
            //    reliveZ = -46;
            //}
            //else
            //{
            //    Debug.Log("[PlayerCharacter:Relive()] Team B player relive, placed to team B position");
            //    reliveX = 24;
            //    reliveZ = 46;
            //}
            //transform.position = new Vector3(reliveX, reliveY, reliveZ);

            this.gameObject.GetComponent<movegetgromjoystick>().moveEnable = true;

            // relive-protect
            protectTime = protectTimeMax;
            if (protectTime <= 0)
            {
                isJustAlive = false;
            }
            else
            {
                isJustAlive = true;
                InvokeRepeating("protectDecrease", 0f, 1.0f);
            }
        }
    }
    void protectDecrease()
    {
        if (protectTime == 0)
        {
            isJustAlive = false;
            CancelInvoke("protectDecrease");
        }
        else
        {
            protectTime--;
        }
    }

    public void TakeWeapon(int weaponstatus)
    {
        Debug.Log("[PlayerCharacter:TakeWeapon()] called");
        photonView.RPC("holdWeapon", RpcTarget.All, weaponstatus, true);
        // holdWeapon(weaponstatus, true);
        PlayAudio(8, false);
    }
    /**************************** 林海力 end **********************/

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            if (GameObject.Find("KDText"))
            {
                KDtext = GameObject.Find("KDText").GetComponent<TextMeshProUGUI>();
            }

            if (GameObject.Find("KDBoard"))
            {
                KDboard = GameObject.Find("KDBoard");
            }

            cc = GetComponent<CharacterController>();
            baw = GameObject.FindObjectOfType<BlackAndWhite>(); ;

            //foreach (Transform child in weapons) child.gameObject.SetActive(false);

            // set team
            team = (TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"];
        }

        // this should be set for all player instances
        Debug.Log("Player " + photonView.Owner.NickName + " Resetting weapons");
        weapons = new Transform[weaponKinds + 1];
        weapons[1] = weapon_sword.GetComponent<Transform>();
        weapons[2] = weapon_dagger.GetComponent<Transform>();
        weapons[3] = weapon_two_handed_axe.GetComponent<Transform>();
        weapons[4] = weapon_two_handed_sword.GetComponent<Transform>();

        for (int i = 1; i <= weaponKinds; i++)
        {
            weapons[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log(transform.position);
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        if (KDtext)
        {
            KDtext.text = "🔪: " + killTime + "  " + "💀: " + deathTime + "  " + "🏆: " + score + "  ";
        }
        if (gameObject.transform.position.y < -15.0f)
        {
            gameObject.GetComponent<movegetgromjoystick>().moveEnable = false;
            isAlive = false;
            transform.position = new Vector3(180.0f, 0.0f, -10.0f);
            //if (!IsInvoking("Relive"))
            //{
            //    Invoke("Relive", 10.0f);
            //}
            if (!IsInvoking("CountDown"))
            {
                baw.setDeath();
                counttime = DeathTime;
                InvokeRepeating("CountDown", 0f, 1.0f);
            }
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
            stream.SendNext(killTime);
            stream.SendNext(deathTime);
            stream.SendNext(score);
            stream.SendNext((int)team);
        }
        else
        {
            // Network player, receive data
            this.isProtected = (bool)stream.ReceiveNext();
            this.isJustAlive = (bool)stream.ReceiveNext();
            this.isAlive = (bool)stream.ReceiveNext();
            this.killTime = (int)stream.ReceiveNext();
            this.deathTime = (int)stream.ReceiveNext();
            this.score = (int)stream.ReceiveNext();
            this.team = (TeamController.Team)stream.ReceiveNext();
        }
    }
    #endregion
}
