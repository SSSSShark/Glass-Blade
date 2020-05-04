using UnityEngine;
using UnityEngine.UI;
using System.Collections;



public class SettingBox : MonoBehaviour
{
    public enum GameMode
    {
        Normal = 0,
        Occupation = 1
    }
    #region public Fields

    public GameObject panel;
    public GameObject messageBox;
    public Button ok;
    public Button cancel;
    public Dropdown dpn;

    #endregion

    #region Setting Results

    public GameMode mode = GameMode.Normal;
    private GameMode tmpmode;

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
        dpn.onValueChanged.AddListener(DropDownSelect);

        tmpmode = mode;
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
        panel.SetActive(false);
    }

    public void OnClickCancel()
    {
        /*do nothing*/
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
}
