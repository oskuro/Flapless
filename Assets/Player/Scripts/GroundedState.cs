using UnityEngine;
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
            if (player.BalloonCount > 0)
            {
                player.ChangeState(new FlyingState(player));
            }
            else if (player.IsGrounded)
            {
                player.Rb2D.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
            }
        }


 
    }

    public override void FixedUpdate()
    {
        // Direct velocity control for more precise movement
        float targetSpeed = player.MoveInput * player.PlayerRunSpeed;
        float speedDiff = targetSpeed - player.Rb2D.linearVelocity.x;
        float acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? player.GroundAcceleration : player.GroundDeceleration;
        
        // Apply acceleration to reach target speed
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, 0.96f) * Mathf.Sign(speedDiff);
        player.Rb2D.AddForce(movement * Vector2.right, ForceMode2D.Force);

        // Clamp horizontal speed
        Vector2 clampedVelocity = player.Rb2D.linearVelocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -player.PlayerRunSpeed, player.PlayerRunSpeed);
        player.Rb2D.linearVelocity = clampedVelocity;

        var runSpeed = Mathf.Clamp(Mathf.Abs(movement), 0f, 1f);
        player.PlayerAnimator.SetFloat("RunSpeed", runSpeed);
    }


    public override void Exit()
    {
        // Cleanup if needed
    }
}
