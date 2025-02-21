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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        Rb2D = GetComponent<Rigidbody2D>();

        PlayerAnimator = GetComponentInChildren<Animator>();

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
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
    }

    void FixedUpdate()
    {
        float move = _moveAction.ReadValue<float>();
        if(move == 0)
            return;
        
        if(_previousFlyTime + _flyInterval < Time.time)
        {
            Fly(move);
            _previousFlyTime = Time.time;
        }
        
    }

    private void Fly(float move)
    {
        
        Vector2 moveMagnitude = new Vector2(move * horizontalSpeed, verticalSpeed);
        Rb2D.AddForce(moveMagnitude, ForceMode2D.Impulse);

        //Rb2D.linearVelocity = Vector2.ClampMagnitude(Rb2D.linearVelocity, 10f);
        Rb2D.linearVelocityX = Mathf.Clamp(Rb2D.linearVelocity.x, -horizontalSpeed, horizontalSpeed);
        Rb2D.linearVelocityY = Mathf.Clamp(Rb2D.linearVelocity.y, -verticalSpeed, verticalSpeed);
    }
}
