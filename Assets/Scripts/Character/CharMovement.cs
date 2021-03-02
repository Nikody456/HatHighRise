﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
[RequireComponent(typeof(CharacterView))]
public class CharMovement : MonoBehaviour
{
    private CharacterView _view;
    private Stats _playerStats;

    public float _currentSpeed =1; ///Steve set to be able to move

    public float sprintSpeed; ///TODO STEVE: Move this into Stats 
    [SerializeField] ParticleSystem sprintParticles =default;

    [SerializeField] float gravity = default; ///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior
    [SerializeField] float friction = default;///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior
    [SerializeField] float airFriction = default;///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior

    //COYOTE TIME VARS
    [SerializeField] float coyoteTime;
    private bool _coyotePreEnabled = false;
    private float _coyotePre = 10;
    private bool _coyotePostEnabled = false;
    private float _coyotePost = 10;

    [SerializeField] int jumpLimit = 1; //Additional jump powerup?
    private int _jumps;

    [SerializeField] float moveDirection = default; //the direction the player is moving
    [SerializeField] LayerMask groundedMask = default;
    private Rigidbody2D _controller;

    public float input;

    private void Start()
    {
        _playerStats = GetComponent<Stats>();
        _view = GetComponent<CharacterView>();
        _controller = GetComponent<Rigidbody2D>();
        moveDirection = 0;
    }

    public void TrySprint()
    {
        if (isGrounded())
        {
            _currentSpeed = _playerStats.CurrentSprintSpeed;
            if (Mathf.Abs(input) > .25f)
            {
                sprintParticles.Play();
            }
            else
            {
                sprintParticles.Stop();
            }
        }
        else
        {
            _currentSpeed = _playerStats.CurrentMoveSpeed;
            sprintParticles.Stop();
        }
    }

    public void StopSprint()
    {
        _currentSpeed = _playerStats.CurrentMoveSpeed;
        sprintParticles.Stop();
    }

    public void TryJump()
    {

        if(_jumps == 0 && (isGrounded() || isOnWall() != 0 || _coyotePost <= coyoteTime))
        {
            DoJump();
        }
        else if (_jumps > 0 && _jumps < jumpLimit)
        {
            DoJump();
        }
        else if (!_coyotePreEnabled)
        {
            _coyotePre = 0;
            _coyotePreEnabled = true;
        }

    }

    private void DoJump()
    {

        _coyotePre = coyoteTime+1;
        _coyotePreEnabled = false;

        int wallCheck = isOnWall();
        if (wallCheck != 0 && !isGrounded())
        {
            moveDirection = wallCheck * _currentSpeed;
            _controller.velocity = new Vector2(moveDirection, _playerStats.CurrentJumpSpeed);
        }
        else
        {
            _controller.velocity = new Vector2(_controller.velocity.x, _playerStats.CurrentJumpSpeed);
        }
        
        sprintParticles.Emit(20);
        _jumps++;
    }

    public void SetInput(float newInput)
    {
        input = newInput;
    }

    bool isGrounded()
    {
        Vector2 boxPosition = new Vector2(transform.position.x, transform.position.y - transform.GetComponent<BoxCollider2D>().size.y / 2 - .065f);
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxPosition, new Vector2(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f), 0,groundedMask);

        Transform parent = null;

        for(int i = 0; i < collisions.Length; i++)
        {
            if(collisions[i].tag == "Platform")
            {
                parent = collisions[i].transform;
            }
        }

        transform.parent = parent;

        if(collisions.Length > 0)
        {
            //Debug.Log("grounded");
            return true;
        }
        else
        {
            //Debug.Log("not grounded");
            return false;
        }
    }

    int isOnWall()
    {

        Vector2 boxPositionR = new Vector2(transform.position.x - transform.GetComponent<BoxCollider2D>().size.x / 2 - .0625f, transform.position.y);
        Vector2 boxPositionL = new Vector2(transform.position.x + transform.GetComponent<BoxCollider2D>().size.x / 2 + .0625f, transform.position.y);
        Collider2D[] collisionsL = Physics2D.OverlapBoxAll(boxPositionL, new Vector2(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f), 0, groundedMask);
        Collider2D[] collisionsR = Physics2D.OverlapBoxAll(boxPositionR, new Vector2(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f), 0, groundedMask);

        if (collisionsL.Length > 0)
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

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - transform.GetComponent<BoxCollider2D>().size.y / 2 - .065f, 0), new Vector3(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f, 1));
        Vector2 boxPositionR = new Vector2(transform.position.x - transform.GetComponent<BoxCollider2D>().size.x/2 - .0625f, transform.position.y);
        Vector2 boxPositionL = new Vector2(transform.position.x + transform.GetComponent<BoxCollider2D>().size.x / 2 + .0625f, transform.position.y);
        Gizmos.DrawCube(boxPositionR, new Vector3(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f, 1));
        Gizmos.DrawCube(boxPositionL, new Vector3(.125f, transform.GetComponent<BoxCollider2D>().size.y - .25f, 1));
    }

    void Update()
    {

        _currentSpeed = _playerStats.CurrentMoveSpeed;
        if(isGrounded())
        {
            moveDirection = Mathf.Lerp(moveDirection, input * _currentSpeed, friction * Time.deltaTime);
            if(_controller.velocity.y <= 0)
            {
                _jumps = 0;
            }
        }
        else
        {
            moveDirection = Mathf.Lerp(moveDirection, input * _currentSpeed, airFriction * Time.deltaTime);
            if (!_coyotePostEnabled)
            {
                _coyotePostEnabled = true;
                _coyotePost = 0;
            }
            else
            {
                if (_coyotePost <= coyoteTime)
                {
                    _coyotePost += 1 * Time.deltaTime;
                }
            }
        }

        if (isOnWall() != 0 && _controller.velocity.y <= 0)
        {
            if (Mathf.Sign(input) != Mathf.Sign(isOnWall()) && input != 0)
            {
                _controller.velocity = new Vector2(moveDirection, 0);
            }
            _jumps = 0;
        }

        if(_coyotePre <= coyoteTime)
        {
            _coyotePre += 1 * Time.deltaTime;
            TryJump();
        }

        _controller.velocity = new Vector2(moveDirection, _controller.velocity.y);
        _view.SetMoveSpeed(_controller.velocity.magnitude);
    }
}