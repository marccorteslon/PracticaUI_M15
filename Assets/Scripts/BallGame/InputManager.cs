using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    public static Inputs inputMap { get; private set; }
    private void Awake()
    {
        Instance = this;
        inputMap = new Inputs();
        inputMap.Enable();
    }

    private void Start()
    {
        inputMap.Ball.Enable();
    }
}
