using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlChatRoom : MonoBehaviourPun
{

    public TMP_InputField chatInput;
    public TextMeshProUGUI chatText;
    public ScrollRect scrollRect;
    public GameObject btnSend;

    public bool IsVisible = true;
    public List<string> messages = new List<string>();
    private string inputLine = "";

    // Use this for initialization
    void Start()
    {
        FindObject();
        gameObject.SetActive(false);
    }

    public void FindObject()
    {
        if (!chatInput)
        {
            chatInput = GameObject.Find("ChatInputField").GetComponent<TMP_InputField>();
        }
        if (!btnSend)
        {
            btnSend = GameObject.Find("ChatSendButton");
        }
        if (!chatText)
        {
            chatText = GameObject.Find("ChatText").GetComponent<TextMeshProUGUI>();
        }
        if (btnSend)
        {
            btnSend.GetComponent<Button>().onClick.AddListener(
                delegate { SendContent(); }
            );
        }
    }

    //发送聊天内容
    public void SendContent()
    {
        if (!this.IsVisible || !PhotonNetwork.InRoom)
        {
            return;
        }
        //获取聊天框内容
        inputLine = chatInput.text;
        Debug.Log("Sending Content");

        if (!string.IsNullOrEmpty(this.inputLine))
        {
            chatText.text = "";
            //使用photonView.RPC发送消息
            this.photonView.RPC("Chat", RpcTarget.All, this.inputLine);
            this.inputLine = "";
            chatInput.text = "";
            chatInput.ActivateInputField();
        }
        else
        {
            return;
        }

    }

    //系统消息
    [PunRPC]
    public void SystemInfo(string newLine)
    {
        Debug.Log("Receving content");
        chatText.text = "";

        this.messages.Add("<color=#B8860B>" + newLine + "</color> ");

        //只显示最新的24条消息
        List<string> newmessages = new List<string>();
        if (messages.Count > 24)
        {
            for (int i = (messages.Count - 24); i < messages.Count; i++)
            {
                newmessages.Add(messages[i]);
            }

            for (int i = 0; i < newmessages.Count; i++)
            {
                chatText.text += newmessages[i] + "\n";
            }
        }
        else
        {
            for (int i = 0; i < messages.Count; i++)
            {
                chatText.text += messages[i] + "\n";
            }
        }

        //复位
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();

    }

    //接收方收取消息
    [PunRPC]
    public void Chat(string newLine, PhotonMessageInfo mi)
    {
        string senderName = "anonymous";
        chatText.text = "";
        if (mi.Sender != null)
        {
            if (!string.IsNullOrEmpty(mi.Sender.NickName))
            {
                senderName = mi.Sender.NickName;
            }
            else
            {
                senderName = "player " + mi.Sender.UserId;
            }
        }

        this.messages.Add("<color=#EEAD0E>" + senderName + ":</color> " + newLine);


        //只显示最新的24条消息
        List<string> newmessages = new List<string>();
        if (messages.Count > 24)
        {
            for (int i = (messages.Count - 24); i < messages.Count; i++)
            {
                newmessages.Add(messages[i]);
            }

            for (int i = 0; i < newmessages.Count; i++)
            {
                chatText.text += newmessages[i] + "\n";
            }
        }
        else
        {
            for (int i = 0; i < messages.Count; i++)
            {
                chatText.text += messages[i] + "\n";
            }
        }

        //复位
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();

    }


    public void ShowChatPanel()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
            this.IsVisible = false;
        }
        else
        {
            gameObject.SetActive(true);
            this.IsVisible = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SendContent();
        }

    }

}
