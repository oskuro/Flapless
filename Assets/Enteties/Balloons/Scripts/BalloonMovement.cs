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
    [SerializeField] float verticalSpeed = 4f;
    [SerializeField] float maxDistance = 1f;
    [SerializeField] float offsetMagnitude = 0.35f;
    [SerializeField] float offsetChangeRate = 0.5f;
    [SerializeField] AnimationCurve speedCurve;
    float offsetChangeTimer = 0f;
    Vector3 flyOffDir = Vector3.zero;
    Collider2D col;
    Rigidbody2D rb;

    // Time in seconds before the balloon flies away
    public float timeToLive = 10f;
    [SerializeField] LayerMask ignorePlayerMask;

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = true;
        flyOffDir = new Vector3((float)Random.Range(-1, 2), 0, 0f);
    }

    void Update()
    {
        if (playerAnchor == null)
        {
            rb.linearVelocity = (Vector3.up + flyOffDir);
        }
        else
        {
            FollowAnchorWithForce();
        }
    }

    /// <summary>
    /// Updates the balloon's position to follow the player's anchor point with a random horizontal offset.
    /// Adjusts the balloon's speed based on the distance to the anchor and a speed curve.
    /// </summary>
    private void FollowAnchor()
    {
        var distance = Vector3.Distance(playerAnchor.position, transform.position);

        Vector3 targetPosition = playerAnchor.position;
        Vector3 direction = (targetPosition - transform.position).normalized;
        var speed = Mathf.Lerp(minSpeed, maxSpeed, speedCurve.Evaluate(distance / maxDistance));

        Vector3 desiredVelocity = new Vector3(direction.x * speed, direction.y * speed * verticalSpeed, 0f); // Increase vertical speed

        // Apply damping to smooth out the movement and reduce overshooting
        float dampingFactor = 0.9f; // Adjust this value as needed
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, desiredVelocity, Time.deltaTime * dampingFactor);
    }

    private void FollowAnchorWithForce()
    {
        var distance = Mathf.Abs(Vector3.Distance(playerAnchor.position, transform.position));
        if (playerAnchor == null) return;

        Vector3 targetPosition = playerAnchor.position;
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Calculate the force to apply
        float forceMagnitude = Mathf.Lerp(minSpeed, maxSpeed, speedCurve.Evaluate(distance / maxDistance));
        Vector3 force = new Vector3(direction.x * verticalSpeed, direction.y * verticalSpeed, 0f);

        // Apply the force to the Rigidbody2D
        rb.AddForce(force);

        // Adjust the y position to be more precise
        Vector3 currentPosition = transform.position;
        //currentPosition.y = Mathf.Lerp(currentPosition.y, targetPosition.y, Time.deltaTime * verticalSpeed);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * verticalSpeed);
        transform.position = currentPosition;
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