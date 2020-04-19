// Author: David Wang
// Function: 显示夺取倒计时，倒计时结束夺取成功，显示"✔"
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Com.Glassblade.Group1
{
    public class CircleCountDownDisplay : MonoBehaviour
    {
        // 调用占点模式
        public OMode OM;
        // 最大时间
        private float max;
        // 当前时间
        private float current;
        // 占领的队伍
        private int? team;
        // 整个倒计时牌
        public Transform CircleCountDown;
        // 提示文字，倒计时时间
        public Transform Indicator;
        // 外圈
        public Transform Circle;

        // 初始化赋值
        void Start()
        {
            max = (float)OM.occupyCountDown;
            current = max;
            team = null;
            CircleCountDown.GetComponent<CanvasGroup>().alpha = 0;
        }

        // 
        void Update()
        {
            current = OM.currentTime;
            team = OM.tempTeam;
            // 0队用红色标注
            if (team == 0)
            {
                CircleCountDown.GetComponent<CanvasGroup>().alpha = 1;
                Circle.GetComponent<Image>().fillAmount = current/max;
                Circle.GetComponent<Image>().color = Color.red;
                Indicator.GetComponent<Text>().text = ((int)current).ToString();
                Indicator.GetComponent<Text>().color = Color.red;
                if (current == 0)
                {
                    Indicator.GetComponent<Text>().text = "✔";
                }
            }
            // 1队用蓝色标注
            if (team == 1)
            {
                CircleCountDown.GetComponent<CanvasGroup>().alpha = 1;
                Circle.GetComponent<Image>().fillAmount = current/max;
                Circle.GetComponent<Image>().color = Color.blue;
                Indicator.GetComponent<Text>().text = ((int)current).ToString();
                Indicator.GetComponent<Text>().color = Color.blue;
                if (current == 0)
                {
                    Indicator.GetComponent<Text>().text = "✔";
                }
            }
            if (team == null)
            {
                CircleCountDown.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }
}
