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
        PlayDeathAnimation();
        Destroy(gameObject);
    }

    protected override void PlayDeathAnimation()
    {
        Instantiate(_carExplosion, transform.position, Quaternion.identity);
    }
}
