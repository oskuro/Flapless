using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;
public class PlayerBalloonLift : MonoBehaviour
{
    [SerializeField] public int MaxBalloons = 3;


    public Action OnDeath;
    public int Balloons
    {
        get { return _balloons; }
        set
        {
            _balloons = value;

            var newGravity = _baseGravity - (_balloonLiftStrength * (float) _balloons);
            _rB2D.gravityScale = newGravity;
        }

    }
    [SerializeField]
    private int _balloons = 2;
    [SerializeField]
    private float _horizontalForce;
    [SerializeField]
    private float _verticalForce;
    private Rigidbody2D _rB2D;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    [SerializeField]
    private float _balloonLiftStrength;
    [SerializeField]
    private float _baseGravity = 1.2f;
    [SerializeField]
    private float _flapInterval = 0.5f;
    private float _previousFlapTime = 0;

    void Awake()
    {
        _rB2D = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");

    }

    void Update()
    {
        Flap();
    }

    private void Flap()
    {
        // Are we allowed to flap?
        if(_previousFlapTime + _flapInterval > Time.time)
            return;

        // Do we want to flap
        if(_jumpAction.IsPressed() == false)
            return;
        
        float moveInput = _moveAction.ReadValue<float>();
        _previousFlapTime = Time.time;
        Vector2 movement = new Vector2(moveInput * _horizontalForce, _verticalForce);
        _rB2D.AddForce(movement, ForceMode2D.Impulse);
    }

    public void RemoveBalloon()
    {
        Balloons--;
        if (_balloons <= 0 && OnDeath != null)
        {
            OnDeath();
            this.enabled = false;
        }
    }

    public void AddBalloon()
    {
        Balloons++;
    }
}
