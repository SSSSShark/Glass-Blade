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
}
