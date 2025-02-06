public class GroundedState : PlayerState
{
    public GroundedState(Player player) : base(player) { }

    public override void Enter()
    {
        // Set animation, reset jump count, etc.
    }

    public override void Update()
    {
        if (player.IsJumping)
        {
            player.ChangeState(new FlyingState(player));
        }
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
