//Author: David Wang
//Function: For group2: When a player dies, his opponent score increases by 1, shown on the scoreboard
//Todo: After doing what is mentioned in CountDown.cs, add this script to Score A & Score B
//Notion: UpdateScore function is to be called
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreIncrease : MonoBehaviour
{
    public int score = 0;           //得分
    public Text scoreText;          //分数文本
    
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();     //获取文本组件
    }

    //死亡时调用
    public void UpdateScore()
    {
        score ++;                   //分数+1
        if(score >= 10)
        {
            scoreText.text = score.ToString();          //设置分数文本
        }
        else
        {
            scoreText.text = "0" + score.ToString();    //设置分数文本
        }
    }
}
