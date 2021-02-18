using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Stats))]
public class CharMovement : MonoBehaviour
{
    private Stats _playerStats;

    public float _currentSpeed =1; ///Steve set to be able to move

    public float sprintSpeed; ///TODO STEVE: Move this into Stats 
    [SerializeField] ParticleSystem sprintParticles =default;

    [SerializeField] float gravity = default; ///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior
    [SerializeField] float friction = default;///TODO ??: Move into its own GAMECONSTANTS static class thats non-monobehavior

    [SerializeField] int jumpLimit = 1; //Additional jump powerup?
    private int _jumps;

    [SerializeField] float moveDirection = default; //the direction the player is moving
    [SerializeField] LayerMask groundedMask = default;
    private Rigidbody2D _controller;

    public float input;

    private void Start()
    {

        _playerStats = GetComponent<Stats>();

        _controller = GetComponent<Rigidbody2D>();
        moveDirection = 0;
    }

    public void TrySprint()
    {
        if (isGrounded())
        {
            _currentSpeed = _playerStats.CurrentMoveSpeed * sprintSpeed;
            sprintParticles.Play();
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

        if(_jumps == 0 && isGrounded())
        {
            DoJump();
        }
        else if (_jumps > 0 && _jumps < jumpLimit)
        {
            DoJump();
        }

    }

    private void DoJump()
    {
        _controller.velocity = new Vector2(_controller.velocity.x, _playerStats.CurrentJumpSpeed);
        sprintParticles.Emit(20);
        _jumps++;
    }

    public void SetInput(float newInput)
    {
        input = newInput;
    }

    bool isGrounded()
    {
        Vector2 boxPosition = new Vector2(transform.position.x, transform.position.y - transform.localScale.y / 2 - .065f);
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

    void OnDrawGizmosSelected()
    {
        // Draw a semitransparent blue cube at the transforms position
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y - transform.localScale.y/2 - .0625f, 0), new Vector3(transform.GetComponent<BoxCollider2D>().size.x - .125f, .125f, 1));
    }

    void Update()
    {

        _currentSpeed = _playerStats.CurrentMoveSpeed;
        moveDirection = Mathf.Lerp(moveDirection, input * _currentSpeed, friction * Time.deltaTime);

        if (isGrounded() && _controller.velocity.y <= 0)
        {
            _jumps = 0;
        }

        _controller.velocity = new Vector2(moveDirection, _controller.velocity.y);

    }
}
