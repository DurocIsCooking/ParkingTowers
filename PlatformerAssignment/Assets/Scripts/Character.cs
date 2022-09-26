using UnityEngine;

// Abstract class for players and cars.
public abstract class Character : MonoBehaviour
{
    //Rigidbody
    [SerializeField] protected Rigidbody2D _rigidbody;

    // Horizontal movement
    [SerializeField] protected float _horizontalAcceleration;
    [SerializeField] protected float _maxHorizontalVelocity;

    // Gravity scale, to play with floatiness
    [SerializeField] protected float _gravityScale;

    // Sets horizontal velocity with a cap
    protected void SetMovementSpeed(float horizontalVelocity)
    {
        // Make sure velocity does not exceed cap, in positive or negative
        if(Mathf.Abs(horizontalVelocity) > Mathf.Abs(_maxHorizontalVelocity))
        {
            if (horizontalVelocity > 0)
                horizontalVelocity = _maxHorizontalVelocity;
            else
                horizontalVelocity = -_maxHorizontalVelocity;
        }
        // Set speed
        _rigidbody.velocity = (new Vector2(horizontalVelocity, _rigidbody.velocity.y));
    }

    public abstract void Die();
    protected abstract void PlayDeathAnimation();

    // Both players and cars die on collision with hazards (spikes or other cars)
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Die();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Die();
        }
    }
    

}
