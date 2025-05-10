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

    [SerializeField] private bool _debug = false;

    public Action OnNoBalloonsLeft;

    bool _shiftBallons = false;

    void Start()
    {
        //balloonSlots.AddRange(transform.GetComponentsInChildren<BalloonSlot>());
        balloonSlots.ForEach(bs => bs.onBalloonDeath += RemoveBalloon);

        rb2d = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player>();
        if (playerMovement)
        {
            SpawnBalloons(playerMovement.MaxBalloons);
        }
        playerMove = GetComponent<PlayerBalloonLift>();
        if (playerMove)
            SpawnBalloons(playerMove.MaxBalloons);
    }

    void Update()
    {
        if(_shiftBallons) {
            int numBalloons = balloonSlots.Count(slot => slot.Balloon != null);
            float balloonSize = 0.55f;
            var totalBalloonSpace = balloonSize * numBalloons;
            for(int i=0; i < numBalloons; i++)
            {
                var newXOffset = (balloonSize * i) + balloonSize - totalBalloonSpace;
                Debug.Log(newXOffset);
                var bPos = balloonSlots[i].transform.position;
                bPos.x = transform.position.x + newXOffset;
                balloonSlots[i].transform.position = bPos;
            }
            _shiftBallons = false;
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
            } else {
                bs.TriggerInvunerable();
            }

            var freeSlots = balloonSlots.Where(bs => bs.IsSlotFree()).Count();
            if (freeSlots == balloonSlots.Count) {
                OnNoBalloonsLeft?.Invoke();
                break;
            }
            
        }
        

        _shiftBallons = true;
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
                    bloon.transform.parent = playerMovement.gameObject.transform;
                if (playerMove)
                {
                    playerMove.AddBalloon();
                    bloon.transform.parent = playerMove.gameObject.transform;
                }
                break;
            }
        }
        _shiftBallons = true;
    }

    void OnDisable()
    {
        balloonSlots.ForEach(bs => bs.onBalloonDeath -= RemoveBalloon);
    }
}