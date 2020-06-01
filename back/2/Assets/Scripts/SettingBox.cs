using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Reflection.Emit;
using System.Linq;
using System.Text.RegularExpressions;

public class SettingBox : MonoBehaviour
{
    public enum GameMode
    {
        Normal = 0,
        Occupation = 1
    }
    #region Public Fields

    public GameObject settingstore;
    public GameObject panel;
    public GameObject messageBox;
    public GameObject timeUpbtn;
    public GameObject timeDownbtn;
    public Button ok;
    public Button cancel;
    public Dropdown modedpn;
    public Toggle friendlyFireTog;
    public Text timeText;
    public Image timeTextImg;

    #endregion

    #region Setting Results

    public GameMode mode = GameMode.Normal;
    private GameMode tmpmode;
    public bool friendlyFire = false;
    private bool tmpfriendlyFire;
    public int setTime;
    private int tmpsetTime;

    #endregion
    // Use this for initialization
    void Start()
    {
        panel = GameObject.Find("Setting");
        messageBox = gameObject;
        ok = messageBox.transform.Find("Ok").GetComponent<Button>();
        cancel = messageBox.transform.Find("Cancel").GetComponent<Button>();

        ok.onClick.AddListener(OnClickOk);
        cancel.onClick.AddListener(OnClickCancel);
        modedpn.onValueChanged.AddListener(DropDownSelect);
        tmpsetTime = int.Parse(timeText.text);

        tmpmode = mode;
        tmpfriendlyFire = friendlyFire;
        setTime=tmpsetTime;

        settingstore.GetComponent<SettingStore>().setTime = setTime;
        settingstore.GetComponent<SettingStore>().friendlyFire = friendlyFire;
        panel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region Public Methods

    /// <summary>
    /// 显示dialog
    /// </summary>
    public void ShowDialog()
    {
        panel.SetActive(true);
    }
    /// <summary>
    /// 关闭dialog
    /// </summary>
    public void ExitDialog()
    {
        /*do nothing*/
        panel.SetActive(false);
    }

    public void OnClickOk()
    {
        mode = tmpmode;
        friendlyFire = tmpfriendlyFire;
        setTime = tmpsetTime;
        settingstore.GetComponent<SettingStore>().friendlyFire = friendlyFire;
        settingstore.GetComponent<SettingStore>().setTime = setTime;

        panel.SetActive(false);
    }
    public void OnClickCancel()
    {
        //modedpn.ClearOptions();
        //modedpn.AddOptions(list);
        modedpn.value = (int)mode;  //恢复当前的状态
        friendlyFireTog.isOn = friendlyFire;
        timeText.text = setTime.ToString();

        panel.SetActive(false);
    }

    public void DropDownSelect(int value)
    {
        switch ((GameMode)value)
        {
            case GameMode.Normal:
                tmpmode = GameMode.Normal;
                timeDownbtn.GetComponent<Button>().enabled = true;
                timeUpbtn.GetComponent<Button>().enabled = true;
                timeDownbtn.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                timeUpbtn.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f);
                timeTextImg.color = new Color(1.0f, 1.0f, 1.0f);
                Debug.Log("[SettingBox:DropDownSelect()] Normal Mode selected.");
                break;
            case GameMode.Occupation:
                tmpmode = GameMode.Occupation;
                timeDownbtn.GetComponent<Button>().enabled = false;
                timeUpbtn.GetComponent<Button>().enabled = false;
                timeDownbtn.GetComponent<Image>().color = new Color(181.0f / 255.0f, 181.0f / 255f, 181.0f / 255.0f);
                timeUpbtn.GetComponent<Image>().color = new Color(181.0f / 255.0f, 181.0f / 255f, 181.0f / 255.0f);
                timeTextImg.color = new Color(181.0f / 255.0f, 181.0f / 255f, 181.0f / 255.0f);
                Debug.Log("[SettingBox:DropDownSelect()] Occupation Mode selected");
                break;
        }
    }
    public void OnToggleValueChange()
    {
        tmpfriendlyFire = friendlyFireTog.isOn;
    }

    public void OnClickTimeUp()
    {
       
        tmpsetTime += 1;
        if (tmpsetTime > 1 && !timeDownbtn.activeInHierarchy)
        {
            timeDownbtn.SetActive(true);
        }
        if (tmpsetTime >= 20)
        {
            timeUpbtn.SetActive(false);
        }
        timeText.text = tmpsetTime.ToString();
    }

    public void OnClickTimeDown()
    {
        tmpsetTime = int.Parse(timeText.text);
        tmpsetTime -= 1;
        if (tmpsetTime < 20 && !timeUpbtn.activeInHierarchy)
        {
            timeUpbtn.SetActive(true);
        }
        if (tmpsetTime <= 1)
        {
            timeDownbtn.SetActive(false);
        }
        timeText.text = tmpsetTime.ToString();
    }

    #endregion
}

