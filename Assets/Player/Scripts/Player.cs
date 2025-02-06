using UnityEngine;

public class Player : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }
    private PlayerState currentState;

    private void Start()
    {
        ChangeState(new GroundedState(this));
    }

    private void Update()
    {
        currentState?.Update();
    }

    public void ChangeState(PlayerState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
