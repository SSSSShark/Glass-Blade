﻿//Author: David Wang
//Function: For group2: When a player dies, his opponent score increases by 1, shown on the scoreboard
//Todo: After doing what is mentioned in CountDown.cs, add this script to Score A & Score B
//Notion: UpdateScore function is to be called
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Glassblade.Group1
{
    public class NScoreIncrease : MonoBehaviour
    {
        //得分
        public int score = 0;
        //分数文本
        public Text scoreText;

        void Start()
        {
            //获取文本组件
            scoreText = GetComponentInChildren<Text>();
        }

        /// <summary>
        /// 死亡时调用
        /// </summary>
        public void UpdateScore()
        {
            //分数+1
            score++;

            //设置分数文本
            scoreText.text = score.ToString();

        }
    }
}