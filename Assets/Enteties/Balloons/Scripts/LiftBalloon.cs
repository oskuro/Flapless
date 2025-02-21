using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class LiftBalloon : MonoBehaviour
{
    [SerializeField] float _liftForce;
    Rigidbody2D _rb2d;
    
    void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();    
    }

    void FixedUpdate()
    {
        _rb2d.AddForce(Vector2.up * _liftForce);    
    }


}
