using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class BalloonMovement : MonoBehaviour
{
    Transform _playerAnchor;
    //[SerializeField] float _minSpeed = .1f;
    [SerializeField] float _maxSpeed = 3f;
    [SerializeField] AnimationCurve _speedCurve;
    Vector3 _flyOffDir = Vector3.zero;
    Collider2D _collider;
    Rigidbody2D _rb;

    Vector2 _extrapolatedPosition = Vector2.zero;

    
    [HideInInspector] public float TimeToLive = 10f; // Set by BalloonSlot
    [SerializeField] LayerMask _ignorePlayerMask;
    [SerializeField] float _predictionTime = 0.3f;
    Animator _animator;

    int _popHash;
    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = true;
        _flyOffDir = new Vector3((float)Random.Range(-1, 2), 0, 0f);
        _animator = GetComponentInChildren<Animator>();
        _popHash = Animator.StringToHash("Pop");
        
    }

    void Update()
    {
        
    }

    void FixedUpdate() {
        if (_playerAnchor == null)
        {
            _rb.linearVelocity = ((Vector3.up + _flyOffDir) * _maxSpeed);
        }
        else
        {
            FollowAnchor();
            //PredictPosition(_rb, _predictionTime);
        }
    }

    void PredictPosition(Rigidbody2D _rigidbody, float _predictionTime){
        //get the rigidbodies velocity
        Vector2 _targetVelocity = _rigidbody.linearVelocity;
        //multiply it by the amount of seconds you want to see into the future
        _targetVelocity *= _predictionTime;
        //add it to the rigidbodies position
       // _targetVelocity += _rigidbody.position;
        //Return the position of where the target will be in the amount of seconds you want to see into the future
        _extrapolatedPosition = _targetVelocity + _rigidbody.position;
    }

    private void FollowAnchor()
    {
        Vector3 direction = (_playerAnchor.position - transform.position);
        float distance = direction.magnitude;
        direction.Normalize();

        float stoppingDistance = 0.5f; // Distance at which we start damping

        var futureDistance = Vector2.Distance(_extrapolatedPosition, _playerAnchor.position);
        // Scale force based on distance
        float speedFactor = Mathf.Clamp01(distance / stoppingDistance); 
        speedFactor = _speedCurve.Evaluate(speedFactor);
        //Debug.Log("Speed Factor: " + speedFactor);
        _rb.linearVelocity = direction * _maxSpeed * speedFactor;
        //_rb.AddForce(direction * forceStrength * speedFactor);

    }

    public void Free()
    {
        _playerAnchor = null;
        _collider.enabled = false;
        _animator.SetTrigger(_popHash);
    }

    public void Fly(Transform anchor)
    {
        gameObject.layer = 0;
        _collider.excludeLayers = _ignorePlayerMask;
        _playerAnchor = anchor;
    }

    void OnDrawGizmos() 
    {
        if(_extrapolatedPosition != Vector2.zero)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(_rb.position, _extrapolatedPosition);
        }
    }
}