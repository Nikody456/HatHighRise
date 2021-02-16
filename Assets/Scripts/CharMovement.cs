using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(CharacterController))]
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

    [SerializeField] Vector2 moveVector = default; //the direction the player is moving
    [SerializeField] Vector2 speedClamp = default; //limit the speed of the player
    private CharacterController _controller;

    public float input;

    private void Start()
    {

        _playerStats = GetComponent<Stats>();

        _controller = GetComponent<CharacterController>();
        moveVector = new Vector2(0, -speedClamp.y);
    }

    public void TrySprint()
    {
        if (_controller.isGrounded)
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

    public void TryJump()
    {
        if (_jumps < jumpLimit)
        {
            moveVector.y = _playerStats.CurrentJumpSpeed;
            _jumps++;
            sprintParticles.Emit(20);
        }
    }

    public void SetInput(float newInput)
    {
        input = newInput;
    }

    void Update()
    {
        _currentSpeed = _playerStats.CurrentMoveSpeed;
        moveVector = new Vector2(Mathf.Lerp(moveVector.x, input * _currentSpeed, friction * Time.deltaTime), moveVector.y);


        if(_controller.isGrounded && _jumps == 0)
        {
            moveVector = new Vector2(moveVector.x, -1);
        }

        if (_controller.isGrounded)
        {
            _jumps = 0;
        }
        else
        {
            moveVector.y -= gravity;
        }

        moveVector = new Vector2(Mathf.Clamp(moveVector.x, -speedClamp.x, speedClamp.x), Mathf.Clamp(moveVector.y, -speedClamp.y, speedClamp.y));
        _controller.Move(moveVector * Time.deltaTime);

    }
}
