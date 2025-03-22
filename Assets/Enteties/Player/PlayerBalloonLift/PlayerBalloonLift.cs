using System;
using UnityEngine;
using UnityEngine.InputSystem;
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
            var newGravity = 1f * ((float)MaxBalloons / (float)_balloons);
            if (newGravity != Mathf.Infinity) // The value might be infinite if _ballons are 0
                _rB2D.gravityScale = newGravity;

        }

    }
    [SerializeField] private int _balloons = 2;
    [SerializeField] float _horizontalForce;
    [SerializeField] float _verticalForce;
    private Rigidbody2D _rB2D;
    InputAction _moveAction;
    InputAction _jumpAction;

    void Awake()
    {
        _rB2D = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");
    }

    void Update()
    {
        Balloons = _balloons;
        float horizontal = _moveAction.ReadValue<float>();

        if (_jumpAction.WasPressedThisFrame())
        {
            Vector2 movement = new Vector2(horizontal * _horizontalForce, 1f * _verticalForce);
            _rB2D.AddForce(movement, ForceMode2D.Impulse);
        }
    }

    public void RemoveBalloon()
    {
        Balloons--;
        if (_balloons <= 0 && OnDeath != null)
        {
            this.enabled = false;
            OnDeath();
        }
    }

    public void AddBalloon()
    {
        Balloons++;
    }
}
