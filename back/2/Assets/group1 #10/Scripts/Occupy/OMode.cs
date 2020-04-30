// Author: David Wang
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using Photon.Realtime;

namespace Com.Glassblade.Group1
{
    public class OMode : MonoBehaviourPun
    {
        // 占据队伍结果 null, 0, 1
        public TeamController.Team team;
        // 暂时占据该点的队伍
        public TeamController.Team tempTeam;
        // 玩家列表
        ArrayList Players;
        // 计数玩家数量，先入为主
        public int hostPlayer;
        // 计数玩家数量，后入为客
        public int guidePlayer;
        // 圈内玩家总人数
        public int playerCnt;

        // 占领倒计时,留为可调时间
        public int occupyCountDown;
        // 是否因为敌方玩家进入而暂停倒计时
        private bool stoped;
        // 倒计时中间时间
        public float currentTime;
        //add by wmj
        //占领加分幅度
        public int scoreInc;
        // private UnityAction<PlayerCharacter> playerDeadAction;

        //public void PlayerDead(PlayerCharacter player)
        //{
        //    OnTriggerExit(player.GetComponent<Collider>());
        //}
        //wmj ends

        /// <summary>
        /// 给变量做初始化
        /// </summary>
        void Start()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                // 起初占领结果为空
                team = TeamController.Team.unknown;
                // 占领倾向为空
                tempTeam = TeamController.Team.unknown;
                // 初始化玩家列表
                Players = new ArrayList();
                // 初始化主玩家数量
                hostPlayer = 0;
                // 初始化客（敌对）玩家数量
                guidePlayer = 0;
                // 初始化圈内玩家总数
                playerCnt = 0;
                // 刚开始默认暂停
                stoped = true;
                // 倒计时时间等于设定好的占领倒计时
                currentTime = (float)occupyCountDown;
                //add by wmj
                //init dict
                //playerDeadAction = new UnityAction<PlayerCharacter>(PlayerDead);
                //wmj ends
            }
        }

        /// <summary>
        /// Update作为占领倒计时，倒计时结束则成功占领该点
        /// </summary>
        void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                playerCnt = hostPlayer + guidePlayer;
                if (!stoped)
                {
                    if (currentTime > 0)
                    {
                        currentTime -= Time.deltaTime;
                    }
                    else
                    {
                        team = tempTeam;
                        if (currentTime < 0)
                        { 
                            //add by wmj,modify by Via Cytus
                            //这时点里玩家应全加分
                            foreach (PlayerCharacter p in Players)
                            {
                                if((TeamController.Team)p.photonView.Controller.CustomProperties["team"] == team)
                                {
                                    p.CallUpdateScore(p.photonView.Owner, scoreInc);
                                    //p.score += scoreInc;
                                }
                            }
                            //wmj ends
                        }
                        currentTime = 0;
                        // 倒计时结束，占领
                        
                    }
                }
            }
        }

        /// <summary>
        /// 进入触发盒，表示有玩家进入该点区域，做相应判别：首先该点是否未被本方占领，
        /// 且没有其他玩家在该区域内，是则开始倒计时。若该区域内已经有其他玩家，
        /// 判断是本方人还是对方人，本方人没有任何影响，对方人则暂停倒计时。
        /// 如果进入该点时已经被本方占领，则无影响
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            // 获取玩家
            PlayerCharacter player = other.GetComponent<PlayerCharacter>();
            // Capsule 范围内有玩家
            if (player)
            {
                // 玩家是活着的
                if (player.isAlive)
                {
                    player.OM = this;
                    if (PhotonNetwork.IsMasterClient)
                    {
                        var playerTeam = (TeamController.Team)player.photonView.Controller.CustomProperties["team"];
                        //add by wmj
                        //注册action
                        //status.deathevent.AddListener(playerDeadAction);

                        //wmj ends

                        // 玩家进入了未被占领或者被地方占领的点
                        if (team != playerTeam)
                        {
                            // 占领点内没有玩家
                            if (hostPlayer == 0 && guidePlayer == 0)
                            {
                                // 占领点内主玩家数++
                                hostPlayer++;
                                // 占领点有被主队占领的倾向
                                tempTeam = playerTeam;
                                // 开始计时
                                stoped = false;
                            }
                            else
                            {
                                // 同队玩家进入无影响
                                if (tempTeam == playerTeam)
                                {
                                    hostPlayer++;
                                }
                                // 敌方玩家进入，倒计时暂停
                                else
                                {
                                    guidePlayer++;
                                    stoped = true;
                                }
                            }
                        }
                        // 玩家进入了己方已经占领的点，无影响
                        else
                        {
                            // 玩家进入了自己方占领状态但敌方正在夺取的点
                            if ((tempTeam != TeamController.Team.unknown) && (playerTeam != tempTeam))
                            {
                                // 此时对于该点来说算作敌方
                                guidePlayer++;
                                stoped = true;
                            }
                            // 玩家进入了正常状态下己方占领的点
                            else
                            {
                                hostPlayer++;
                                tempTeam = playerTeam;
                            }
                        }
                        // 加入玩家列表
                        if (!Players.Contains(player))
                        {
                            Players.Add(player);
                        }
                    }
                }

            }

        }

        public void DeathEvent(PlayerCharacter player)
        {
            OnTriggerExit(player.GetComponent<Collider>());
        }

        /// <summary>
        /// 玩家离开占点区域的触发器，离开的玩家是本方玩家，若本方玩家全部离开，
        /// 则判断是否有对方玩家，有则重新开始计时。没有则占领倾向为空。
        /// 如果对方玩家全部离开则恢复倒计时
        /// </summary>
        private void OnTriggerExit(Collider other)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CharacterBehavior player = other.GetComponent<CharacterBehavior>();
                PlayerCharacter status = other.GetComponent<PlayerCharacter>();

                if (player)
                {
                    //remove by wmj
                    //死人要通过这个函数移出点
                    //if (status.isAlive)
                    //wmj ends
                    var playerTeam = (TeamController.Team)status.photonView.Controller.CustomProperties["team"];
                    {
                        //add by wmj
                        //移除action
                        //status.deathevent.RemoveListener(playerDeadAction);
                        status.OM = null;
                        //wmj ends
                        Players.Remove(player);
                        // 离开的是本方玩家
                        if (playerTeam == tempTeam)
                        {
                            hostPlayer--;
                            // 本方玩家全部走掉
                            if (hostPlayer == 0)
                            {
                                // 对方开始变为主
                                hostPlayer = guidePlayer;
                                currentTime = (float)occupyCountDown;
                                stoped = true;
                                // 对方玩家也全部走掉
                                if (guidePlayer == 0)
                                {
                                    // 占领倾向为空
                                    tempTeam = TeamController.Team.unknown;
                                }
                                // 对方玩家没走
                                else if (guidePlayer > 0)
                                {
                                    // 对方表现占领倾向
                                    if (playerTeam == TeamController.Team.TeamA)
                                    {
                                        tempTeam = TeamController.Team.TeamB;
                                    }
                                    else
                                    {
                                        tempTeam = TeamController.Team.TeamA;
                                    }
                                    // 开始倒计时占领
                                    stoped = false;
                                }
                                guidePlayer = 0;
                            }
                            // 本方玩家还有留在区域内的
                            else
                            {
                                tempTeam = playerTeam;
                            }
                        }
                        // 对方玩家离开
                        else
                        {
                            guidePlayer--;
                            // 对方玩家全部离开
                            if (guidePlayer == 0)
                            {
                                stoped = false;
                            }
                        }
                    }
                }
            }
        }

    }
}

