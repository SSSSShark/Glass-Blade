using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingStore : MonoBehaviour
{
    #region Master Settings

    public bool friendlyFire;
    public int setTime;

    #endregion

    #region Self Settings

    public SkillBox.Skill myskill;
    public Sprite skillsprite;

    #endregion

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    //void Start()
    //{
    //    GameObject.DontDestroyOnLoad(gameObject);
    //}

    //// Update is called once per frame
    //void Update()
    //{
    //    /*do nothing*/
    //}
}
