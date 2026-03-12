using UnityEngine;

public class JumpButton : MonoBehaviour
{
    [SerializeField] private BallInput ballInput;

    public void Jump()
    {
        if (ballInput == null) return;
        ballInput.Jump();
    }
}