using UnityEngine;
using UnityEngine.UI;

// This script controls the player's input, movement, and animations. Inherits from Character.
public class Player : Character
{
    // Horizontal movement
    private float _horizontalInput;

    // Jumping
    private int _numberOfJumps;
    public static int MaxJumps = 1;
    private int _jumpCap = 4; // Highest number of jumps the player can get access to
    private bool _wantsToJump = false; // Stores player input for jumping. Needed since input is in Update and Jump is in FixedUpdate
    [SerializeField] protected float _jumpVelocity;

    // Wall jump
    private bool _isTouchingWall = false; // Primarily used for wall jumps
    private bool _isTouchingSurface = false; // Used to make sure player cannot wall jump when touching a surface (sometimes player can clip through a surface and touch a wall)
    private bool _wallOnRight; // Returns true if a wall the player is touching is on the right. Returns false if wall is on left. Used to walljump away from wall

    // Wings upgrade
    private GameObject _jumpIndicator;
    private bool _foundWings = false;

    // Animation
    [SerializeField] private Animator[] _wingsAnimators;
    [SerializeField] private GameObject[] _wings;

    // Respawn point
    public static Vector3 RespawnPoint;

    private void Awake()
    {
        // Starting values
        if (MenuManager.Instance != null)
        {
            _jumpIndicator = MenuManager.Instance.JumpIndicator;
        }
        SetNumberOfJumps(MaxJumps);
        
        // Rigidbody
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _rigidbody.gravityScale = _gravityScale;

        // Animation
        _wingsAnimators[0].SetBool("isLeftWing", true);
        _wingsAnimators[1].SetBool("isLeftWing", false);

        if(MaxJumps > 1)
        {
            EnableWingSprites();
        }
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
    }

    private void ManageJump()
    {
        // Wall jump has priority over regular jumps and does not cost a jump
        if(_isTouchingWall && _wantsToJump && !_isTouchingSurface)
        {
            WallJump(_wallOnRight);
        }
        else if(_numberOfJumps > 0 && _wantsToJump)
        {
            Jump();
        }
        _wantsToJump = false;
    }
   
    private void Jump()
    {
        // Add force, trigger animations, and decrement number of jumps
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpVelocity);
        _isTouchingSurface = false;
        if (_numberOfJumps != MaxJumps)
        {
            _wingsAnimators[0].SetBool("useWings", true);
            _wingsAnimators[1].SetBool("useWings", true);
        }
        SetNumberOfJumps(_numberOfJumps - 1);
    }

    private void WallJump(bool wallOnRight)
    {
        // Wall jump sends the player outwards from the wall in addition to jumping normally
        _isTouchingWall = false;
        Jump();
        // Refund a jump, since walljump does not cost a jump, and the jump method decrements jumps
        SetNumberOfJumps(_numberOfJumps + 1);

        // Horizontal movement
        float wallJumpSpeed = -10f;
        if(!wallOnRight)
        {
            wallJumpSpeed = -wallJumpSpeed;
        }
        SetMovementSpeed(wallJumpSpeed);
    }

    // Sets number of jumps and manages wings UI
    private void SetNumberOfJumps(int numJumps)
    {
        _numberOfJumps = numJumps;

        // Here we enable one wings UI sprite for each airjump available
        int wingsUICount = numJumps;
        if (wingsUICount == MaxJumps) // We don't want to display a sprite for the ground jump
        {
            wingsUICount--;
        }
        // Indicate each jump by enabling one sprite
        foreach (Image jumpTracker in _jumpIndicator.GetComponentsInChildren<Image>())
        {
            if (wingsUICount-- > 0)
            {
                jumpTracker.enabled = true;
            }
            else
            {
                jumpTracker.enabled = false;
            }
        }
    }

    private new void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);

        // Reset player's jumps when they touch the floor
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            SetNumberOfJumps(MaxJumps);
            _isTouchingSurface = true;
        }

        // On wall collision, get ready to walljump if needed
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

    private void OnCollisionExit2D(Collision2D collision)
    {
        // If the player walks off a ledge, they lose their first jump
        if (_numberOfJumps == MaxJumps && collision.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
        {
            _isTouchingSurface = false;
            SetNumberOfJumps(_numberOfJumps - 1);
        }

        // When the player leaves contact with the wall, disable wall jump after a slight buffer
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            Invoke("DisableWallJump", 0.08f);
        }
    }

    // Removes ability to wall jump
    private void DisableWallJump()
    {
        _isTouchingWall = false;
    }

    private new void OnTriggerEnter2D(Collider2D collider)
    {
        base.OnTriggerEnter2D(collider);
        GameObject colObject = collider.gameObject;

        // Jump upgrade collision
        if(colObject.layer == LayerMask.NameToLayer("JumpUpgrade"))
        {
            // Bring upgrade to default layer so it no longer interacts with player
            colObject.layer = LayerMask.NameToLayer("Default");
            // Disable visuals, but do not destroy the gameobject so that particles can finish their lifetime
            colObject.GetComponent<SpriteRenderer>().enabled = false;
            colObject.GetComponent<ParticleSystem>().Stop();
            // Apply jump upgrade
            UpgradeJump();
        }

        // Set checkpoint
        if(colObject.layer == LayerMask.NameToLayer("Checkpoint"))
        {
            RespawnPoint = colObject.transform.position;
        }

        // Victory
        if(colObject.layer == LayerMask.NameToLayer("Victory"))
        {
            MenuManager.Instance.LoadGameEndMenu();
        }
    }

    // Gives the player an additional air jump, and makes wings + wingsUI visible if needed.
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

            EnableWingSprites();
            
        }
        // Increase available jumps
        if(MaxJumps < _jumpCap)
        {
            MaxJumps++;
            SetNumberOfJumps(_numberOfJumps + 1);
        }
    }

    // Make wings visible
    private void EnableWingSprites()
    {
        foreach (GameObject wing in _wings)
        {
            wing.SetActive(true);
        }
    }

    // Destroys the player and opens death menu
    public override void Die()
    {
            MenuManager.Instance.OpenDeathMenu();
            PlayDeathAnimation();
            Destroy(gameObject);    
    }

    protected override void PlayDeathAnimation()
    {
        // I haven't had time to add a death animation for the player, but this is where I would put it :)
    }


}
