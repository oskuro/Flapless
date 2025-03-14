using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerBalloonLift : MonoBehaviour
{
    [SerializeField] private int _maxBalloons = 3;
    public int Balloons {
        get {return _balloons;}
        set 
        {
            _balloons = value;
            _rB2D.gravityScale = 1f * ((float)_maxBalloons / (float)value);
        }

    }
    [SerializeField] private int _balloons = 2;
    [SerializeField] float _horizontalForce;
    [SerializeField] float _verticalForce;
    private Rigidbody2D _rB2D;
    InputAction _moveAction;
    InputAction _jumpAction;
    void Start()
    {
        _rB2D = GetComponent<Rigidbody2D>();

        _moveAction = InputSystem.actions.FindAction("Move");
        _jumpAction = InputSystem.actions.FindAction("Jump");

    }

    void Update()
    {
        Balloons = _balloons;
        float horizontal =_moveAction.ReadValue<float>();
        _jumpAction.IsPressed();
        if(_jumpAction.WasPressedThisFrame()) 
        {
            Debug.Log("Jump");
            Vector2 movement = new Vector2(horizontal * _horizontalForce, 1f * _verticalForce);
            _rB2D.AddForce(movement, ForceMode2D.Impulse);
        }
    }
}
