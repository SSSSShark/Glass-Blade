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
    public int score = 0;
    public Text scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void UpdateScore()
    {
        score ++;
        if(score >= 10)
        {
            scoreText.text = score.ToString();
        }
        else
        {
            scoreText.text = "0" + score.ToString();       
        }
    }
}
