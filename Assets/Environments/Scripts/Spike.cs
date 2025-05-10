using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage = 10;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Health>(out Health health))
        {
            health.TakeDamage(damage);
        }


    }
}
