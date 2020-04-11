using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    #region Public Fields
    [SerializeField]
    [Tooltip("The prefeb to use for representing the player")]
    public GameObject playerPrefeb;
    [SerializeField]
    [Tooltip("The prefeb used to show time ,owned by maste client")]
    public GameObject informationPrefeb;
    public Joystick joystick;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        if(playerPrefeb == null)
        {
            Debug.LogError("Missing playerPrefeb reference");
        }
        else
        {
            if (movegetgromjoystick.LocalPlayerInstance == null)
            {
                GameObject thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                thePlayer.GetComponent<movegetgromjoystick>().touch = joystick;
                //thePlayer.GetComponent<CharacterBehavior>().touch = joystick;
            }
            else
            {
                // ignore
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
