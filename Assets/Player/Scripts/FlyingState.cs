public class FlyingState : PlayerState
{
    public FlyingState(Player player) : base(player) { }

    public override void Enter()
    {
        // Set flying animation, modify gravity, etc.
    }

    public override void Update()
    {
        if (player.IsGrounded && player.BalloonCount == 0)
        {
            player.ChangeState(player.GroundedState);
        }
    }

    public override void FixedUpdate()
    {
        
    }

    public override void Exit()
    {
        // Cleanup if needed
    }

    
}
