using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //Rigidbody
    [SerializeField] protected Rigidbody2D _rigidbody;

    // Horizontal movement (these might not be needed here hmmmmm)
    [SerializeField] protected float _horizontalAcceleration;
    [SerializeField] protected float _maxHorizontalVelocity;

    // Jumping
    [SerializeField] protected float _jumpVelocity; // How much to increase player's y velocity on jump
    [SerializeField] protected float _gravityScale; // Gravity scale, to play with floatiness


    protected void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpVelocity);
    }
    
    //protected void WallJump()

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

    private void Die()
    {
        PlayDeathAnimation();
        Destroy(gameObject);
    }

    private void PlayDeathAnimation()
    {

    }
}
