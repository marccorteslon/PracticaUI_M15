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
        // Opcional: mostrar ghost en la posición inicial del dedo
        if (joystickGhost != null)
        {
            joystickGhost.position = data.position;
        }

        OnDrag(data);
    }

    public void OnDrag(PointerEventData data)
    {
        Debug.Log(data.position);

        // Mover el joystick a la posición del dedo
        joystick.position = data.position;

        // Dirección desde el centro del joystick
        Vector3 dir = joystick.position - joystickParent.position;

        // Distancia al centro
        float distance = dir.magnitude;

        // Limitar al radio máximo
        if (distance > maxRadius)
        {
            dir.Normalize();
            dir *= maxRadius;
        }

        // Aplicar desplazamiento local respecto al centro
        joystick.localPosition = dir;

        // Guardar dirección normalizada para usarla como input
        InputDirection = new Vector2(dir.x, dir.y) / maxRadius;
        InputDirection = Vector2.ClampMagnitude(InputDirection, 1f);
    }

    public void OnEndDrag(PointerEventData data)
    {
        // Volver al centro
        joystick.localPosition = Vector3.zero;

        if (joystickGhost != null)
        {
            joystickGhost.localPosition = Vector3.zero;
        }

        InputDirection = Vector2.zero;
    }
}