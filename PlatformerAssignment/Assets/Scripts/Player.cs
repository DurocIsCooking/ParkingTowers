using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Character
{

    // Horizontal movement
    private float _horizontalInput;

    // Jumping
    private int _numberOfJumps;
    private int _maxJumps = 1;
    private bool _wantsToJump = false; // Stores player input for jumping. Needed since input is in Update and Jump is in FixedUpdate
    
    // Wall jump
    private bool _isTouchingWall = false; // Primarily used for wall jumps
    private bool _wallOnRight; // Returns true if a wall the player is touching is on the right. Returns false if wall is on left. Used to walljump away from wall

    // Wings upgrade
    [SerializeField] private GameObject _jumpIndicator;
    private bool _foundWings = false;

    private void Awake()
    {
        // Starting values
        SetNumberOfJumps(_maxJumps);

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
            SetNumberOfJumps(_numberOfJumps - 1);
        }
        _wantsToJump = false;
    }

    private void SetNumberOfJumps(int numJumps)
    {
        _numberOfJumps = numJumps;
        // Jump UI
            // Don't want to display first (grounded) jump, only double jumps
        if(numJumps == _maxJumps)
        {
            numJumps--;
        }
        // Indicate each jump by enabling one sprite
        foreach(Image jumpTracker in _jumpIndicator.GetComponentsInChildren<Image>())
        {
            if(numJumps-- > 0)
            {
                jumpTracker.enabled = true;
            }
            else
            {
                jumpTracker.enabled = false;
            }
        }
    }

    private void WallJump(bool wallOnRight)
    {
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

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // Reset player's jumps when they touch the floor
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            SetNumberOfJumps(_maxJumps);
        }

        
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            // Restore double jumps
            SetNumberOfJumps(_maxJumps - 1);

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


    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player walks off a ledge, they lose their first jump
        if (_numberOfJumps == _maxJumps && collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            SetNumberOfJumps(_numberOfJumps - 1);
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

    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        GameObject colObject = collider.gameObject;
        if(colObject.layer == LayerMask.NameToLayer("JumpUpgrade"))
        {
            // Bring upgrade to default layer so it no longer interacts with player
            colObject.layer = LayerMask.NameToLayer("Default");
            // Disable visuals, but allow current particles to finish their lifetime
            colObject.GetComponent<SpriteRenderer>().enabled = false;
            colObject.GetComponent<ParticleSystem>().Stop();
            // Apply jump upgrade
            UpgradeJump();
        }
    }
    private void UpgradeJump()
    {
        if (!_foundWings)
        {
            // Display wings text
            GameObject wingsText = _jumpIndicator.transform.GetChild(0).gameObject;
            if (!wingsText.activeSelf)
            {
                wingsText.SetActive(true);
            }
            // Enable player wing sprites
            foreach (SpriteRenderer spriteRenderer in transform.GetComponentsInChildren<SpriteRenderer>())
            {
                spriteRenderer.enabled = true;
            }
        }
        // Increase available jumps
        _maxJumps++;
        SetNumberOfJumps(_numberOfJumps + 1);
    }

    protected override void PlayDeathAnimation()
    {

    }
}
