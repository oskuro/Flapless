using UnityEngine;
using UnityEngine.Tilemaps;

public class PatrollingEnemy : Enemy
{
    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private int _patrolDistance = 10;

    private Vector3 _startPos;
    private Vector3 _patrolPos;
    private Vector3 _targetPos;

    void Start()
    {
        _startPos = transform.position;
        _patrolPos = _startPos;

        // Search for ground tiles to the right up to _patrolDistance tiles
        for (int i = 1; i <= _patrolDistance; i++)
        {
            Vector3 checkPos = _startPos + Vector3.right * i;
            Vector3Int tilePos = _groundTilemap.WorldToCell(checkPos);

            if (_groundTilemap.GetTile(tilePos) == null)
            {
                // Step back to last tile that was ground
                _patrolPos = _startPos + Vector3.right * (i - 1);
                break;
            }

            // If we reach max distance and still on ground, patrol to that edge
            if (i == _patrolDistance)
                _patrolPos = checkPos;
        }

        _targetPos = _patrolPos;
    }

    void Update()
    {
        var distanceToTarget = (transform.position - _targetPos).magnitude;
        if (distanceToTarget < 0.1f)
        {
            _targetPos = (_targetPos == _patrolPos) ? _startPos : _patrolPos;
        }

        var targetDirection = (_targetPos - transform.position).normalized;
        transform.Translate(_speed * Time.deltaTime * targetDirection);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();
        if (health)
            health.TakeDamage(_damage);
    }
}
