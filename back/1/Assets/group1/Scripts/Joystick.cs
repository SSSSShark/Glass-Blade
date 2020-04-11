using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public Transform handle;
    public RectTransform panel;
    public int maxradius;
    public Vector2 Movement { get => movement; }

    private Vector2 movement;

    public void OnDrag(PointerEventData eventData)
    {
        //可能需要区分pointerID
        RectTransformUtility.ScreenPointToLocalPointInRectangle(panel, eventData.position, eventData.pressEventCamera, out Vector2 position);
        position = position.normalized * (position.magnitude > maxradius ? maxradius : position.magnitude);
        movement = position / maxradius;
        handle.localPosition = position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        movement = handle.localPosition = Vector2.zero;
    }
    //cytus add to use 'wasd'
    void Update()
    {
        Vector2 position = new Vector2(0, 0);

        if (Input.GetKey("w"))
        {
            position.y++;
        }
        if (Input.GetKey("w"))
        {
            movement = position.normalized;
            handle.localPosition = position;
        }
        if (Input.GetKeyUp("w"))
        {
            movement = handle.localPosition = Vector2.zero;
        }


        if (Input.GetKey("a"))
        {
            position.x--;
        }
        if (Input.GetKey("a"))
        {
            movement = position.normalized;
            handle.localPosition = position;
        }
        if (Input.GetKeyUp("a"))
        {
            movement = handle.localPosition = Vector2.zero;
        }


        if (Input.GetKey("s"))
        {
            position.y--;
        }
        if (Input.GetKey("s"))
        {
            movement = position.normalized;
            handle.localPosition = position;
        }
        if (Input.GetKeyUp("s"))
        {
            movement = handle.localPosition = Vector2.zero;
        }


        if (Input.GetKey("d"))
        {
            position.x++;
        }
        if (Input.GetKey("d"))
        {
            movement = position.normalized;
            handle.localPosition = position;
        }
        if (Input.GetKeyUp("d"))
        {
            movement = handle.localPosition = Vector2.zero;
        }
    }
}
