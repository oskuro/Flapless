using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalloonManager : MonoBehaviour
{
    [SerializeField] LayerMask balloonLayerMask;
    [SerializeField] List<BalloonSlot> balloonSlots;
    int maxBalloons = 3;
    
    Rigidbody2D rb2d;
    Player playerMovement;

    void Start()
    {
        //balloonSlots.AddRange(transform.GetComponentsInChildren<BalloonSlot>());
        balloonSlots.ForEach(bs => bs.onBalloonDeath += RemoveBalloon);
        
        rb2d = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("BalloonManager: OnTriggerEnter2D");
        if (balloonLayerMask == (balloonLayerMask | (1 << collision.gameObject.layer)))
        {
            Debug.Log("BalloonManager: OnTriggerEnter2D - BalloonLayer");
            SpawnBalloon(collision.gameObject);
        }
    }

    public void RemoveBalloon(GameObject bloon)
    {
        foreach (BalloonSlot bs in balloonSlots)
        {
            if (bs.Balloon == bloon)
            {
                playerMovement.RemoveBalloon();
                bs.FreeBalloon();
                break;
            }
        }
    }

    public void SpawnBalloon(GameObject bloon)
    {
        var freeSlots = balloonSlots.Where(bs => bs.IsSlotFree()).Count();
        if (freeSlots <= 0)
            return;

        foreach (BalloonSlot bs in balloonSlots)
        {
            if (bs.IsSlotFree())
            {
                bs.AddBalloon(bloon);
                playerMovement.AddBalloon();
                break;
            }
        }

    }

    void OnDisable()
    {
        balloonSlots.ForEach(bs => bs.onBalloonDeath -= RemoveBalloon);
    }
}