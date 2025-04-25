using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PatrollingEnemy : Enemy
{

    [SerializeField] private float _speed;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask _groundMask;

    [SerializeField] private Tilemap _tilemap;

    private Vector3 _leftBound;
    private Vector3 _rightBound;
    private Vector3 _targetPos;


    private int _range = 10;

    void Start()
    {
        GetComponents();
        SetPatrolPositions();   
    }

    void Update()
    {
        MoveTowardsPatrolPoint();
    }

    private void MoveTowardsPatrolPoint()
    {
        float distance = Vector3.Distance(transform.position, _targetPos);

        if (distance < 1f)
        {
            // Switch direction
            if (_targetPos == _rightBound)
                _targetPos = _leftBound;
            else
                _targetPos = _rightBound;
        }

        Vector3 dir = (_targetPos - transform.position).normalized;
        transform.Translate(new Vector3(_speed * Time.deltaTime * dir.x, 0, 0));

        // Flip sprite
        if (dir.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(dir.x), 1, 1);
    }

    private void SetPatrolPositions()
    {
        var currentCell = _tilemap.WorldToCell(transform.position);

        _leftBound = _tilemap.CellToWorld(GetPatrolCell(currentCell, -1));
        _rightBound = _tilemap.CellToWorld(GetPatrolCell(currentCell, 1));

        _targetPos = _leftBound;
    }

    private Vector3Int GetPatrolCell(Vector3Int startCell, int direction)
    {
        Vector3Int lastValid = startCell;
        for (int i = 1; i <= _range; i++)
        {
            Vector3Int check = startCell + new Vector3Int(i * direction, 0, 0);
            if (_tilemap.GetTile(check) == null)
                lastValid = check;
            else
                break;
        }

        return lastValid;
    }

    private void GetComponents()
    {
        _tilemap = transform.parent.GetComponentInChildren<Tilemap>();

        if(_tilemap == null )
        {
            Debug.LogWarning("Could not find tilemap", this);
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.gameObject.name);
        var health = collision.gameObject.GetComponent<Health>();

        if(health)
            health.TakeDamage(_damage);
    }
}