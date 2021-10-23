using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{

    // Horizontal movement
    private float _horizontalInput; // Horizontal movement input

    // Jumping
    private int _numberOfJumps; // Player's current number of jumps
    private int _maxJumps = 2; // Player's maximum number of jumps
    private bool _wantsToJump = false; // Stores player input for jumping. Needed since input is in Update and Jump is in FixedUpdate
    
    // Wall jump
    private bool _isTouchingWall = false; // Tracks whether player can wall jump
    private bool _wallOnRight; // Returns true if a wall the player is touching is on the right. Returns false if wall is on left. Used to walljump away from wall

    //Friction manipulation
    [SerializeField] private PhysicsMaterial2D _physicsMaterial;
    private float _desiredFriction = 0.4f;
    private bool _isTouchingSomething;

    private void Awake()
    {
        // Starting values
        _numberOfJumps = _maxJumps;

        // Pointers
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
        //ManageFriction();
    }

    private void ManageFriction()
    {
        // While the player is sliding along the floor, we want them to have friction so they comes to a stop naturally
        // However, when the player lands on the floor, we want a friction of 0, because otherwise the player loses speed on impact (it's jarring)
        Debug.Log(_isTouchingSomething);
        if(_isTouchingSomething)
        {
            gameObject.GetComponent<BoxCollider2D>().sharedMaterial.friction = _desiredFriction;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }
        else
        {
            gameObject.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0;
            //gameObject.GetComponent<BoxCollider2D>().enabled = false;
            //gameObject.GetComponent<BoxCollider2D>().enabled = true;

        }
    }

    private void ManageHorizontalMovement()
    {
        // Add acceleration to current velocity
        float horizontalVelocity = _rigidbody.velocity.x + _horizontalInput * _horizontalAcceleration;
        
        // Move
        SetMovementSpeed(horizontalVelocity);

        // Reset friction if touching something
    }

    private void ManageJump()
    {
        // Wall jump has priority over regular jumps and does not drain jump counter
        if(_isTouchingWall && _wantsToJump)
        {
            WallJump(_wallOnRight);
        }
        else if(_numberOfJumps > 0 && _wantsToJump)
        {
            Jump();
            _numberOfJumps--;
        }
        _wantsToJump = false;
    }

    private void WallJump(bool wallOnRight)
    {
        Debug.Log("Walljump");
        // No longer touching wall
        _isTouchingWall = false;
        // Wall jump sends the player outwards from the wall in addition to jumping normally
        Jump();

        float wallJumpSpeed = -10f;
        if(!wallOnRight)
        {
            wallJumpSpeed = -wallJumpSpeed;
        }
        SetMovementSpeed(wallJumpSpeed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Reset player's jumps when they touch the floor
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            _numberOfJumps = _maxJumps;
        }

        
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Toggle whether player is touching a wall
            _isTouchingWall = true;
            // Set player's horizontal speed to 0
            _rigidbody.velocity = new Vector2(0, _rigidbody.velocity.y);
            // Store whether wall is on right or left, to apply WallJump in correct direction
            _wallOnRight = (collision.GetContact(0).point.x > transform.position.x);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Used for friction manip
        //_isTouchingSomething = true;
        //Invoke("ResetFriction", 2);
    }

    private void ResetFriction()
    {
        Debug.Log("reset friction");
        gameObject.GetComponent<BoxCollider2D>().sharedMaterial.friction = _desiredFriction;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Used for friction manip
        //_isTouchingSomething = false;
        //gameObject.GetComponent<BoxCollider2D>().sharedMaterial.friction = 0;

        // If the player walks off a ledge, they lose their first jump
        if (_numberOfJumps == _maxJumps && collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            _numberOfJumps--;
        }

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Disable wall jump after a slight buffer
            Invoke("DisableWallJump", 0.08f);
        }
    }

    private void DisableWallJump()
    {
        _isTouchingWall = false;
    }    
}
