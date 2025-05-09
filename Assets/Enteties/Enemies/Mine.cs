using System.Collections;
using UnityEngine;

public class Mine : Enemy
{
    [SerializeField] float _timeToBoom = 1f;
    [SerializeField] float _explosionRadius = 2f;
    [SerializeField] int _maxDamage = 10;

    Animator _animator;
    int _blinkSpeedParameter;
    int _blinkTriggerParameter;
    bool _countdownActive = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _blinkSpeedParameter = Animator.StringToHash("Speed");
        _blinkTriggerParameter = Animator.StringToHash("Blink");
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(_countdownActive == false)
        {
            StartCoroutine(Countdown());
        }    
    }

    IEnumerator Countdown() {
        _countdownActive = true;
        float timeStarted = Time.time;
        _animator.SetTrigger(_blinkTriggerParameter);
        var speed = 1f;
        while(Time.time < timeStarted + _timeToBoom)
        {
            yield return new WaitForEndOfFrame();
            _animator.SetFloat(_blinkSpeedParameter, speed);
            speed += Time.deltaTime;
        }

        Kaboom();
    }

    void Kaboom() {
        var colliders = Physics2D.OverlapCircleAll(transform.position, _explosionRadius);
        foreach(Collider2D col in colliders) {
            var health = col.gameObject.GetComponent<Health>();
            if(health) 
            {
                var distance = transform.position - (Vector3) col.ClosestPoint(transform.position);
                var damage = distance.magnitude / _explosionRadius * _maxDamage; 
                health.TakeDamage((int) damage);
            }
        }
        Destroy(gameObject);
    }
}
