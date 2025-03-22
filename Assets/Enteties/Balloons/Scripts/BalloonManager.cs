using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    [SerializeField] LayerMask balloonLayerMask;
    [SerializeField] List<BalloonSlot> balloonSlots;
    [SerializeField] GameObject _balloonPrefab;

    Rigidbody2D rb2d;
    Player playerMovement;
    PlayerBalloonLift playerMove;

    private bool _debug = false;

    public Action OnNoBalloonsLeft;

    void Start()
    {
        //balloonSlots.AddRange(transform.GetComponentsInChildren<BalloonSlot>());
        balloonSlots.ForEach(bs => bs.onBalloonDeath += RemoveBalloon);

        rb2d = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player>();
        if (playerMovement == null)
        {
            playerMove = GetComponent<PlayerBalloonLift>();
            if (playerMove)
                SpawnBalloons(playerMove.MaxBalloons);
        }
    }

    private void SpawnBalloons(int balloons)
    {
        for (int i = 0; i < balloons; i++)
        {
            SpawnBalloon();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_debug)
            Debug.Log("BalloonManager: OnTriggerEnter2D");
        if (balloonLayerMask == (balloonLayerMask | (1 << collision.gameObject.layer)))
        {
            if (_debug)
                Debug.Log("BalloonManager: OnTriggerEnter2D - BalloonLayer");
            if (collision.gameObject.GetComponent<BalloonMovement>() != null)
                SpawnBalloon(collision.gameObject);
        }
    }

    public void RemoveBalloon(GameObject bloon)
    {
        foreach (BalloonSlot bs in balloonSlots)
        {
            if (bs.Balloon == bloon)
            {
                if (playerMovement)
                    playerMovement.RemoveBalloon();
                if (playerMove)
                    playerMove.RemoveBalloon();
                bs.FreeBalloon();
                var freeSlots = balloonSlots.Where(bs => bs.IsSlotFree()).Count();
                if (freeSlots == balloonSlots.Count)
                    OnNoBalloonsLeft?.Invoke();
                break;
            }
        }
    }

    public void SpawnBalloon(GameObject bloon = null)
    {
        var freeSlots = balloonSlots.Where(bs => bs.IsSlotFree()).Count();
        if (freeSlots <= 0)
            return;

        foreach (BalloonSlot bs in balloonSlots)
        {
            if (bs.IsSlotFree())
            {
                if (bloon == null)
                {
                    bloon = Instantiate(_balloonPrefab, transform.position, Quaternion.identity);
                }
                bs.AddBalloon(bloon);
                if (playerMovement)
                    playerMovement.AddBalloon();
                if (playerMove)
                {
                    playerMove.AddBalloon();
                }
                break;
            }
        }

    }

    void OnDisable()
    {
        balloonSlots.ForEach(bs => bs.onBalloonDeath -= RemoveBalloon);
    }
}