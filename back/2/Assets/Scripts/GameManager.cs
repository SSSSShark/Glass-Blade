using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
    public enum GameType
    {
        Occupation,
        Normal,
        Error
    };

    #region Public Fields
    [SerializeField]
    [Tooltip("The prefeb to use for representing the player")]
    public GameObject playerPrefeb;
    [SerializeField]
    [Tooltip("The prefeb used to show time ,owned by maste client")]
    public GameObject informationPrefeb;
    public Joystick joystick;
    public GameObject weaponSystem;
    public GameObject localPlayerObj;
    public GameObject gameSkillButton;
    public GameObject mapCamera;

    public Button AttackBtn;

    public GameType gameType;

    #endregion

    /// <summary>
    /// Invoked when the game initialized.
    /// We instantiate players and needed elements here
    /// </summary>
    void Awake()
    {
        if (playerPrefeb == null)
        {
            Debug.LogError("[GameManager:Start()] BUG: Missing playerPrefeb reference");
        }
        else
        {
            if (movegetgromjoystick.LocalPlayerInstance == null)
            {
                // Instantiate the player
                GameObject thePlayer;
                if ((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA)
                {
                    thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(-24, 5f, -46), Quaternion.identity, 0);
                }
                else
                {
                    thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(24, 5f, 46), Quaternion.identity, 0);
                }

                localPlayerObj = thePlayer;

                // Assign components in the scene to the player we just instantiated, so that the player can
                // use those component

                // Assign joy stick
                thePlayer.GetComponent<movegetgromjoystick>().touch = joystick;

                // Set up player attack button
                AttackBtn.onClick.AddListener(thePlayer.GetComponent<PlayerCharacter>().Attack);

                // Assign the player to the weapon refresh places
                int nChild = weaponSystem.transform.childCount;
                Debug.Log("[GameManager:Start()] there are " + nChild + " fresh places");
                //thePlayer.GetComponent<CharacterBehavior>().touch = joystick;
                GameObject[] children = new GameObject[nChild];
                WeaponRefresh wr;

                for (int i = 0; i < nChild; ++i)
                {
                    children[i] = weaponSystem.transform.GetChild(i).gameObject;
                    if (wr = children[i].GetComponent<WeaponRefresh>())
                    {
                        // Debug.Log("[GameManager:Start()] player assigned");
                        wr.player = thePlayer.gameObject;
                    }
                }

                // Assign the refresh places to the player
                thePlayer.GetComponent<PlayerCharacter>().refresh_places = children;

                // Assign the GameManager itself to the player so that the player can call the game manager
                thePlayer.GetComponent<PlayerCharacter>().GM = this;

                // assign attack button
                gameSkillButton.GetComponent<SkillCooling>().player = thePlayer;

                // assign sprite
                if (GameObject.Find("SettingStore").GetComponent<SettingStore>().skillsprite)
                {
                    gameSkillButton.GetComponent<Image>().sprite = GameObject.Find("SettingStore").GetComponent<SettingStore>().skillsprite;
                }

                // assign the player to character behavior
                thePlayer.GetComponent<CharacterBehavior>().gamePlayer = thePlayer;

                // assign the payer to the player charactor
                thePlayer.GetComponent<PlayerCharacter>().gamePlayer = thePlayer;

                thePlayer.GetComponent<PlayerCharacter>().skillBtn = gameSkillButton.GetComponent<Button>();

                mapCamera.GetComponent<MapCameraControl>().player = thePlayer;

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
