//Author: David Wang
//Function: To show countdown time, component of Canvas/Information/CountDown
//Todo:Add Canvas to the scene, then add Information.prefab,add this script in CountDown
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    public int minute;
    private int getTime;
    private float timer = 0;
    public Text countTimeText;
    // Start is called before the first frame update
    void Start()
    {
        getTime = 60 * minute;
        countTimeText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int M = getTime / 60;
        int S = getTime % 60;
        timer += Time.deltaTime;
        if(timer >= 1f)
        {
            timer = 0;
            getTime --;
            countTimeText.text = M + ":" + string.Format("{0:00}",S);
        }
    }
}
