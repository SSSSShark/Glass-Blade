using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadHouseControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var target = other.GetComponent<PlayerCharacter>();
        if (target)
        {
            Debug.Log("[DeadHoushControl:OnTriggerEnter()] find target");
        }
    }


    private void OnTriggerStay(Collider other)
    {
        {
            var target = other.GetComponent<PlayerCharacter>();
            if (target)
            {
                if (target.isAlive)
                {
                    var reliver = Random.Range(-180.0f, 180.0f);
                    target.transform.rotation = Quaternion.Euler(0, reliver, 0);

                    float reliveX;
                    float reliveZ;
                    float reliveY = 1;
                    if (target.team == TeamController.Team.TeamA)
                    {
                        Debug.Log("[DeadHoushControl:OnTriggerStay()] Team A player relive, placed to team A position");
                        reliveX = -24;
                        reliveZ = -46;
                    }
                    else
                    {
                        Debug.Log("[DeadHoushControl:OnTriggerStay()] Team B player relive, placed to team B position");
                        reliveX = 24;
                        reliveZ = 46;
                    }
                    target.transform.position = new Vector3(reliveX, reliveY, reliveZ);
                }
            }
        }
    }
}
