using UnityEngine;
using UnityEngine.EventSystems;

public class CameraButtonHold : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public enum ActionType
    {
        Rotate,
        Zoom
    }

    [SerializeField] private CameraInput cameraInput;
    [SerializeField] private ActionType actionType;
    [SerializeField] private float value = 1f;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (cameraInput == null) return;

        if (actionType == ActionType.Rotate)
            cameraInput.SetRotateButtonInput(value);
        else
            cameraInput.SetZoomButtonInput(value);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (cameraInput == null) return;

        if (actionType == ActionType.Rotate)
            cameraInput.SetRotateButtonInput(0f);
        else
            cameraInput.SetZoomButtonInput(0f);
    }
}