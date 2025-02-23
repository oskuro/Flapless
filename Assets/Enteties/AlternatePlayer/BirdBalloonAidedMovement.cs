using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdBalloonAidedMovement : MonoBehaviour
{
    [SerializeField] float horizontalSpeed = 2f;
    [SerializeField] float verticalSpeed = 2f;
    [SerializeField] float _flyInterval = 0.5f;
    float _previousFlyTime = 0f;
    public Rigidbody2D Rb2D { get; private set; }
    public Animator PlayerAnimator { get; private set; }
    private SpriteRenderer _spriteRenderer;
    InputAction _moveAction;
    public bool IsGrounded { get; private set; }
    float _rayDistance = 1f;
    [SerializeField] Vector2 _groundCheckSize;
    [SerializeField] LayerMask _groundLayer;
    [SerializeField] LayerMask _balloonLayer;


    // Animator hashes to improve speed 
    int _flapHash;
    int _jumpingHash;
    int _groundedHash;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        Rb2D = GetComponent<Rigidbody2D>();

        PlayerAnimator = GetComponentInChildren<Animator>();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        // calculate player height
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            var playerHeight = collider.bounds.max.y;
            var offset = collider.offset.y;
            _rayDistance = playerHeight / 2f - offset;
        }

        _flapHash = Animator.StringToHash("Flap");
        _jumpingHash = Animator.StringToHash("Jumping");
        _groundedHash = Animator.StringToHash("IsGrounded");
    }

    // Update is called once per frame
    void Update()
    {
        float move =_moveAction.ReadValue<float>();

        if (move > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (move < 0)
        {
            _spriteRenderer.flipX = true;
        }

        CheckGrounded();
        SetAnimations();
    }

    private void SetAnimations()
    {

        
    }

    void FixedUpdate()
    {
        float move = _moveAction.ReadValue<float>();
        if(move == 0) 
        {
            PlayerAnimator.SetBool(_jumpingHash, !IsGrounded);
            return;
        }
        
        bool flying = _previousFlyTime + _flyInterval < Time.time;
        if(flying)
        {
            PlayerAnimator.SetTrigger(_flapHash);            
            Fly(move);
            _previousFlyTime = Time.time;
        }
        
    }

    private void Fly(float move)
    {
        Vector2 moveMagnitude = new Vector2(move * horizontalSpeed, verticalSpeed);
        Rb2D.AddForce(moveMagnitude, ForceMode2D.Impulse);

        Rb2D.linearVelocityX = Mathf.Clamp(Rb2D.linearVelocity.x, -horizontalSpeed, horizontalSpeed);
        Rb2D.linearVelocityY = Mathf.Clamp(Rb2D.linearVelocity.y, -verticalSpeed, verticalSpeed);
    }

    private void CheckGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, _groundCheckSize, 0f, Vector2.down, _rayDistance, _groundLayer);
        IsGrounded = hit.collider != null;
        PlayerAnimator.SetBool(_groundedHash, IsGrounded);

        hit = Physics2D.BoxCast(transform.position, _groundCheckSize, 0f, Vector2.down, _rayDistance, _balloonLayer);
        
        if (hit.collider == null){return;}
        
        Debug.Log("Balloon hit");
        
        if(!hit.collider.gameObject.TryGetComponent<Health>(out Health health)) { return; }
        
        Debug.Log("Balloon hit with health");
        health.TakeDamage(10);

    }

}
