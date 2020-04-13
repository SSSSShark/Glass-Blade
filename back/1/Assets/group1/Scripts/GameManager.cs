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

    public GameObject weaponSystem;

    #endregion

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
                int nChild = weaponSystem.transform.childCount;
                Debug.Log("GameManager: the player instantiated has " + nChild + " children");
                //thePlayer.GetComponent<CharacterBehavior>().touch = joystick;
                GameObject[] children = new GameObject[nChild];
                WeaponRefresh wr;

                for(int i = 0; i < nChild; ++ i) {
                    children[i] = weaponSystem.transform.GetChild(i).gameObject;
                    if(wr = children[i].GetComponent<WeaponRefresh>()) {
                        Debug.Log("GameManager: player assigned");
                        wr.player = thePlayer.gameObject;
                    }
                }

                thePlayer.GetComponent<PlayerCharacter>().refresh_places = children;
            }
            else
            {
                
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
