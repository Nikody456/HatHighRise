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

    [SerializeField] float maxSlideSpeed;
    private float _slideSpeed = 0;

    protected override void DoMovement()
    {
        base.DoMovement();


        if (_isGrounded)
        {
            _view.SetIsOnWall(false);
        }
        else
        {
            CoyoteTimePost();
            WallJumping();

        }

        if (_coyotePre <= coyoteTime) //if player has tried to jump before they have landed, keep trying to jump
        {
            _coyotePre += 1 * Time.deltaTime;
            TryJump();
        }
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

    public bool getIsGrounded()
    {
        return _isGrounded;
    }

    private void WallJumping() //Wall jump logic
    {
        if (isOnWall() != 0 && _controller.velocity.y <= 0) //if player is on a wall, and is also not jumping off of a wall
        {
            _view.SetIsOnWall(true);//change animator settings accordingly

            if (Mathf.Sign(_input) != Mathf.Sign(isOnWall()) && _input != 0)
            {
                _controller.velocity = new Vector2(_moveDirection, _slideSpeed); //make the player not fall
                _controller.gravityScale = 0;
                _slideSpeed = Mathf.Lerp(_slideSpeed, maxSlideSpeed, .5f * Time.deltaTime);
            }
            else
            {
                _controller.gravityScale = 1.5f;
                _slideSpeed = 0;
            }

            _jumps = 0;//reset jumps
        }
        else
        {
            _view.SetIsOnWall(false);
            _controller.gravityScale = 1.5f;
        }
    }
    public override bool TryJump() //coyote time and jumps
    {
        jumpLimit = _playerStats.CurrentJumpLimit;

        if(_jumps == 0 && !_isGrounded && isOnWall() == 0 && _coyotePost >= coyoteTime)
        {
            _jumps = 1;
        }

        if (_jumps == 0 && (_isGrounded || isOnWall() != 0 || _coyotePost < coyoteTime)) //only let the player use their first jump when they are grounded, on a wall, or if coyote time applies
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

        return true;

    }


    protected override void DoJump() //coyote time and walls
    {
        //disable pre coyote time
        _coyotePre = coyoteTime + 1;
        _coyotePreEnabled = false;
        _slideSpeed = 0;
        base.DoJump();

        AudioManager.Instance.PlaySFX("jumpSound");
    }


    protected override void UpdateJumpVelocity()
    {
        int wallCheck = isOnWall();

        if (wallCheck != 0 && !_isGrounded) //make sure the player jumps away from the wall if wall jumping
        {
            _moveDirection = wallCheck * _currentSpeed;
            _controller.velocity = new Vector2(_moveDirection, _playerStats.CurrentJumpSpeed);
        }
        else
        {
            base.UpdateJumpVelocity();
        }
    }

    int isOnWall() //determine whether or not the player can wall jump
    {
        var boxCol = transform.GetComponent<BoxCollider2D>().size;
        //similar to isGrounded, have two intersect tests on either side of the player
        Vector2 boxPositionR = new Vector2(transform.position.x -boxCol.x / 2 - .0625f, transform.position.y);
        Vector2 boxPositionL = new Vector2(transform.position.x + boxCol.x / 2 + .0625f, transform.position.y);
        Collider2D[] collisionsL = Physics2D.OverlapBoxAll(boxPositionL, new Vector2(.125f, boxCol.y - .25f), 0, _groundedMask);
        Collider2D[] collisionsR = Physics2D.OverlapBoxAll(boxPositionR, new Vector2(.125f, boxCol.y - .25f), 0, _groundedMask);

        if (collisionsL.Length > 0) //return an int for which side of the player the wall is on
        {
            return -1;
        }
        else if (collisionsR.Length > 0)
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
