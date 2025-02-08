using UnityEngine;
public class FlyingState : PlayerState
{
    private Rigidbody2D Rb2D => player.Rb2D; // uwu, property to access Rigidbody2D
    public FlyingState(Player player) : base(player) { }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (player.IsGrounded)
        {
            player.ChangeState(player.GroundedState);
        }
    }

    public override void FixedUpdate()
    {

        // Vertical Movement
        Vector2 clampedVelocity = Rb2D.linearVelocity;
        if (player.IsJumping)
        {
            // Add upward force to make the character fly
            Rb2D.AddForce(Vector2.up * player.FlyingForce, ForceMode2D.Force);
            // Clamp Y
            clampedVelocity.y = Mathf.Clamp(clampedVelocity.y, -player.FlyingForce, player.FlyingForce);
        }

        // Horizontal
        float targetSpeed = player.MoveInput * player.FlyingSpeed;
        float speedDiff = targetSpeed - clampedVelocity.x;
        float acceleration = player.FlyingAcceleration * 0.5f; 

        if (player.MoveInput == 0) 
        {
            float drag = player.FlyingDrag; 
            clampedVelocity.x *= (1 - drag * Time.fixedDeltaTime); 
        }
        else
        {
            float smoothFactor = 0.005f; 
            clampedVelocity.x = Mathf.Lerp(clampedVelocity.x, targetSpeed, smoothFactor);
            
            float movement = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, 0.5f) * Mathf.Sign(speedDiff);
            Rb2D.AddForce(movement * Vector2.right, ForceMode2D.Force);
        }

        float maxChange = player.MaxVelocityChange;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -player.FlyingSpeed, player.FlyingSpeed);
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, player.Rb2D.linearVelocity.x - maxChange, player.Rb2D.linearVelocity.x + maxChange);

        player.Rb2D.linearVelocity = clampedVelocity;
    }

    public override void Exit()
    {
        // Cleanup if needed
    }

    
}
