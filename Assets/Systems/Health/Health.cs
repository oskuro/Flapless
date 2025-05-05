using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    public int CurrentHealth { get { return _currentHealth; } }
    int _currentHealth;
    bool _isDead = false;
    public Action<GameObject> OnDeath;
    public Action<Health> OnDamaged;
    public Action<Health> OnHealed;

    [SerializeField] bool _debug = false;

    void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(_debug)
            Debug.Log($"{gameObject.name} took {damage} damage");

        if (_isDead || !enabled)
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
    }

    private void DestroySelf()
    {
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
}
