using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RopeManager : MonoBehaviour
{
    LineRenderer lineRenderer;
    Transform player;
    [SerializeField] float ropeWidth = 0.1f;
    [SerializeField] Transform _target;
    Vector3[] positions = new Vector3[2];

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if(_target == null)
            _target = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        positions[0] = transform.position;
        positions[1] = _target.position;

        lineRenderer.SetPositions(positions);

        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
    }



    void OnDisable()
    {
        lineRenderer.enabled = false;
    }
}