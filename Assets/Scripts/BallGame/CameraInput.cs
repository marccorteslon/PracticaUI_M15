using UnityEngine;

[RequireComponent(typeof(CameraController))]
public class CameraInput : MonoBehaviour
{
    private CameraController controller;

    [SerializeField] private VirtualJoystick cameraJoystick;
    [SerializeField] private float rotationMultiplier = 120f;
    [SerializeField] private float zoomMultiplier = 10f;
    [SerializeField] private bool useVerticalForZoom = false;

    private void Start()
    {
        controller = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (controller == null || cameraJoystick == null) return;

        Vector2 input = cameraJoystick.InputDirection;

        if (Mathf.Abs(input.x) > 0.01f)
            controller.Rotate(input.x * rotationMultiplier * Time.deltaTime);

        if (useVerticalForZoom && Mathf.Abs(input.y) > 0.01f)
            controller.Zoom(-input.y * zoomMultiplier * Time.deltaTime);
    }
}