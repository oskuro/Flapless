using System;
using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    private bool _isInvulnerable = false;

    public int CurrentHealth { get { return _currentHealth; } }
    int _currentHealth;
    bool _isDead = false;
    public Action<GameObject> OnDeath;
    public Action<Health> OnDamaged;
    public Action<Health> OnHealed;
    public Action<Vector2> OnKnockback;

    [SerializeField] Vector2 deathKnockback = default;

    [SerializeField] bool _debug = false;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage, Vector2 knockbackForce = default)
    {
        if(_debug)
            Debug.Log($"{gameObject.name} took {damage} damage");

        if (_isDead || !enabled || _isInvulnerable)
            return;

        _currentHealth -= damage;
        if (_currentHealth <= 0)
        {
            _isDead = true;
            
            if(OnDeath != null)
                OnDeath?.Invoke(gameObject);
            else
                DestroySelf();
        } else 
        {
            OnDamaged?.Invoke(this);
        }

        if(knockbackForce != default && OnKnockback != null)
        {
            OnKnockback(knockbackForce);
        }
    }

    private void DestroySelf()
    {
        if(deathKnockback != default)
        {
            var colliders = Physics2D.OverlapCircleAll(transform.position, 1f);

            foreach(Collider2D col in colliders)
            {
                if(col.gameObject.TryGetComponent<Health>(out Health health))
                {
                    health.TakeDamage(0, deathKnockback);
                }
            }
        }
        Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        if (_isDead || !enabled)
            return;

        _currentHealth += amount;
        _currentHealth = Math.Clamp(_currentHealth, 0, _maxHealth);
        OnHealed?.Invoke(this);
    }

    public void SetInvulnerable(float duration)
    {
        Debug.Log(gameObject.name + " Invunerable");
        if (!gameObject.activeInHierarchy) return;
        StartCoroutine(InvulnerabilityCoroutine(duration));
    }

    private IEnumerator InvulnerabilityCoroutine(float duration)
    {
        _isInvulnerable = true;
        yield return new WaitForSeconds(duration);
        _isInvulnerable = false;
        Debug.Log(gameObject.name + " vunerable");
    }
}
