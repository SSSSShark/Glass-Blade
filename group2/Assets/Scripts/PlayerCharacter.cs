using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : MonoBehaviour
{
  /*************** 李晨昊 begin **************************/
  public Rigidbody weapon;  //武器
  public Transform muzzle;  //辅助武器旋转变量
  Rigidbody weaponInstance; //武器的实例变量
  CharacterController cc;
  public float attackTime;  //攻击持续时间

  public bool isAlive = true;
  public bool attacking = false;
  /**************** 李晨昊 end **************************/

  /********************* 林海力 begin *******************/
  public bool isProtected = false; //无敌状态标识
  public bool isJustAlive = false; //是否刚刚复活标识
  int protecttime;
  public int protecttimeMAX = 1;
  /********************** 林海力 end *********************/

  /****************** 汪至磊 begin **********************/
  //黑白渲染组件
  BlackAndWhite baw;
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
  public int holdWeaponIndex = 0;
  public int weaponKinds = 4;
  public Transform[] weapons;
  /******************** 汪至磊 end **********************/

  //four weapons
  /********************* 林海力 begin *******************/
  public GameObject weaponattack1;
  public Rigidbody Axe;
  Rigidbody Axesetinstance;
  public float Axe2handDelayTime = 1.0f;
  /********************** 林海力 end *********************/
  /******************** 李晨昊 begin ********************/
  public Rigidbody dagger;
  public float daggerAttackDelayTime = 1.0f;
  public float launchForce = 10;
  public Rigidbody swordTwoHanded;
  public float swordTwoHandedDelayTime = 1.0f;
  public float angle = 120f; //扇形角度
  public float radius = 4f; //扇形半径
  /******************** 李晨昊 end **********************/
  /****************** 汪至磊 begin **********************/
  public Rigidbody sword;
  public float swordAttackDelayTime = 1.0f;
  Rigidbody tempInstance;
  /******************** 汪至磊 end **********************/

  /*************************** 李晨昊 begin *************************/
  public void Attack() {
    if (!isAlive) return;
    if (attacking) return;
    attacking = true;
    if (isHoldWeapon)
    {
      switch(holdWeaponIndex) {
        case 1: Invoke("Axe2HandAttack", Axe2handDelayTime); break;
        case 2: Invoke("DaggerAttack", daggerAttackDelayTime); break;
        case 3: Invoke("SwordTwoHandedAttack", swordTwoHandedDelayTime); break;
        case 4: Invoke("SwordAttack", swordAttackDelayTime); break;
      }


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

  void RefreshAttack() {
    attacking = false;
  }
  //four weapon attack
  /**********************week9, first weapon, 林海力************/
  public void Axe2HandAttack()
  {
    Quaternion qtarget = Quaternion.AngleAxis(90, this.transform.forward) * this.transform.rotation;//实例化武器
    Axesetinstance = Instantiate(Axe, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, qtarget) as Rigidbody;
    Axesetinstance.transform.SetParent(this.transform, false);
    Axesetinstance.transform.localPosition = new Vector3(0, 1, 0);
    Axesetinstance.angularVelocity = this.transform.up * 1 * 7.0f;
    GameObject a = Instantiate(weaponattack1, transform.position, Quaternion.identity);//实例化攻击范围
    a.transform.parent = this.transform;
    a.transform.localPosition = new Vector3(0, 1, 0);
    var t = a.GetComponent<weapon1colider>();
    t.setAttackTime(attackTime);
  }
  /**********************week9, first weapon************/
  void DaggerAttack() {
    var daggerInstance = Instantiate(dagger, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, weapons[2].rotation) as Rigidbody;
    daggerInstance.velocity = launchForce * transform.forward;
    // Invoke("daggerDelay", daggerAttackDelayTime);
    //Destroy(dagger);
  }
  /*********************************************** 李晨昊 week9 begin ************************************************************/
  void SwordTwoHandedAttack()
  {
    var swordTwoHandedInstance = Instantiate(swordTwoHanded, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, this.transform.rotation) as Rigidbody;
    swordTwoHandedInstance.transform.SetParent(this.transform, false);
    swordTwoHandedInstance.transform.localPosition = new Vector3(0, 1, 1);  // 设置实例初始出现位置
    swordTwoHandedInstance.transform.localRotation = Quaternion.Euler(new Vector3(90, -60, 0)); // 设置初始角度
    swordTwoHandedInstance.angularVelocity = this.transform.up * 1 * 2.5f;  // 旋转轴 * 方向 * 角速度

    var gos = GameObject.FindGameObjectsWithTag("Player"); // 获取所有人物
    foreach (var go in gos)
    {         //逐一判断是否在攻击范围内
      if (UmbrellaAttact(this.gameObject.transform, go.transform, angle, radius))
      {
        go.GetComponent<PlayerCharacter>().TakeDamage();
      }
    }
  }


  // 判断敌人是否在扇形攻击范围内
  bool UmbrellaAttact(Transform attacker, Transform attacked, float angle, float radius)
  {
    Vector3 deltaA = attacked.position - attacker.position;
    float tmpAngle = Mathf.Acos(Vector3.Dot(deltaA.normalized, attacker.forward)) * Mathf.Rad2Deg;

    float tmpHeight = Mathf.Abs(attacked.position.y - attacker.position.y);
    if (tmpAngle < angle * 0.5f && deltaA.magnitude < radius && tmpHeight < 1.5f)
    {
      return true;
    }
    return false;
  }
  /************************************** 李晨昊 week9 end *******************************************/

  /****************week9, fouth weapon, 汪至磊**********************************/
  void SwordAttack()
  {
    tempInstance = Instantiate(sword,
                               this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward,
                               weapons[4].rotation) as Rigidbody;
    tempInstance.velocity = 9 * transform.forward;
    //SwordInstance.MovePosition(new Vector3(0, 1, 0) + this.transform.forward);
    //SwordInstance.velocity = 10 * transform.forward;
    Invoke("SwordAttackAnime", 0.1f);
  }
  void SwordAttackAnime()
  {
    tempInstance.velocity -= 2 * transform.forward;
    if(tempInstance.velocity.magnitude > 10)
    {
      tempInstance.velocity = 0 * transform.forward;
      CancelInvoke("SwordAttackAnime");
    }
    Invoke("SwordAttackAnime", 0.1f);
  }
  /**************************forth weapon******************************************/



  public void TakeDamage() {
    Debug.Log("damage");
    // 非刚复活无敌状态且活着，才能受到伤害死亡
    if (!isProtected && !isJustAlive && isAlive)
    {
      Death();
    }
  }



  /************************************ 李晨昊 end ********************/

  /***************************** 汪至磊 begin ************************/
  public void Death() {
    //**灰屏
    baw.setDeath();
    isAlive = false;
    //**倒下,先停1s,用2s分段倒下
    rotatedtimes = 0;
    InvokeRepeating("DeathRotate", 1f, (float)(2.0 / DeathRotateTimes));

    //**倒计时
    counttime = DeathTime;
    InvokeRepeating("CountDown", 0f, 1.0f);
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
    baw.setLive();
    
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

  public bool TakeWeapon(int weaponstatus)
  {
    bool temp = isHoldWeapon;
    if (weaponstatus > 0)
    {
      holdWeapon(weaponstatus, true);
    }
    return temp;
  }
  /**************************** 林海力 end **********************/




  // Start is called before the first frame update
  void Start() {
    cc = GetComponent<CharacterController>();
    baw = GameObject.FindObjectOfType<BlackAndWhite>();
    //weapons = weaponObject.GetComponentsInChildren<Transform>();
    //foreach (Transform child in weapons) child.gameObject.SetActive(false);
    for (int i = 1; i <= weaponKinds; i++)
    {
      //Debug.Log(weapons[i].name);
      weapons[i].gameObject.SetActive(false);
    }
    //week9 test here ,change holdWeaponIndex
    isHoldWeapon = true;
    holdWeaponIndex = 1;
  }

}
