using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;


[RequireComponent(typeof(Stats))]
public class PlayerMovement : CharMovement
{

    //COYOTE TIME VARS
    [SerializeField] float coyoteTime;
    private bool _coyotePreEnabled = false;
    private float _coyotePre = 10;
    private bool _coyotePostEnabled = false;
    private float _coyotePost = 10;

    protected override void Start()
    {
        base.Start();

        if(startPosition == Vector2.zero)
        {
            startPosition = transform.position;
        }
        else
        {
            transform.position = startPosition;
        }

    }

    protected override void Update()
    {
        //Determine if the player is grounded each frame
        bool grounded = isGrounded();
        _view.SetIsGrounded(grounded);

        if (grounded)
        {
            moveDirection = Mathf.Lerp(moveDirection, _input * _currentSpeed, friction * Time.deltaTime);

            _view.SetIsOnWall(false);

            //Reset Jumps
            if (_controller.velocity.y <= 0)
            {
                _jumps = 0;
            }
        }
        else
        {
            moveDirection = Mathf.Lerp(moveDirection, _input * _currentSpeed, airFriction * Time.deltaTime);
            CoyoteTimePost();
            WallJumping();
        }

        if (_coyotePre <= coyoteTime) //if player has tried to jump before they have landed, keep trying to jump
        {
            _coyotePre += 1 * Time.deltaTime;
            TryJump();
        }

        //set velocity of the character
        _controller.velocity = new Vector2(moveDirection, _controller.velocity.y);

        SetAnimatorSpeeds();
    }

    private void CoyoteTimePost() //Allows the player to jump slightly after walking off a platform
    {
        if (!_coyotePostEnabled) //if timer is not already incrementing
        {
            _coyotePostEnabled = true; //start timer
            _coyotePost = 0;
        }
        else
        {
            if (_coyotePost <= coyoteTime) //increment timer until player can no longer jump
            {
                _coyotePost += 1 * Time.deltaTime;
            }
        }
    }

    private void WallJumping() //Wall jump logic
    {
        if (isOnWall() != 0 && _controller.velocity.y <= 0) //if player is on a wall, and is also not jumping off of a wall
        {
            _view.SetIsOnWall(true);//change animator settings accordingly

            if (Mathf.Sign(_input) != Mathf.Sign(isOnWall()) && _input != 0)
            {
                _controller.velocity = new Vector2(moveDirection, 0); //make the player not fall
                _controller.gravityScale = 0;
            }
            else
            {
                _controller.gravityScale = 1.5f;
            }

            _jumps = 0;//reset jumps
        }
        else
        {
            _view.SetIsOnWall(false);
            _controller.gravityScale = 1.5f;
        }
    }

    public override void TryJump() //coyote time and jumps
    {

        if(_jumps == 0 && (isGrounded() || isOnWall() != 0 || _coyotePost <= coyoteTime)) //only let the player use their first jump when they are grounded, on a wall, or if coyote time applies
        {
            DoJump();
        }
        else if (_jumps > 0 && _jumps < jumpLimit) //subsequent jumps can be performed mid-air
        {
            DoJump();
        }
        else if (!_coyotePreEnabled) //enables coyoteTime if jump fails
        {
            _coyotePre = 0;
            _coyotePreEnabled = true;
        }

    }

    protected override void DoJump() //coyote time and walls
    {
        //disable pre coyote time
        _coyotePre = coyoteTime+1;
        _coyotePreEnabled = false;

        int wallCheck = isOnWall();

        if (wallCheck != 0 && !isGrounded()) //make sure the player jumps away from the wall if wall jumping
        {
            moveDirection = wallCheck * _currentSpeed;
            _controller.velocity = new Vector2(moveDirection, _playerStats.CurrentJumpSpeed);
        }
        else
        {
            _controller.velocity = new Vector2(_controller.velocity.x, _playerStats.CurrentJumpSpeed); //jump normally when not on a wall
        }
        
        sprintParticles.Emit(20); //emit some particles
        _jumps++; //keep track of how many times the player has jumped
    }

    int isOnWall() //determine whether or not the player can wall jump
    {
        //similar to isGrounded, have two intersect tests on either side of the player
        Vector2 boxPositionR = new Vector2(transform.position.x - transform.GetComponent<BoxCollider2D>().size.x / 2 - .0625f, transform.position.y);
        Vector2 boxPositionL = new Vector2(transform.position.x + transform.GetComponent<BoxCollider2D>().size.x / 2 + .0625f, transform.position.y);
        Collider2D[] collisionsL = Physics2D.OverlapBoxAll(boxPositionL, new Vector2(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f), 0, groundedMask);
        Collider2D[] collisionsR = Physics2D.OverlapBoxAll(boxPositionR, new Vector2(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f), 0, groundedMask);

        if (collisionsL.Length > 0) //return an int for which side of the player the wall is on
        {
            return -1;
        }
        else if(collisionsR.Length > 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    void OnDrawGizmosSelected() //Debug boxes to test the grounded and on wall box sizes
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - transform.GetComponent<BoxCollider2D>().size.y / 2 - .065f, 0), new Vector3(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f, 1));
        Vector2 boxPositionR = new Vector2(transform.position.x - transform.GetComponent<BoxCollider2D>().size.x / 2 - .0625f, transform.position.y);
        Vector2 boxPositionL = new Vector2(transform.position.x + transform.GetComponent<BoxCollider2D>().size.x / 2 + .0625f, transform.position.y);
        Gizmos.DrawCube(boxPositionR, new Vector3(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f, 1));
        Gizmos.DrawCube(boxPositionL, new Vector3(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f, 1));
    }

}
