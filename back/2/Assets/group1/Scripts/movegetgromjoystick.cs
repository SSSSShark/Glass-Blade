using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class movegetgromjoystick : MonoBehaviourPunCallbacks
{
    #region Public Fields

    public Joystick touch;
    public float speed = 25;
    public Animator ani;

    [Tooltip("The local player instance. Use this to know if the local player is represented in the scene")]
    public static GameObject LocalPlayerInstance;
    public float gravity = 9;
    #endregion
    #region Private Field
    private float yspeed = 0f;
    #endregion
    private void Awake()
    {
        // #Important
        // we keep track of the localPlayer instance to prevent instantiation when levels are synchronized
        if (photonView.IsMine)
        {
            movegetgromjoystick.LocalPlayerInstance = this.gameObject;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
        //touch = GameManager.joyStick;
        if (photonView.IsMine)
        {
            if (touch == null)
            {
                Debug.LogError("cannot find joystick");
            }
        }
        CameraFollow _cameraFollow = this.gameObject.GetComponent<CameraFollow>();

        if (_cameraFollow != null)
        {
            if (photonView.IsMine)
            {
                _cameraFollow.OnStartFollowing();
            }
        }
        else
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> CameraWork Component on playerPrefab.", this);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
        {
            return;
        }

        Vector3 direction = new Vector3(touch.Movement.x, 0, touch.Movement.y);
        CharacterController controller = GetComponent<CharacterController>();
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10);
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
            ani.SetFloat("Speed", speed);
            controller.SimpleMove(direction * speed);
        }
        else
        {
            ani.SetFloat("Speed", 0);
        }
        if (!controller.isGrounded)
        {
            //Debug.Log("g" + gravity);
            yspeed -= gravity * Time.deltaTime;
            controller.Move(new Vector3(0, yspeed * Time.deltaTime, 0));
        }
        else
        {
            yspeed = 0;
        }
    }
}
