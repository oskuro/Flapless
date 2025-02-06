using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    // public fields
    public int BalloonCount { get; private set; } = 0;
    public float MoveInput { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }

    // Variables related to ground movement
    [Header("Grounded Movement")]
    [SerializeField] private float _playerRunSpeed = 5f;
    public float PlayerRunSpeed => _playerRunSpeed;
    [SerializeField] private float _jumpForce = 100f;
    public float JumpForce => _jumpForce;
    public float GroundAcceleration = 70f;  
    public float GroundDeceleration = 50f;

    // The rest
    private PlayerState _currentState;
    InputAction _moveAction;
    InputAction _jumpAction;
    private SpriteRenderer _spriteRenderer;

    [SerializeField] LayerMask _groundLayer;

    float _rayDistance = 1f;
    private void Start()
    {
        InitClassVariables();

        // Change to our default state
        ChangeState(new GroundedState(this));
    }

    private void InitClassVariables()
    {
        // Get our input actions
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");

        // calculate player height
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            var playerHeight = collider.bounds.size.y;
            var offset = collider.offset.y;
            _rayDistance = playerHeight / 2f + 0.1f - offset;
        }

        // Set our Rigidbody so that our states can move the player
        Rb2D = GetComponent<Rigidbody2D>();

        PlayerAnimator = GetComponentInChildren<Animator>();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckGrounded();

        GetPlayerInput();

        if(MoveInput != 0)
            _spriteRenderer.flipX = MoveInput < 0;

        _currentState?.Update();
    }

    private void GetPlayerInput()
    {
        // Get player input values
        MoveInput = _moveAction.ReadValue<float>();
        IsJumping = _jumpAction.WasPerformedThisFrame();
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, _rayDistance, _groundLayer);
        IsGrounded = hit.collider != null;
    }

    private void FixedUpdate() {
        _currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawRay(transform.position, Vector2.down * _rayDistance); 
    }
}
