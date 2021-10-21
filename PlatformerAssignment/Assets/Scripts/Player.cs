using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D _rigidbody;

    // Horizontal movement
    private float _horizontalInput; // Horizontal movement input
    [SerializeField] private float _horizontalSpeed; // Horizontal speed

    // Jumping
    private int _numberOfJumps; // Player's current number of jumps
    private int _maxJumps = 2; // Player's maximum number of jumps
    [SerializeField] private float _jumpVelocity; // How much to increase player's y velocity on jump
    [SerializeField] private float _gravityScale; // Player's gravity scale, to play with floatiness
    bool _wantsToJump = false; // Stores player input for jumping. Needed since input is in Update and Jump is in FixedUpdate

    private void Awake()
    {
        _numberOfJumps = _maxJumps;
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = _gravityScale;
    }

    private void Update()
    {
        CollectInput();
    }

    private void CollectInput()
    {
        // Get horizontal input with axis controls
        _horizontalInput = Input.GetAxis("Horizontal");

        // Check to see if player wants to jump
        if (Input.GetButtonDown("Jump"))
        {
            _wantsToJump = true;
        }
    }

    private void FixedUpdate()
    {
        // Manage movement based on input
        ManageHorizontalMovement();
        ManageJump();
        //rb.addforce OR
        //rb.velocity
    }

    private void ManageHorizontalMovement()
    {
        _rigidbody.AddForce(new Vector2(_horizontalInput * _horizontalSpeed, 0));
    }

    private void ManageJump()
    {
        if(_numberOfJumps > 0 && _wantsToJump)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpVelocity);
            _numberOfJumps--;
            Debug.Log("Jump");
        }
        _wantsToJump = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            _numberOfJumps = _maxJumps;
            Debug.Log("Refresh jump");
            Debug.Log("Number of jumps: " + _numberOfJumps);
        }
    }
}
