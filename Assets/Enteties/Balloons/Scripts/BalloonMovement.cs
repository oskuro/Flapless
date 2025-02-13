using System;
using UnityEngine;
using Random = UnityEngine.Random;

[SelectionBase]
public class BalloonMovement : MonoBehaviour
{
    Transform playerAnchor;
    Vector3 anchorOffset = Vector3.zero;
    [SerializeField] float minSpeed = .1f;
    [SerializeField] float maxSpeed = 3f;
    float verticalSpeed = 4f;
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float offsetMagnitude = 0.35f;
    [SerializeField] float offsetChangeRate = 0.5f;
    [SerializeField] AnimationCurve speedCurve;
    float offsetChangeTimer = 0f;
    Vector3 flyOffDir = Vector3.zero;
    Collider2D col;

    // Time in seconds before the balloon flies away
    public float timeToLive = 10f;
    [SerializeField] LayerMask ignorePlayerMask;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        flyOffDir = new Vector3((float)Random.Range(-1, 2), 0, 0f);
    }

    void Update()
    {
        if (playerAnchor != null)
        {
            FollowAnchor();
        }
        else
        {
            transform.position += (Vector3.up + flyOffDir) * Time.deltaTime;
        }
    }

    private void FollowAnchor()
    {
        var distance = Vector3.Distance(playerAnchor.position + anchorOffset, transform.position);
        if (Time.time - offsetChangeTimer > offsetChangeRate)
        {
            // anchorOffset = new Vector3(Random.Range(-offsetMagnitude, offsetMagnitude), 
            //    Random.Range(-offsetMagnitude, offsetMagnitude), 0f);
            anchorOffset = new Vector3(Random.Range(-offsetMagnitude, offsetMagnitude), 0, 0f);
            offsetChangeTimer = Time.time;
        }

        Vector3 direction = (playerAnchor.position + anchorOffset - transform.position).normalized;
        var speed = Mathf.Lerp(minSpeed, maxSpeed, speedCurve.Evaluate(distance / maxDistance));
        Vector3 translation = new Vector3(direction.x * speed, direction.y * verticalSpeed, 0f);

        transform.Translate(translation * Time.deltaTime);
    }

    public void Free()
    {
        playerAnchor = null;
        col.enabled = false;
    }

    public void Fly(Transform anchor)
    {
        gameObject.layer = 0;
        col.excludeLayers = ignorePlayerMask;
        playerAnchor = anchor;
    }
}