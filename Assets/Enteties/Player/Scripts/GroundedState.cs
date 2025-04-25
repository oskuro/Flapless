using UnityEngine;
public class GroundedState : PlayerState
{
    private Rigidbody2D Rb2D => player.Rb2D;

    public GroundedState(Player player) : base(player) { }

    public override void Enter()
    {
        player.Jumped += Jump;
    }

    public override void Update()
    {
         if (!player.IsGrounded && player.BalloonCount > 0) 
        {
            if(player.Debugging)
                Debug.Log("Changing state to flying");
            player.ChangeState(player.FlyingState);
        }
    }

    public void Jump()
    {
        if (player.IsGrounded)
        {
            Rb2D.AddForce(Vector2.up * player.JumpForce, ForceMode2D.Impulse);
        }
        player.PlayerAnimator.SetBool("Jumping", true);
    }

    public override void FixedUpdate()
    {
        int layerMask = LayerMask.GetMask("Ground"); 
        Collider2D hit = Physics2D.OverlapBox(player.transform.position + Vector3.right * player.MoveInput * 0.5f, player.MoveCheck, 0f, layerMask);
        if (hit != null)
        {
            if(player.Debugging)
                Debug.Log("Can't move");
            return; 
        }

        // Direct velocity control for more precise movement
        float targetSpeed = player.MoveInput * player.PlayerRunSpeed;
        float speedDiff = targetSpeed - Rb2D.linearVelocity.x;
        float acceleration = (Mathf.Abs(targetSpeed) > 0.01f) ? player.GroundAcceleration : player.GroundDeceleration;
        
        // Apply acceleration to reach target speed
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * acceleration, 0.96f) * Mathf.Sign(speedDiff);
        Rb2D.AddForce(movement * Vector2.right, ForceMode2D.Force);

        // Clamp horizontal speed
        Vector2 clampedVelocity = Rb2D.linearVelocity;
        clampedVelocity.x = Mathf.Clamp(clampedVelocity.x, -player.PlayerRunSpeed, player.PlayerRunSpeed);
        Rb2D.linearVelocity = clampedVelocity;

        var runSpeed = Mathf.Clamp(Mathf.Abs(Rb2D.linearVelocity.x), 0f, 1f);
        player.PlayerAnimator.SetFloat("runspeed", runSpeed);
    }


    public override void Exit()
    {
         player.Jumped -= Jump;
    }
}
