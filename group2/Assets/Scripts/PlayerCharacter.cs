using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.GlassBlade.Group2
{
    public class PlayerCharacter : MonoBehaviour
    {
        /*************** 李晨昊 begin **************************/
        //武器
        public Rigidbody weapon;

        //辅助武器旋转变量
        public Transform muzzle;

        //武器的实例变量
        private Rigidbody weaponInstance;

        private CharacterController cc;

        //攻击持续时间
        public float attackTime;

        //死生状态
        public bool isAlive = true;

        //攻击中
        public bool attacking = false;

        //击杀数
        public int killTime = 0;

        //被击杀数
        public int deathTime = 0;

        //匕首
        public Rigidbody dagger;

        //匕首攻击前摇时间
        public float daggerAttackDelayTime = 1.0f;

        //匕首飞出速度调节
        public float launchForce = 10;

        //双手剑
        public Rigidbody swordTwoHanded;

        //双手剑攻击前摇时间
        public float swordTwoHandedDelayTime = 1.0f;

        //双手剑攻击判定扇形角度
        public float angle = 120f;

        //双手剑攻击判定扇形半径
        public float radius = 4f;
        /**************** 李晨昊 end **************************/

        /********************* 林海力 begin *******************/
        //无敌状态标识
        public bool isProtected = false;

        //是否刚刚复活标识
        public bool isJustAlive = false;

        //复活保护的时长
        private int protecttime;

        public int protecttimeMAX = 0;
        /********************** 林海力 end *********************/

        /****************** 汪至磊 begin **********************/
        //黑白渲染组件
        BlackAndWhite baw;

        //死亡倒计时组件
        //counter 挂 Main Camera\Canvas\Text
        public Text counter;

        //死亡倒地时间
        public double DeathContinueTime = 0.5;

        //死亡倒地段数
        public int DeathRotateTimes = 10;

        //复活时间
        public int DeathTime = 10;

        //全局变量
        private int rotatedtimes;

        private int counttime;

        private bool isHoldWeapon = false;

        //持有武器种类
        public int holdWeaponIndex = 0;

        //已实装武器种类
        public int weaponKinds = 4;

        //人物下的武器模型,0号位闲置
        public Transform[] weapons;
        /******************** 汪至磊 end **********************/

        //four weapons
        /********************* 林海力 begin *******************/
        //武器双手斧攻击范围
        public GameObject weaponAttack1;

        //武器双手斧实例
        public Rigidbody Axe;

        private Rigidbody Axesetinstance;

        //双手斧前摇
        public float Axe2handDelayTime = 1.0f;
        /********************** 林海力 end *********************/

        /****************** 汪至磊 begin **********************/
        public Rigidbody sword;

        public float swordAttackDelayTime = 1.0f;

        Rigidbody tempInstance;
        /******************** 汪至磊 end **********************/

        /*************************** 李晨昊 begin *************************/
        /// <summary>
        /// Attack 攻击函数，在按下攻击键时被调用，有四种外加一种默认攻击方式
        /// </summary>
        public void Attack()
        {
            if (!isAlive)
            {
                return;
            }
            if (attacking)
            {
                return;
            }
            attacking = true;
            if (isHoldWeapon)
            {
                switch (holdWeaponIndex)
                {
                    //双手斧攻击
                    case 1: Invoke("Axe2HandAttack", Axe2handDelayTime); break;

                    //匕首攻击
                    case 2: Invoke("DaggerAttack", daggerAttackDelayTime); break;

                    //双手剑攻击
                    case 3: Invoke("SwordTwoHandedAttack", swordTwoHandedDelayTime); break;

                    //单手剑攻击
                    case 4: Invoke("SwordAttack", swordAttackDelayTime); break;
                }
                HoldWeapon(holdWeaponIndex, false);
            }
            else
            {
                weaponInstance = Instantiate(weapon, new Vector3(0, 1, 0.5f), this.transform.rotation) as Rigidbody;
                weaponInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                weaponInstance.transform.SetParent(this.transform, false);

                // 武器旋转表示攻击动作
                weaponInstance.angularVelocity = this.transform.right * 1 * 5.0f;
            }
            Invoke("RefreshAttack", attackTime);
        }

        /// <summary>
        /// RefreshAttack 刷新攻击使能
        /// </summary>
        private void RefreshAttack()
        {
            attacking = false;
        }

        /// <summary>
        /// DaggerAttack 实现匕首攻击动画和攻击判定
        /// 攻击方式为匕首飞出，采用碰撞判定有效攻击
        /// </summary>
        private void DaggerAttack()
        {
            var daggerInstance = Instantiate(dagger, new Vector3(0, 1, 1), weapons[2].localRotation) as Rigidbody;
            daggerInstance.transform.SetParent(this.transform, false);
            daggerInstance.velocity = launchForce * transform.forward;
        }

        /// <summary>
        /// SwordTwoHandedAttack 实现双手剑攻击动画和攻击判定
        /// 攻击方式为扇形横扫，采用查询扇形区域内玩家对象判定有效攻击
        /// </summary>
        private void SwordTwoHandedAttack()
        {
            var swordTwoHandedInstance = Instantiate(swordTwoHanded, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, this.transform.rotation) as Rigidbody;
            swordTwoHandedInstance.transform.SetParent(this.transform, false);

            // 设置实例初始出现位置
            swordTwoHandedInstance.transform.localPosition = new Vector3(0, 1, 1);

            // 设置初始角度
            swordTwoHandedInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 30, 90));

            // 旋转轴 * 方向 * 角速度
            swordTwoHandedInstance.angularVelocity = this.transform.up * 1 * 5.0f;

            // 获取所有人物
            var gos = GameObject.FindGameObjectsWithTag("Player");

            //逐一判断是否在攻击范围内
            foreach (var go in gos)
            {
                if (UmbrellaAttact(this.gameObject.transform, go.transform, angle, radius) && go.GetComponent<PlayerCharacter>().isAlive)
                {
                    go.GetComponent<PlayerCharacter>().TakeDamage();
                    go.GetComponent<PlayerCharacter>().deathTime++;
                    this.killTime++;
                }
            }
        }

        /// <summary>
        /// UmbrellaAttact 判断敌人是否在扇形攻击范围内
        /// </summary>
        /// <param name="attacker">攻击者</param>
        /// <param name="attacked">被攻击者</param>
        /// <param name="angle">扇形区域的圆心角度</param>
        /// <param name="radius">扇形区域的半径</param>
        /// <returns></returns>
        private bool UmbrellaAttact(Transform attacker, Transform attacked, float angle, float radius)
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

        /// <summary>
        /// Takedamage 承受攻击函数，受到攻击时调用
        /// </summary>
        public void TakeDamage()
        {
            Debug.Log("damage");

            // 非刚复活无敌状态且活着，才能受到伤害死亡
            if (!isProtected && !isJustAlive && isAlive)
            {
                Death();
            }
        }
        /*************************** 李晨昊 end **********************/

        //four weapon attack
        /**********************week9, first weapon, 林海力************/
        /// <summary>
        /// 双手斧实例化武器，武器仅有动画；攻击由范围碰撞来实现
        /// </summary>
        public void Axe2HandAttack()
        {
            //实例化武器
            Quaternion qtarget = Quaternion.AngleAxis(90, this.transform.forward) * this.transform.rotation;
            Axesetinstance = Instantiate(Axe, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, qtarget) as Rigidbody;
            Axesetinstance.transform.SetParent(this.transform, false);
            Axesetinstance.transform.localPosition = new Vector3(0, 1, 1.0f);

            //实例化攻击范围
            GameObject a = Instantiate(weaponAttack1, transform.position, Quaternion.identity);
            a.transform.parent = this.transform;
            a.transform.localPosition = new Vector3(0, 1, 0);
            var t = a.GetComponent<Weapon1Colider>();
            t.SetAttackTime(attackTime);
        }
        /**********************week9, first weapon 林海力************/

        /****************week9, fouth weapon, 汪至磊**********************************/
        private void SwordAttack()
        {
            tempInstance = Instantiate(sword,
                                       this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward,
                                       weapons[4].rotation) as Rigidbody;
            tempInstance.velocity = 9 * transform.forward;
            Invoke("SwordAttackAnime", 0.1f);
        }
        private void SwordAttackAnime()
        {
            tempInstance.velocity -= 4 * transform.forward;
            if (tempInstance.velocity.magnitude > 7)
            {
                tempInstance.velocity = 0 * transform.forward;
                CancelInvoke("SwordAttackAnime");
            }
            Invoke("SwordAttackAnime", 0.1f);
        }
        /**************************forth weapon******************************************/

        /***************************** 汪至磊 begin ************************/
        public void Death()
        {
            //**灰屏
            baw.setDeath();
            isAlive = false;

            //**倒下,先停1s,用2s分段倒下
            rotatedtimes = 0;
            InvokeRepeating("DeathRotate", 1f, (float)(DeathContinueTime / DeathRotateTimes));

            //**倒计时
            counttime = DeathTime;
            InvokeRepeating("CountDown", 0f, 1.0f);
        }

        //**大风车吱呀吱悠悠的转~
        private void DeathRotate()
        {
            //Debug.Log("time = "+Time.time);
            transform.Rotate(90 / DeathRotateTimes, 0, 0);
            rotatedtimes++;
            if (rotatedtimes >= DeathRotateTimes)
            {
                CancelInvoke("DeathRotate");
            }
        }

        //**倒计时
        private void CountDown()
        {
            if (counttime == 0)  //复活
            {
                counter.text = "";
                CancelInvoke("CountDown");
                Invoke("Relive", 1f);
            }
            else
            {
                counter.text = "剩余时间:" + counttime;
                counttime = counttime - 1;
            }
        }

        /// <summary>
        /// 在人物模型上显示或消失某一个武器
        /// index为武器索引,ishold表示显示或不显示武器
        /// 武器没有碰撞器,因为攻击不使用碰撞实现
        /// </summary>
        /// <param name="index"></param>
        /// <param name="ishold"></param>
        public void HoldWeapon(int index, bool ishold)
        {   //未持有武器时可以拿一种武器
            if (ishold && !isHoldWeapon)
            {
                holdWeaponIndex = index;
                isHoldWeapon = true;
                weapons[index].gameObject.SetActive(true);
            }
            //持有武器时才能用掉一种武器
            else if (!ishold && isHoldWeapon)
            {
                isHoldWeapon = false;
                weapons[holdWeaponIndex].gameObject.SetActive(false);
            }
        }
        /***************************** 汪至磊 end ***********************/

        /************************** 林海力 begin *************************/
        /// <summary>
        /// 复活函数，位置更改-修改vector3
        /// </summary>
        public void Relive()
        {
            //Debug.Log("relive time = " + Time.time);
            baw.setLive();

            isAlive = true;
            var reliver = Random.Range(-180.0f, 180.0f);
            transform.rotation = Quaternion.Euler(0, reliver, 0);
            /*
            var relivex = Random.Range(-40.0f, 40.0f);
            var relivez = Random.Range(-40.0f, 40.0f);
            transform.position = new Vector3(relivex, 1, relivez);
            */
            transform.position = new Vector3(8.0f, 3.0f, 0f);

            //relive-protect
            protecttime = protecttimeMAX;
            if (protecttime <= 0)
            {
                isJustAlive = false;
            }
            else
            {
                isJustAlive = true;
                InvokeRepeating("ProtectDecrease", 0f, 1.0f);
            }
        }
        private void ProtectDecrease()
        {
            if (protecttime == 0)
            {
                isJustAlive = false;
                CancelInvoke("ProtectDecrease");
            }
            else
            {
                protecttime--;
            }
        }

        /// <summary>
        /// 判断人物现在是否持有武器，若无则拾取武器
        /// </summary>
        public bool TakeWeapon(int weaponstatus)
        {
            bool temp = isHoldWeapon;
            if (weaponstatus > 0)
            {
                HoldWeapon(weaponstatus, true);
            }
            return temp;
        }
        /**************************** 林海力 end **********************/

        // Start is called before the first frame update
        void Start()
        {
            cc = GetComponent<CharacterController>();
            baw = GameObject.FindObjectOfType<BlackAndWhite>();
            //weapons = weaponObject.GetComponentsInChildren<Transform>();
            //foreach (Transform child in weapons) child.gameObject.SetActive(false);
            for (int i = 1; i <= weaponKinds; i++)
            {
                //Debug.Log(weapons[i].name);
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
