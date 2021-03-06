﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCooling : MonoBehaviour
{
    // 技能skill的button
    public Button skillBtn;
    // 文本显示冷却剩余时间
    private Text timeLast;
    // 设定的技能冷却时间
    public int time = 45;
    // 剩余时间
    private int coolingTime;
    //玩家对象
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        // 获取按钮中文本显示组件
        timeLast = skillBtn.transform.Find("Text").GetComponent<Text>();
        timeLast.text = "R";
        // 对于按钮onClick事件指定函数
        skillBtn.onClick.AddListener(OnClickBtn);
        // 剩余时间等于设定时间
        coolingTime = time;
        //获取对应玩家
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // 点击事件的函数，点击后开启协程调用DownTimer()
    void OnClickBtn()
    {
        if (player.GetComponent<PlayerCharacter>().isAlive)
        {
            StartCoroutine(DownTimer());
        }
    }

    // 倒计时协程
    IEnumerator DownTimer()
    {
        while (coolingTime > 0)
        {
            // 倒计时时按钮不能用
            skillBtn.enabled = false;
            // 显示倒计时的文本
            timeLast.text = coolingTime.ToString();
            // 按钮显示改变
            skillBtn.transform.GetComponentInChildren<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            
            // 间隔1S 可设置间隔秒数
            yield return new WaitForSeconds(1);
            // 倒计时剩余时间-1
            coolingTime--;
    
        }
        // 倒计时结束，按钮启用
        skillBtn.enabled = true;
        // 显示文本恢复为R
        timeLast.text = "R";
        // 恢复倒计时时间为设定时间
        coolingTime = time;

        skillBtn.transform.GetComponentInChildren<Image>().color = new Color(1.0f, 1.0f, 1.0f);
    }

    public void Click()
    {
        if (player.GetComponent<PlayerCharacter>().isAlive)
        {
            Debug.Log("[SkillColling:Click()] Skill button clicked, using selected skill");
            player.GetComponent<CharacterBehavior>().SkillTrigger();
        }
    }
}
