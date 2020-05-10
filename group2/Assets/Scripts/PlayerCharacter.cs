using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.GlassBlade.Group2
{
    public class PlayerCharacter : MonoBehaviour
    {
    /* 状态变量集 */
        //无敌变量
        public bool invincible = false;

        //死生状态
        public bool isAlive = true;

        //保护状态标识
        public bool isProtected = false;

        //攻击中
        public bool attacking = false;

        //是否持有武器
        private bool isHoldWeapon = false;

    /* 时间变量集 */
        //攻击持续时间
        public float attackTime;

        //双手斧前摇
        public float Axe2handDelayTime = 1.0f;

        //匕首攻击前摇时间
        public float daggerAttackDelayTime = 1.0f;

        //双手剑攻击前摇时间
        public float swordTwoHandedDelayTime = 1.0f;

        //单手剑攻击前摇时间
        public float swordAttackDelayTime = 1.0f;

        //目前复活保护时长
        private int protecttime;

        //最大复活保护时长
        public int protecttimeMAX = 3;

        //死亡倒地时间
        public double deathContinueTime = 0.5;

        //复活时间
        public int deathTimeCount = 10;

    /* 武器集 */
        //默认武器
        public Rigidbody weapon;

        //双手斧
        public Rigidbody Axe;

        //匕首
        public Rigidbody dagger;

        //双手剑
        public Rigidbody swordTwoHanded;

        //单手剑
        public Rigidbody sword;

        //默认武器的实例变量
        private Rigidbody weaponInstance;

        //双手斧实例
        private Rigidbody axesetInstance;

        //匕首实例
        private Rigidbody daggerInstance;

        //双手剑实例
        private Rigidbody swordTwoHandedInstance;

        //单手剑实例
        private Rigidbody swordInstance;

    /* 攻击范围集 */
        //武器双手斧攻击范围
        public GameObject weaponAttack1;

        //匕首攻击距离调节
        public float daggerAttackDistance = 6.0f;

        //双手剑攻击判定扇形角度
        public float angle = 120f;

        //双手剑攻击判定扇形半径
        public float radius = 4f;

        //单手剑攻击距离调节
        public float swordAttackDistance = 2.0f;

    /* 死亡相关变量集 */
        //黑白渲染组件
        BlackAndWhite baw;

        //死亡倒计时组件
        //counter 挂 Main Camera\Canvas\Text
        public Text counter;

        //死亡倒地段数
        public int deathRotateTimes = 10;

        //目前倒下计数段数
        private int rotatedtimes;

        //复活倒计时
        private int counttime;

    /* 音效 */
        public AudioSource invincibleVoice;
        public AudioSource WeaponDefaultAttack;
        public AudioSource daggerHit;
        public AudioSource daggerAttack;
        public AudioSource swordTwoHandedAttack;
        public AudioSource swordAttack;
        public AudioSource deathVoice;
        public AudioSource axeAttack;
        public AudioSource reliveAS;
        public AudioSource takeWeaponAS;

    /* 其它 */
        private CharacterController cc;

        //击杀数
        public int killTime = 0;

        //被击杀数
        public int deathTime = 0;

        //持有武器种类
        public int holdWeaponIndex = 0;

        //已实装武器种类
        public int weaponKinds = 4;

        //人物下的武器模型,0号位闲置
        public Transform[] weapons;

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
                takeWeaponAS.Play();
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

        /// <summary>
        /// Attack 攻击函数，在按下攻击键时被调用，有四种外加一种默认攻击方式
        /// </summary>
        public void Attack()
        {
            if (!isAlive || attacking)
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
                WeaponDefaultAttack.Play();
                weaponInstance = Instantiate(weapon, new Vector3(0, 1, 0.5f), this.transform.rotation) as Rigidbody;
                weaponInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
                weaponInstance.transform.SetParent(this.transform, false);
                float tempTime = weaponInstance.gameObject.GetComponent<DestroyWeapon>().destroyTime;
                // 武器旋转表示攻击动作
                weaponInstance.angularVelocity = this.transform.right * 1 * (2.1f / tempTime);
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
        /// 双手斧实例化武器，武器仅有动画；攻击由范围碰撞来实现
        /// </summary>
        public void Axe2HandAttack()
        {
            axeAttack.Play();
            //实例化武器
            axesetInstance = Instantiate(Axe, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, this.transform.rotation) as Rigidbody;
            axesetInstance.transform.SetParent(this.transform, false);
            axesetInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 90));
            axesetInstance.transform.localPosition = new Vector3(0, 1, 1.0f);

            //实例化攻击范围
            GameObject a = Instantiate(weaponAttack1, transform.position, Quaternion.identity);
            a.transform.parent = this.transform;
            a.transform.localPosition = new Vector3(0, 1, 0);
            var t = a.GetComponent<Weapon1Colider>();
            t.SetAttackTime(attackTime);
        }

        /// <summary>
        /// DaggerAttack 实现匕首攻击动画和攻击判定
        /// 攻击方式为匕首飞出，采用碰撞判定有效攻击
        /// </summary>
        private void DaggerAttack()
        {
            daggerAttack.Play();
            daggerInstance = Instantiate(dagger, this.transform.localPosition, weapons[2].rotation) as Rigidbody;
            daggerInstance.transform.SetParent(this.transform, false);
            daggerInstance.transform.localPosition = new Vector3(0, 1, 1);
            daggerInstance.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            daggerInstance.velocity = daggerAttackDistance / daggerInstance.GetComponent<DestroyWeapon>().destroyTime * transform.forward;
        }

        /// <summary>
        /// SwordTwoHandedAttack 实现双手剑攻击动画和攻击判定
        /// 攻击方式为扇形横扫，采用查询扇形区域内玩家对象判定有效攻击
        /// </summary>
        private void SwordTwoHandedAttack()
        {
            swordTwoHandedAttack.Play();
            Quaternion qtarget =  this.transform.localRotation;
            swordTwoHandedInstance = Instantiate(swordTwoHanded, this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward, qtarget) as Rigidbody;
            swordTwoHandedInstance.transform.SetParent(this.transform, false);
            swordTwoHandedInstance.transform.localPosition = new Vector3(0, 1, 1.0f);
            swordTwoHandedInstance.transform.localRotation = Quaternion.Euler(new Vector3(0, 90, 90));
            // 获取所有人物
            var gos = GameObject.FindGameObjectsWithTag("Player");

            //逐一判断是否在攻击范围内
            foreach (var go in gos)
            {
                var target = go.GetComponent<PlayerCharacter>();
                if (UmbrellaAttact(this.gameObject.transform, go.transform, angle, radius) && target.isAlive && !target.isProtected && !target.invincible)
                {
                    target.TakeDamage();
                    target.deathTime++;
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

        //动画段数,9段
        //int tempspan;
        /// <summary>
        /// SwordAttack 单手剑攻击实现
        /// </summary>
        void SwordAttack()
        {
            swordAttack.Play();
            swordInstance = Instantiate(sword,
                                       this.transform.localPosition + new Vector3(0, 1, 0) + this.transform.forward,
                                       weapons[4].rotation) as Rigidbody;
            swordInstance.transform.SetParent(this.transform, false);
            swordInstance.transform.localPosition = new Vector3(0, 1, 1);
            swordInstance.transform.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            //tempspan = 0;
            swordInstance.velocity = swordAttackDistance / swordInstance.GetComponent<DestroyWeapon>().destroyTime * transform.forward;
            Invoke("SwordAttackAnime", (0.3f * swordInstance.GetComponent<DestroyWeapon>().destroyTime));
        }

        /// <summary>
        /// SwordAttackAnime 单手剑收回实现
        /// </summary>
        void SwordAttackAnime()
        {
            if (swordInstance)
            {
                swordInstance.velocity = -0.5f * swordAttackDistance / swordInstance.GetComponent<DestroyWeapon>().destroyTime * transform.forward;
                /*
                tempspan++;
                if (tempspan >= 7)
                {
                    tempInstance.velocity = 0 * transform.forward;
                    CancelInvoke("SwordAttackAnime");
                }
                Invoke("SwordAttackAnime", 0.05f);
                */
            }
        }

        /// <summary>
        /// Takedamage 承受攻击函数，受到攻击时调用
        /// </summary>
        public void TakeDamage()
        {
            Debug.Log("damage");
            Death();        
        }

        /// <summary>
        /// Death 死亡函数
        /// </summary>
        private void Death()
        {
            // 播放音效
            deathVoice.Play();

            //**灰屏
            baw.setDeath();
            isAlive = false;

            //**倒下,先停1s,用2s分段倒下
            rotatedtimes = 0;
            InvokeRepeating("DeathRotate", 1f, (float)(deathContinueTime / deathRotateTimes));

            //**倒计时
            counttime = deathTimeCount;
            InvokeRepeating("CountDown", 0f, 1.0f);
        }

        /// <summary>
        /// DeathRotate 死亡时倒地
        /// </summary>
        private void DeathRotate()
        {
            //Debug.Log("time = "+Time.time);
            transform.Rotate(90 / deathRotateTimes, 0, 0);
            rotatedtimes++;
            if (rotatedtimes >= deathRotateTimes)
            {
                CancelInvoke("DeathRotate");
            }
        }

        /// <summary>
        /// CountDown 死亡倒地时的旋转
        /// </summary>
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
        /// 复活函数，位置更改-修改vector3
        /// </summary>
        public void Relive()
        {
            //Debug.Log("relive time = " + Time.time);
            baw.setLive();
            reliveAS.Play();
            isHoldWeapon = false;
            if(holdWeaponIndex > 0)
            {
                weapons[holdWeaponIndex].gameObject.SetActive(false);
            }          
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
                isProtected = false;
            }
            else
            {
                isProtected = true;
                InvokeRepeating("ProtectDecrease", 0f, 1.0f);
            }
        }

        /// <summary>
        /// ProtectDecrease 复活保护阶段倒计时
        /// </summary>
        private void ProtectDecrease()
        {
            if (protecttime == 0)
            {
                isProtected = false;
                CancelInvoke("ProtectDecrease");
            }
            else
            {
                protecttime--;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            cc = GetComponent<CharacterController>();
            baw = GameObject.FindObjectOfType<BlackAndWhite>();
            for (int i = 1; i <= weaponKinds; i++)
            {
                //Debug.Log(weapons[i].name);
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
}
