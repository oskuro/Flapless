using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class BalloonMovement : MonoBehaviour
{
    Transform _playerAnchor;
    //[SerializeField] float _minSpeed = .1f;
    [SerializeField] float _maxSpeed = 3f;
    [SerializeField] float _maxDistance = 1f;
    [SerializeField] AnimationCurve _speedCurve;
    Vector3 _flyOffDir = Vector3.zero;
    Collider2D _collider;
    Rigidbody2D _rb;

    
    [HideInInspector] public float TimeToLive = 10f; // Set by BalloonSlot
    [SerializeField] LayerMask _ignorePlayerMask;
    [SerializeField] float _dampingFactor = 0.35f;

    void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.simulated = true;
        _flyOffDir = new Vector3((float)Random.Range(-1, 2), 0, 0f);

        
    }

    void Update()
    {
        
    }

    void FixedUpdate() {
        if (_playerAnchor == null)
        {
            _rb.linearVelocity = (Vector3.up + _flyOffDir);
        }
        else
        {
            //  FollowAnchor();
        }
    }

    private void FollowAnchor()
    {
    Vector3 direction = (_playerAnchor.position - transform.position);
    float distance = direction.magnitude;
    direction.Normalize();

    float stoppingDistance = 0.5f; // Distance at which we start damping
    float forceStrength = 10f; // Adjust as needed

    // Scale force based on distance
    float speedFactor = Mathf.Clamp01(distance / stoppingDistance); 

    _rb.AddForce(direction * forceStrength * speedFactor);

    // Optionally, dampen velocity when very close to stop overshooting
    if (distance < stoppingDistance)
    {
        _rb.linearVelocity *= 0.9f; // Slow down smoothly
    }
    }

    public void Free()
    {
        _playerAnchor = null;
        _collider.enabled = false;
    }

    public void Fly(Transform anchor)
    {
        gameObject.layer = 0;
        _collider.excludeLayers = _ignorePlayerMask;
        _playerAnchor = anchor;

        // SpringJoint2D joint = gameObject.AddComponent<SpringJoint2D>();
        // joint.connectedBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        // joint.autoConfigureDistance = false;
        // joint.distance = 1; // How high above the player it should stay
        // joint.dampingRatio = 0.5f; // Adjust for smooth movement
        // joint.frequency = 2f; // Adjust for tightness of the "lift"
        transform.position = _playerAnchor.position;
        HingeJoint2D joint = gameObject.AddComponent<HingeJoint2D>();
        joint.connectedBody = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>();
        joint.anchor = _playerAnchor.position;
        joint.enableCollision = true;
        joint.useLimits = true;
        
        JointAngleLimits2D limits = new JointAngleLimits2D();
        limits.min = -7.5f; // Allow a slight tilt
        limits.max = 7.5f;
        joint.limits = limits;

    }
}