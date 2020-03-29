using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movegetgromjoystick : MonoBehaviour
{
    public Joystick touch;
    public float speed=25;
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(touch.Movement.x, 0, touch.Movement.y);
        CharacterController controller = GetComponent<CharacterController>();
        if (direction!=Vector3.zero)
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
    }
}
