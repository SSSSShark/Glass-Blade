// Author: David Wang
// Function: 显示夺取倒计时，倒计时结束夺取成功，显示"✔"
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
namespace Com.Glassblade.Group1
{
    public class CircleCountDownDisplay : MonoBehaviour, IPunObservable
    {
        // 调用占点模式
        public OMode OM;
        // 最大时间
        private float max;
        // 当前时间
        private float current;
        // 占领的队伍
        public TeamController.Team tempTeam;
        // 占领结果
        public TeamController.Team teamResult;
        // 整个倒计时牌
        public RectTransform CircleCountDown;
        // 提示文字，倒计时时间
        public Transform Indicator;
        // 外圈
        public Transform Circle;
        [HideInInspector]
        public String TextHint;
        [HideInInspector]
        public Color ColorHint;
        [HideInInspector]
        public Color TextColorHint;

        private float[] color = new float[4];
        private Transform camPosition;
        // 占点在相机中的位置
        private Vector3 viewPos;
        // 相机到点的向量
        private Vector3 camToDot;
        // 初始化赋值
        void Start()
        {
            max = (float)OM.occupyCountDown;
            current = max;
            tempTeam = TeamController.Team.unknown;
            teamResult = TeamController.Team.unknown;
            //GetComponent<CanvasGroup>().alpha = 0;
            TextHint=Indicator.GetComponent<Text>().text;
            ColorHint= Circle.GetComponent<Image>().color ;
            TextColorHint= Indicator.GetComponent<Text>().color ;
        }

        // 
        void Update()
        {
            current = OM.currentTime;
            tempTeam = OM.tempTeam;
            teamResult = OM.team;

            // David Wang add 占点可视
            // 获取相机位置
            camPosition = Camera.main.transform;
            int window_h = UnityEngine.Screen.height;
            int window_w = UnityEngine.Screen.width;
            Vector3 center = new Vector3(window_w / 2, window_h / 2, 1);
            // 将占点的点三维坐标转化成屏幕坐标
            viewPos = Camera.main.WorldToViewportPoint(OM.transform.position);
            if (viewPos.z >= 0)
                viewPos = new Vector3(viewPos.x * window_w, viewPos.y * window_h, 1);
            else
                viewPos = new Vector3(-viewPos.x * window_w, -viewPos.y * window_h, 1);
           // Debug.Log(viewPos);
            // 计算相机到点的向量值
            // 点在屏幕内
            // 如果点在屏幕内，且位置足以放下倒计时圆盘
            
            int circle_radius = (int)(CircleCountDown.rect.width/2)+1;
            Vector3 tepvec3 = new Vector3();
            if (viewPos.x >= circle_radius&& viewPos.x <= window_w - circle_radius&& viewPos.y >= circle_radius && viewPos.y<= window_h - circle_radius)
            {
                tepvec3 = viewPos;
            }
            else
            {
                Vector3 t = (viewPos - center) / Math.Abs((viewPos - center).x) * (window_w / 2 - circle_radius);
                if (Math.Abs(t.y)<= window_h/2 - circle_radius)
                {
                    tepvec3 = t+center;
                }
                else
                {
                    t = (viewPos - center) / Math.Abs((viewPos - center).y) * (window_h / 2 - circle_radius);
                    tepvec3 = t+center;
                }
            }
            tepvec3.z = 0; 
            
            CircleCountDown.position = tepvec3;
         
            if (PhotonNetwork.IsMasterClient)
            {
                //if (Indicator.GetComponent<Text>().text == "✔")
                //{
                //    return;
                //}
                current = OM.currentTime;
                tempTeam = OM.tempTeam;
                teamResult = OM.team;
                // 0队用红色标注
                if (tempTeam == TeamController.Team.TeamA)
                {
                    if (tempTeam != teamResult)
                    {
                        GetComponent<CanvasGroup>().alpha = 1;
                        Circle.GetComponent<Image>().fillAmount = current / max;
                        Circle.GetComponent<Image>().color = Color.red;
                        Indicator.GetComponent<Text>().text = ((int)current).ToString();
                        Indicator.GetComponent<Text>().color = Color.red;
                    }
                    // 倒计时完成，占领或者标注该点已经被玩家占领
                    if (current <= 0)
                    {
                        GetComponent<CanvasGroup>().alpha = 1;
                        Indicator.GetComponent<Text>().text = "✔";
                    }
                }
                // 1队用蓝色标注
                if (tempTeam == TeamController.Team.TeamB)
                {
                    if (tempTeam != teamResult)
                    {
                        GetComponent<CanvasGroup>().alpha = 1;
                        Circle.GetComponent<Image>().fillAmount = current / max;
                        Circle.GetComponent<Image>().color = Color.blue;
                        Indicator.GetComponent<Text>().text = ((int)current).ToString();
                        Indicator.GetComponent<Text>().color = Color.blue;
                    }
                    // 倒计时完成，占领或者标注该点已经被玩家占领
                    if (current <= 0)
                    {
                        GetComponent<CanvasGroup>().alpha = 1;
                        Indicator.GetComponent<Text>().text = "✔";
                    }
                }
                if (tempTeam == TeamController.Team.unknown)
                {
                    GetComponent<CanvasGroup>().alpha = 1;
                    Circle.GetComponent<Image>().fillAmount = 1;
                    Circle.GetComponent<Image>().color =ColorHint;
                    Indicator.GetComponent<Text>().text = TextHint;
                    Indicator.GetComponent<Text>().color =TextColorHint;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(GetComponent<CanvasGroup>().alpha);
                stream.SendNext(Circle.GetComponent<Image>().fillAmount);

                // color类型无法被序列化
                SetColor(Circle.GetComponent<Image>().color);
                stream.SendNext(color);

                stream.SendNext(Indicator.GetComponent<Text>().text);

                SetColor(Indicator.GetComponent<Text>().color);
                stream.SendNext(color);
            }
            else
            {
                this.GetComponent<CanvasGroup>().alpha = (float)stream.ReceiveNext();
                this.Circle.GetComponent<Image>().fillAmount = (float)stream.ReceiveNext();
   
                this.Circle.GetComponent<Image>().color = GetColor((float[])stream.ReceiveNext());

                this.Indicator.GetComponent<Text>().text = (string)stream.ReceiveNext();

                this.Indicator.GetComponent<Text>().color = GetColor((float[])stream.ReceiveNext());
            }
        }
        public Color GetColor(float[] color)
        {
            return new Color(color[0], color[1], color[2], color[3]);
        }

        public void SetColor(Color c)
        {
            color[0] = c.r;
            color[1] = c.g;
            color[2] = c.b;
            color[3] = c.a;
        }
    }
}
