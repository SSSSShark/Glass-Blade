//Author: wmj


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.Glassblade.Group1
{
    public class OBush : MonoBehaviour
    {
        //相机跟随组件
        OCameraFollow cam;
        //角色列表
        ArrayList players;
        //队友数量
        int allies_cnt;

        void Start()
        {
            //获取相机跟随组价
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OCameraFollow>();
            //实例化角色列表
            players = new ArrayList();
            //队友数量清零
            allies_cnt = 0;
        }

        /// <summary>
        /// 草丛逻辑
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter(Collider other)
        {
            OCharacterBehavior player = other.GetComponent<OCharacterBehavior>();
            if (player)
            {
                //如果是友军
                if (player.team == cam.team)
                {
                    //友军数量为0
                    if (allies_cnt == 0)
                    {
                        //对角色列表中每个人
                        foreach (OCharacterBehavior p in players)
                        {
                            //设置为半透明
                            p.SetTransparent(0.5f);
                        }
                    }
                    //友军数量+1
                    allies_cnt++;
                }
                //新来的是否可见
                player.SetTransparent(allies_cnt == 0 ? 0f : 0.5f);
                //加入角色列表
                players.Add(player);
            }
        }

        /// <summary>
        /// 大致同上
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerExit(Collider other)
        {
            OCharacterBehavior player = other.GetComponent<OCharacterBehavior>();
            if (player)
            {
                players.Remove(player);
                player.SetTransparent(1f);
                if (player.team == cam.team)
                {
                    allies_cnt--;
                    if (allies_cnt == 0)
                    {
                        foreach (OCharacterBehavior p in players)
                        {
                            p.SetTransparent(0f);
                        }
                    }
                }
            }
        }
    }
}