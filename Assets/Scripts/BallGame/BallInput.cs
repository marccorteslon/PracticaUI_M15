using UnityEngine;

[RequireComponent(typeof(BallController))]
public class BallInput : MonoBehaviour
{
    private BallController controller;

    [SerializeField] private VirtualJoystick moveJoystick;

    private void Start()
    {
        controller = GetComponent<BallController>();
    }

    private void Update()
    {
        if (controller == null || moveJoystick == null) return;

        controller.Move(moveJoystick.InputDirection);
    }

    public void Jump()
    {
        if (controller == null) return;
        controller.Jump();
    }
}