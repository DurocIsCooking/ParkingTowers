using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnemy : Character
{
    public bool IsFacingRight;
    [SerializeField] private GameObject _carExplosion;

    public void SetSpeed(float acceleration, float maxVelocity)
    {
        _horizontalAcceleration = acceleration;
        _maxHorizontalVelocity = maxVelocity;
        if (!IsFacingRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void FixedUpdate()
    {
        // Add acceleration to current velocity
        float horizontalVelocity = Mathf.Abs(_rigidbody.velocity.x) + _horizontalAcceleration;

        if (!IsFacingRight)
        {
            horizontalVelocity = -horizontalVelocity;
        }

        // Move
        SetMovementSpeed(horizontalVelocity);
        
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        GameObject collisionObject = collision.collider.gameObject;

        if (collisionObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Die();
        }

    }

    public override void Die()
    {
        // Disable colliders and play animation
        foreach(Collider2D collider in GetComponentsInChildren<Collider2D>())
        {
            collider.enabled = false;
        }
        // Stop car from moving while death animation plays
        _rigidbody.gravityScale = 0;
        _rigidbody.velocity = Vector2.zero;
        _rigidbody.angularVelocity = 0;
        _horizontalAcceleration = 0;

        PlayDeathAnimation();
    }

    protected override void PlayDeathAnimation()
    {
        GetComponent<Animator>().SetBool("destroyed", true);
        Instantiate(_carExplosion, transform.position, Quaternion.identity);
    }

    // Called by an animation event
    public void DestroyCar()
    {
        Destroy(gameObject);
    }
}
