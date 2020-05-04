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
        { "技能1", Skill.SkillA},{ "技能2", Skill.SkillB},{ "技能3", Skill.SkillC},{ "技能4", Skill.SkillD}
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
        skillImg.color = tmpcolor;
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

