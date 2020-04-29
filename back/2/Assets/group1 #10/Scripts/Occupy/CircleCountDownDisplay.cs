// Author: David Wang
// Function: 显示夺取倒计时，倒计时结束夺取成功，显示"✔"
using System.Collections;
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
        //public Transform CircleCountDown;
        // 提示文字，倒计时时间
        public Transform Indicator;
        // 外圈
        public Transform Circle;

        private float[] color = new float[4];

        // 初始化赋值
        void Start()
        {
            max = (float)OM.occupyCountDown;
            current = max;
            tempTeam = TeamController.Team.unknown;
            teamResult = TeamController.Team.unknown;
            GetComponent<CanvasGroup>().alpha = 0;
        }

        // 
        void Update()
        {
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
                    GetComponent<CanvasGroup>().alpha = 0;
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
