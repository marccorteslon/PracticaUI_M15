using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CameraController))]
public class CameraInput : MonoBehaviour
{
    private CameraController controller;

    [Header("Drag Rotation")]
    [SerializeField] private bool enableMouseDragRotation = true;
    [SerializeField] private bool enableTouchDragRotation = true;
    [SerializeField] private float dragRotationMultiplier = 0.2f;
    [SerializeField] private bool invertDrag = false;

    [Header("Zoom")]
    [SerializeField] private bool enableMouseWheelZoom = true;
    [SerializeField] private float mouseWheelZoomMultiplier = 2f;

    [Header("Button Hold Speeds")]
    [SerializeField] private float buttonRotationSpeed = 90f;
    [SerializeField] private float buttonZoomSpeed = 10f;

    private bool draggingMouse;
    private Vector2 lastMousePosition;

    private bool draggingTouch;
    private Vector2 lastTouchPosition;

    private float buttonRotateInput;
    private float buttonZoomInput;

    private void Start()
    {
        controller = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (controller == null) return;

        HandleMouseDrag();
        HandleTouchDrag();
        HandleMouseWheelZoom();
        HandleButtonInput();
    }

    private void HandleMouseDrag()
    {
        if (!enableMouseDragRotation) return;

        if (Input.GetMouseButtonDown(1))
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                draggingMouse = false;
                return;
            }

            draggingMouse = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1) && draggingMouse)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 delta = currentMousePosition - lastMousePosition;
            lastMousePosition = currentMousePosition;

            ApplyDragRotation(delta, currentMousePosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            draggingMouse = false;
        }
    }

    private void HandleTouchDrag()
    {
        if (!enableTouchDragRotation) return;
        if (Input.touchCount == 0)
        {
            draggingTouch = false;
            return;
        }

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                draggingTouch = false;
                return;
            }

            draggingTouch = true;
            lastTouchPosition = touch.position;
        }

        if (touch.phase == TouchPhase.Moved && draggingTouch)
        {
            Vector2 currentTouchPosition = touch.position;
            Vector2 delta = currentTouchPosition - lastTouchPosition;
            lastTouchPosition = currentTouchPosition;

            ApplyDragRotation(delta, currentTouchPosition);
        }

        if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            draggingTouch = false;
        }
    }

    private void ApplyDragRotation(Vector2 delta, Vector2 pointerPosition)
    {
        float sign = invertDrag ? -1f : 1f;

        float viewportOffset = pointerPosition.x - (Screen.width * 0.5f);
        float centerFactor = Mathf.Sign(viewportOffset == 0 ? 1f : viewportOffset);

        float rotationValue = delta.x * dragRotationMultiplier * sign * centerFactor;
        controller.Rotate(rotationValue);
    }

    private void HandleMouseWheelZoom()
    {
        if (!enableMouseWheelZoom) return;

        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.001f)
        {
            controller.Zoom(-scroll * mouseWheelZoomMultiplier);
        }
    }

    private void HandleButtonInput()
    {
        if (Mathf.Abs(buttonRotateInput) > 0.001f)
        {
            controller.Rotate(buttonRotateInput * buttonRotationSpeed * Time.deltaTime);
        }

        if (Mathf.Abs(buttonZoomInput) > 0.001f)
        {
            controller.Zoom(buttonZoomInput * buttonZoomSpeed * Time.deltaTime);
        }
    }

    public void SetRotateButtonInput(float value)
    {
        buttonRotateInput = value;
    }

    public void SetZoomButtonInput(float value)
    {
        buttonZoomInput = value;
    }

    public void FocusBall()
    {
        controller.SwitchTargetBall();
    }

    public void FocusTarget()
    {
        controller.SwitchTargetTarget();
    }

    public void FocusMiddle()
    {
        controller.SwitchTargetMiddlepoint();
    }
}