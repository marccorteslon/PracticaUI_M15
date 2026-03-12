using UnityEngine;
using UnityEngine.InputSystem;

public class MovementManager : MonoBehaviour
{
    private BallController ballController;

    public Inputs inputMap;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ballController = GetComponent<BallController>();
        inputMap = InputManager.inputMap;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = inputMap.Ball.Move.ReadValue<Vector2>();
        ballController.Move(moveInput);
        if (inputMap.Ball.Jump.WasPressedThisFrame()){
            ballController.Jump();
        }
    }
}
