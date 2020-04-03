using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MonoBehaviour
{
    CameraFollow cam;
    ArrayList players;
    int allies_cnt;
    void Start()
    {
        cam= GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>();
        players = new ArrayList();
        allies_cnt = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if(player)
        {
            if(player.team==cam.team)
            {
                if(allies_cnt==0)
                {
                    foreach(PlayerCharacter p in players)
                    {
                        p.SetTransparent(0.5f);
                    }
                }
                allies_cnt++;
            }
            player.SetTransparent(allies_cnt == 0 ? 0f: 0.5f);
            players.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerCharacter player = other.GetComponent<PlayerCharacter>();
        if (player)
        {
            players.Remove(player);
            player.SetTransparent(1f);
            if (player.team == cam.team)
            {
                allies_cnt--;
                if (allies_cnt == 0)
                {
                    foreach (PlayerCharacter p in players)
                    {
                        p.SetTransparent(0f);
                    }
                }
            }
        }
    }
}
