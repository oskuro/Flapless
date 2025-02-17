using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class BalloonMovement : MonoBehaviour
{
    Transform _playerAnchor;
    [SerializeField] float _minSpeed = .1f;
    [SerializeField] float _maxSpeed = 3f;
    [SerializeField] float _maxDistance = 1f;
    [SerializeField] AnimationCurve _speedCurve;
    Vector3 _flyOffDir = Vector3.zero;
    Collider2D _collider;
    Rigidbody2D _rb;

    
    [HideInInspector] public float TimeToLive = 10f; // Set by BalloonSlot
    [SerializeField] LayerMask _ignorePlayerMask;

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
            FollowAnchor();
        }
    }

    private void FollowAnchor()
    {
        Vector3 targetPos = _playerAnchor.position;
        Vector3 direction = (targetPos - transform.position).normalized;
        float speed = _speedCurve.Evaluate(Vector3.Distance(transform.position, targetPos) / _maxDistance) * _maxSpeed;
        _rb.linearVelocity = direction * speed;
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
    }
}