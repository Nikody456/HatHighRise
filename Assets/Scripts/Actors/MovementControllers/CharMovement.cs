using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;


[RequireComponent(typeof(Stats))]
public class CharMovement : ActorMovement
{
    protected CharacterView _view;
    protected Stats _playerStats;

    protected float _currentSpeed;

    [SerializeField] protected ParticleSystem sprintParticles =default;

    [SerializeField] protected float gravity = default; ///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior
    [SerializeField] protected float friction = default;///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior
    [SerializeField] protected float airFriction = default;///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior

    [SerializeField] protected int jumpLimit = 1; //Additional jump powerup?
    protected int _jumps;
    protected bool _isGrounded;

    public Vector2 startPosition;

    protected override void Awake()
    {
        base.Awake();
        _playerStats = GetComponent<Stats>();
        _view = this.GetComponent<CharacterView>();

        if(startPosition == Vector2.zero)
        {
            startPosition = transform.position;
        }
        else
        {
            transform.position = startPosition;
        }


    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void DoMovement()
    {
        _currentSpeed = sprintParticles.isPlaying? _playerStats.CurrentSprintSpeed : _playerStats.CurrentMoveSpeed; ///Need to get every frame, not just at start
        //Determine if the player is grounded each frame
        _isGrounded = isGrounded();
        _view.SetIsGrounded(_isGrounded);

        float useSpeed = _currentSpeed;

        if (GetComponent<HatManager>())
        {
            useSpeed = Mathf.Clamp(_currentSpeed - .5f * (GetComponent<HatManager>().getNumHats()), _currentSpeed / 2, _currentSpeed);
        }

        if (_isGrounded)
        {
            _moveDirection = Mathf.Lerp(_moveDirection, _input * useSpeed, friction * Time.deltaTime);

            //Reset Jumps
            if (_controller.velocity.y <= 0)
            {
                _jumps = 0;
            }
        }
        else
        {
            _moveDirection = Mathf.Lerp(_moveDirection, _input * useSpeed, airFriction * Time.deltaTime);
        }

        //set velocity of the character
        _controller.velocity = new Vector2(_moveDirection, _controller.velocity.y);

        SetAnimatorSpeeds();
    }

    protected void SetAnimatorSpeeds() //Set Animator Xspeed and Yspeed... Flip the player sprite
    {
        //Send character speed info to the animator
        _view.SetXSpeed(Mathf.Abs(_controller.velocity.x));
        _view.SetYSpeed(_controller.velocity.y);

        if (Mathf.Abs(_controller.velocity.x) > .5) //determine which direction the character is facing
        {
            if (_controller.velocity.x > 0)
            {
                _view.SetMirror(false);
            }
            else
            {
                _view.SetMirror(true);
            }
        }
    }

    public void TrySprint()
    {
        if (isGrounded() && Mathf.Abs(_input) > .25f)
        {
            _currentSpeed = _playerStats.CurrentSprintSpeed; //set player speed to sprint speed
            sprintParticles.Play();
        }
        else
        {
            _currentSpeed = _playerStats.CurrentMoveSpeed;
            sprintParticles.Stop();
        }
    }

    public void StopSprint() //For when the player releases the shift button
    {
        _currentSpeed = _playerStats.CurrentMoveSpeed;
        sprintParticles.Stop();
    }

    public virtual bool TryJump()
    {
        jumpLimit = _playerStats.CurrentJumpLimit;
        if ((_jumps == 0 && isGrounded()) || (_jumps > 0 && _jumps < jumpLimit)) //only let the player use their first jump when they are grounded, on a wall, or if coyote time applies
        {
            DoJump();
            return true;
        }
        return false;

    }

    protected virtual void DoJump()
    {
        UpdateJumpVelocity();
        sprintParticles.Emit(20); //emit some particles
        _jumps++; //keep track of how many times the player has jumped
    }

    protected virtual void UpdateJumpVelocity()
    {
        _controller.velocity = new Vector2(_controller.velocity.x, _playerStats.CurrentJumpSpeed); //jump

    }

    protected bool isGrounded() //determine whether or not the player is grounded
    {

        ///TODO Figure out player on moving platform physics
        Vector2 boxPosition = new Vector2(transform.position.x, transform.position.y - transform.GetComponent<BoxCollider2D>().size.y / 2 - .065f);
        //an array of every 2d collider that intersects the players grounded box
        Collider2D[] collisions = Physics2D.OverlapBoxAll(boxPosition, new Vector2(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f), 0,_groundedMask);


        //moving platform logic
        //redo this with physics

        for(int i = 0; i < collisions.Length; i++)
        {
            if (collisions[i].transform.parent != null && collisions[i].transform.parent.GetComponent<MovingPlatform>() != null)
            {
                MovingPlatform platform = collisions[i].transform.parent.GetComponent<MovingPlatform>();
                if (platform != null)
                {
                    _controller.transform.Translate(platform.platformVelocity, transform);
                }
            }
        }

        if(collisions.Length > 0) //if there is a collider within the player's grounded box, the player is grounded
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void TryMeleeAttack()
    {
        _view.TrySetMeleeAttack();
    }
    public void TryRangedAttack()
    {
        _view.TrySetRangedAttack();
    }


    private void OnDrawGizmosSelected() //Debug boxes to test the grounded and on wall box sizes
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - transform.GetComponent<BoxCollider2D>().size.y / 2 - .065f, 0), new Vector3(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f, 1));
    }
    private void FootStep()
    {
        AudioManager.Instance.PlaySFX("stepSound");
    }

    private void GuardFootStep()
    {
        AudioManager.Instance.PlaySFX("guardStepSound");
    }
}
