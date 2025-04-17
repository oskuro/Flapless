using UnityEngine;
using UnityEngine.Tilemaps;

public class PatrollingEnemy : Enemy
{

    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private Tilemap _tilemap;
    private Vector3 _startPos;
    private Vector3 _patrolPos;
    private Vector3 _targetPos;

    void Start()
    {
        _startPos = transform.position;
        var hit = Physics2D.Raycast(_startPos, Vector2.right, _groundMask);
        if(hit)
        {
            _tilemap = hit.collider.gameObject.GetComponent<Tilemap>();
            var gridPos = _tilemap.WorldToCell(transform.position);
            var newPos = gridPos + new Vector3Int(5, 0, 0);

            var tile = _tilemap.GetTile(newPos);

            if(tile == null)
                _patrolPos = _tilemap.CellToWorld(newPos);
            // _patrolPos = hit.point - Vector2.right;
            _targetPos = _patrolPos;
        }    
    }

    void Update()
    {
        var distanceToTarget = (transform.position - _targetPos).magnitude;
        if(distanceToTarget < 0.1f)
        {
            if(_targetPos == _patrolPos)
                _targetPos = _startPos;
            else
                _targetPos = _patrolPos;
        }

        var targetDirection = (_targetPos - transform.position).normalized;

        transform.Translate(_speed * Time.deltaTime * targetDirection);
        //OnDeath(this);
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        var health = collision.gameObject.GetComponent<Health>();

        if(health)
            health.TakeDamage(_damage);
    }
}