using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingStore : MonoBehaviour
{
    #region Master Settings
    public bool frienlyFire;
    public string setTime;
    #endregion

    #region Self Settings
    public SkillBox.Skill myskill;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        GameObject.DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        /*do nothing*/
    }
}
