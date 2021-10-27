using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEnemy : Character
{
    [SerializeField] private bool _isFacingRight;
    [SerializeField] private GameObject _carExplosion;

    private void Awake()
    {
        if(!_isFacingRight)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
    }

    private void FixedUpdate()
    {
        // Add acceleration to current velocity
        float horizontalVelocity = Mathf.Abs(_rigidbody.velocity.x) + _horizontalAcceleration;

        if (!_isFacingRight)
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

    protected override void PlayDeathAnimation()
    {
        Instantiate(_carExplosion, transform.position, Quaternion.identity);
    }
}
