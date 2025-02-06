public class FlyingState : PlayerState
{
    public FlyingState(Player player) : base(player) { }

    public override void Enter()
    {
        // Set flying animation, modify gravity, etc.
    }

    public override void Update()
    {
        if (player.IsGrounded)
        {
            player.ChangeState(new GroundedState(player));
        }
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
