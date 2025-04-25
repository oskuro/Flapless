using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[SelectionBase]
public class Player : MonoBehaviour
{
    // public fields
    public int BalloonCount { get; set; } = 0;
    [SerializeField] LayerMask _balloonLayer;
    public float MoveInput { get; private set; }
    public Rigidbody2D Rb2D { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsJumping { get; private set; }

    public bool Debugging = false;

    // Variables related to ground movement
    [Header("Grounded Movement")]
    [SerializeField] private float _playerRunSpeed = 5f;
    public float PlayerRunSpeed => _playerRunSpeed;
    [SerializeField] private float _jumpForce = 4;
    public float JumpForce => _jumpForce;
    public float GroundAcceleration = 70f;  
    public float GroundDeceleration = 50f;
    public Action Jumped; 
    float _rayDistance = 1f;
    [SerializeField] Vector2 _groundCheckSize;

    // Flying goes here
    [Header("Flying Movement")]
    [SerializeField] private float _flyingForce = 5f;
    public float FlyingForce => _flyingForce;
    [SerializeField] private float _flyingSpeed = 5f;
    public float FlyingSpeed => _flyingSpeed;
    public float FlyingAcceleration = 35f;  
    public float FlyingDeceleration = 20f;
    public float FlyingDrag = 0.2f;
    public float VerticalVelocityClamp;

    
    // The rest
    public PlayerState _currentState;
    public PlayerState GroundedState { get; private set; }
    public PlayerState FlyingState { get; private set; }
    public int MaxBalloons { get; set; } = 2;

    InputAction _moveAction;
    InputAction _jumpAction;
    private SpriteRenderer _spriteRenderer;
    
    [Header("General")]
    public Vector2 MoveCheck;
    [SerializeField] LayerMask _groundLayer;
    internal float MaxVelocityChange = 0.2f;

    public Action OnDeath;


    void Awake() 
    {
        GroundedState = new GroundedState(this);
        FlyingState = new FlyingState(this);
    }

    private void Start()
    {
        InitClassVariables();

        // init state
        if (BalloonCount > 0) { ChangeState(new FlyingState(this)); }
        else { ChangeState(new GroundedState(this)); }
        
    }

    private void InitClassVariables()
    {
        // Get our input actions
        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");

        // calculate player height
        Collider2D collider = GetComponent<CapsuleCollider2D>();
        if (collider != null)
        {
            var playerHeight = collider.bounds.max.y;
            var offset = collider.offset.y;
            _rayDistance = (playerHeight / 2f - offset) * transform.localScale.y;

            if(Debugging)
                Debug.Log($"Ray distance: {_rayDistance}");
        }

        // Set our Rigidbody so that our states can move the player
        Rb2D = GetComponent<Rigidbody2D>();

        PlayerAnimator = GetComponentInChildren<Animator>();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        CheckGrounded();
        CheckForEnemies();
        PlayerAnimator.SetBool("IsGrounded", IsGrounded);
        PlayerAnimator.SetBool("Jumping", !IsGrounded);

        GetPlayerInput();

        // Flip sprite
        if(MoveInput != 0)
            _spriteRenderer.flipX = MoveInput < 0;

        _currentState?.Update();
    }

    private void GetPlayerInput()
    {
        // Get player input values
        MoveInput = _moveAction.ReadValue<float>();
        IsJumping = _jumpAction.IsPressed();
        if(_jumpAction.WasPressedThisFrame()) 
        {
            Jumped?.Invoke();
        }
    }

    private void CheckGrounded()
    {
        // This is the actual grounded check
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, _groundCheckSize, 0f, Vector2.down, _rayDistance, _groundLayer);
        IsGrounded = hit.collider != null;

    }

    void CheckForEnemies() {
        // This is checking for balloon beneath player
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, _groundCheckSize, 0f, Vector2.down, _rayDistance, _balloonLayer);
        
        // Return early
        if (hit.collider == null) {return;}
        
        // Damage balloon
        if(!hit.collider.gameObject.TryGetComponent<Health>(out Health health)) { return; }
        health.TakeDamage(10);
    }

    private void FixedUpdate() {
        _currentState?.FixedUpdate();
    }

    public void ChangeState(PlayerState newState)
    {
        if(Debugging)
            Debug.Log("New state: " + newState);
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    void OnCollisionEnter2D(Collision2D collision) 
    {  
        if(Debugging) 
        {
            Debug.Log("Collision: " + collision.gameObject.name);
        } 
    } 

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(Debugging) 
        {
            Debug.Log("Trigger: " + collider.gameObject.name);
        }

    }

    void OnDrawGizmos() 
    {
        if(!Debugging)
            return;
        // Is Grounded Check
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position + (Vector3.down * _rayDistance), (Vector3) _groundCheckSize);
        
        // Movement Check
        Gizmos.color = Color.red; 
        Gizmos.DrawWireCube(transform.position + Vector3.right * MoveInput * 0.5f, MoveCheck); 
    }

    public void AddBalloon() 
    {
        BalloonCount++;
    }

    public void RemoveBalloon() 
    {
        BalloonCount--;
        if (BalloonCount <= 0 && OnDeath != null)
        {
            OnDeath();
            this.enabled = false;
        }
    }
}
