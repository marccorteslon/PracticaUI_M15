using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform joystickParent;
    public RectTransform joystick;
    public RectTransform joystickGhost;
    public float maxRadius = 100f;

    public Vector2 InputDirection { get; private set; }

    public void OnBeginDrag(PointerEventData data)
    {
        if (joystickGhost != null)
            joystickGhost.position = data.position;

        OnDrag(data);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector3 dir = data.position - joystickParent.position;
        float distance = dir.magnitude;

        if (distance > maxRadius)
        {
            dir.Normalize();
            dir *= maxRadius;
        }

        joystick.localPosition = dir;

        InputDirection = new Vector2(dir.x, dir.y) / maxRadius;
        InputDirection = Vector2.ClampMagnitude(InputDirection, 1f);
    }

    public void OnEndDrag(PointerEventData data)
    {
        joystick.localPosition = Vector3.zero;

        if (joystickGhost != null)
            joystickGhost.localPosition = Vector3.zero;

        InputDirection = Vector2.zero;
    }
}