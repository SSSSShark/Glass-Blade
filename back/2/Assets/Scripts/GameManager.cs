using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
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

  public Button AttackBtn;

  #endregion

  // Start is called before the first frame update
  void Start()
  {
    if (playerPrefeb == null)
    {
      Debug.LogError("Missing playerPrefeb reference");
    }
    else
    {
      if (movegetgromjoystick.LocalPlayerInstance == null)
      {
        GameObject thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(-13, 5f, -25), Quaternion.identity, 0);
        thePlayer.GetComponent<movegetgromjoystick>().touch = joystick;
        AttackBtn.onClick.AddListener(thePlayer.GetComponent<PlayerCharacter>().Attack);
        int nChild = weaponSystem.transform.childCount;
        Debug.Log("GameManager: the player instantiated has " + nChild + " children");
        //thePlayer.GetComponent<CharacterBehavior>().touch = joystick;
        GameObject[] children = new GameObject[nChild];
        WeaponRefresh wr;

        for (int i = 0; i < nChild; ++i)
        {
          children[i] = weaponSystem.transform.GetChild(i).gameObject;
          if (wr = children[i].GetComponent<WeaponRefresh>())
          {
            Debug.Log("GameManager: player assigned");
            wr.player = thePlayer.gameObject;
          }
        }

        thePlayer.GetComponent<PlayerCharacter>().refresh_places = children;
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
