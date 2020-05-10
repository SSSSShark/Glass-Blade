using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class SkillBox : MonoBehaviour
{
    public enum Skill
    {
        SkillA = 1,
        SkillB = 2,
        SkillC = 3,
        SkillD = 4
    }
    #region public Fields

    public GameObject settingstore;
    public GameObject panel;
    public GameObject messageBox;
    public Button ok;
    public GameObject[] skillbtns;
    public Image skillImg;

    #endregion

    #region Setting Results

    public Skill skill = Skill.SkillA;
    private Skill tmpskill;
    private Color tmpcolor;
    private Dictionary<string, Skill> skillNameMap = new Dictionary<string, Skill>{
        { "突进", Skill.SkillA},{ "加速", Skill.SkillB},{ "隐身", Skill.SkillC},{ "无敌", Skill.SkillD}
    };
    //private static List<Button> items = new List<Button>();

    #endregion
    // Use this for initialization
    void Start()
    {
        panel = GameObject.Find("SkillSetting");
        messageBox = gameObject;
        ok = messageBox.transform.Find("Ok").GetComponent<Button>();
        skillImg = GameObject.Find("SkillImage").GetComponent<Image>();
        skillbtns = GameObject.FindGameObjectsWithTag("Skill");
        BindClickEventWithItems();
        //cancel = messageBox.transform.Find("Cancel").GetComponent<Button>();

        ok.onClick.AddListener(OnClickOk);
        //cancel.onClick.AddListener(OnClickCancel);
        // dpn.onValueChanged.AddListener(DropDownSelect);

        tmpskill = skill;
        settingstore.GetComponent<SettingStore>().myskill = skill;
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
        skill = tmpskill;
        settingstore.GetComponent<SettingStore>().myskill = skill;
        skillImg.color = tmpcolor;

        /* 更改图片方法 */
        //string path = "Images/Item/img";    //image路径
        //Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite;    //参数为资源路径和资源类型
        //skillImg.sprite = sprite; 

        panel.SetActive(false);
    }

    //public void OnClickCancel()
    //{
    //    /*do nothing*/
    //    panel.SetActive(false);
    //}

    private void BindClickEventWithItems()
    {
        for (int i = 0; i < skillbtns.Length; i++)
        {
            string str = skillbtns[i].GetComponentInChildren<Text>().text;
            Color color = skillbtns[i].GetComponentInChildren<Image>().color;
            skillbtns[i].GetComponent<Button>().onClick.AddListener(delegate
            {
                tmpskill = skillNameMap[str];
                tmpcolor = color;
                Debug.Log(tmpskill);
            });
        }
    }
}

