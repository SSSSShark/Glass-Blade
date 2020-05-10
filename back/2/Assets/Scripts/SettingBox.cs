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
    #region public Fields

    public GameObject settingstore;
    public GameObject panel;
    public GameObject messageBox;
    public Button ok;
    public Button cancel;
    public Dropdown modedpn;
    public Toggle friendlyFireTog;
    public InputField timeText;
    #endregion

    #region Setting Results

    public GameMode mode = GameMode.Normal;
    private GameMode tmpmode;
    public bool friendlyFire = false;
    private bool tmpfriendlyFire;
    public string setTime = "8:00";
    private string tmpsetTime;
    #endregion
    // Use this for initialization
    void Start()
    {
        panel = GameObject.Find("Setting");
        messageBox = gameObject;
        ok = messageBox.transform.Find("Ok").GetComponent<Button>();
        cancel = messageBox.transform.Find("Cancel").GetComponent<Button>();
        //timeText = messageBox.transform.Find("TimeInputField").GetComponent<InputField>();

        ok.onClick.AddListener(OnClickOk);
        cancel.onClick.AddListener(OnClickCancel);
        modedpn.onValueChanged.AddListener(DropDownSelect);

        tmpmode = mode;
        tmpfriendlyFire = friendlyFire;
        tmpsetTime = setTime;

        settingstore.GetComponent<SettingStore>().setTime = setTime;
        settingstore.GetComponent<SettingStore>().frienlyFire = friendlyFire;
        panel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

    }
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
        tmpsetTime = timeText.text;
        settingstore.GetComponent<SettingStore>().frienlyFire = friendlyFire;       
        if (Regex.IsMatch(tmpsetTime, @"\d:[0-5]?\d$"))
        {
            setTime = tmpsetTime;
            settingstore.GetComponent<SettingStore>().setTime = setTime;
            Debug.Log("Set time success");
        }
        else
        {
            Debug.LogWarning("Set time failed: not in correct format");
        }
        panel.SetActive(false);
    }
    public void OnClickCancel()
    {
        //modedpn.ClearOptions();
        //modedpn.AddOptions(list);
        modedpn.value = (int)mode;  //恢复当前的状态
        friendlyFireTog.isOn = friendlyFire;
        timeText.text = setTime;

        panel.SetActive(false);
    }

    public void DropDownSelect(int value)
    {
        switch ((GameMode)value)
        {
            case GameMode.Normal:
                tmpmode = GameMode.Normal;
                Debug.Log("select: Normal");
                break;
            case GameMode.Occupation:
                tmpmode = GameMode.Occupation;
                Debug.Log("select: Occupation");
                break;
        }
    }
    public void OnToggleValueChange()
    {
        tmpfriendlyFire = friendlyFireTog.isOn;
        Debug.Log("friendlyfire: " + friendlyFireTog.isOn);
    }
}
