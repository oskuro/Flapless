using System;
using UnityEngine;

public class BalloonSlot : MonoBehaviour
{
    private BalloonMovement balloon;
    private RopeManager rope;
    private Health health;

    public Action<GameObject> onBalloonDeath;
    float balloonTimeToLive = 10;
    public GameObject Balloon
    {
        get
        {
            if (balloon == null)
                return null;
            return balloon.gameObject;
        }
    }

    public bool IsSlotFree()
    {
        if (balloon == null)
            return true;
        return false;
    }

    public void AddBalloon(GameObject b)
    {
        if (IsSlotFree())
        {
            balloon = b.GetComponent<BalloonMovement>();
            rope = b.GetComponent<RopeManager>();
            health = b.GetComponent<Health>();
            health.onDeath += BalloonPopped;
            balloon.enabled = true;
            rope.enabled = true;
            balloon.Fly(transform);
            balloonTimeToLive = Time.fixedTime + balloon.timeToLive;
        }
    }


    public void BalloonPopped(GameObject bloon)
    {
        if (bloon == balloon.gameObject)
        {
            health.onDeath -= BalloonPopped;
            onBalloonDeath?.Invoke(balloon.gameObject);
        }
    }

    public void FreeBalloon()
    {
        balloon.Free();
        balloon = null;

        rope.enabled = false;
        rope = null;
    }
}