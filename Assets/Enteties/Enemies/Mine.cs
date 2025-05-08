using System.Collections;
using UnityEngine;

public class Mine : Enemy
{
    [SerializeField] float _timeToBoom = 1f;
    [SerializeField] float _explosionRadius = 2f;

    bool _countdownActive = false;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_countdownActive == false)
        {
            StartCoroutine(Kaboom());
        }    
    }

    IEnumerator Kaboom() {
        _countdownActive = true;
        float timeStarted = Time.time;
        Debug.Log("Mine started countdown");
        while(Time.time < timeStarted + _timeToBoom)
        {
            yield return new WaitForEndOfFrame();
        }

        Debug.Log("Boom! " + _explosionRadius);
    }
}
