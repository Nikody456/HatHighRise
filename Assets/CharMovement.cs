using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(CharacterController))]
public class CharMovement : MonoBehaviour
{
    [SerializeField] Stats playerStats;

    [SerializeField] float currentSpeed;

    public float baseSpeed;
    public float sprintSpeed;
    [SerializeField] ParticleSystem sprintParticles;

    [SerializeField] float jumpSpeed;
    [SerializeField] float gravity;
    [SerializeField] float friction;
    [SerializeField] int jumpLimit = 1; //Additional jump powerup?
    private int jumps;

    [SerializeField] Vector2 moveVector; //the direction the player is moving
    [SerializeField] Vector2 speedClamp; //limit the speed of the player
    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        moveVector = new Vector2(0, -speedClamp.y);
    }

    void Update()
    {
        if (Input.GetKey("left shift") && controller.isGrounded)
        {
            currentSpeed = sprintSpeed;
            sprintParticles.Play();

        }
        else
        {
            currentSpeed = baseSpeed;
            sprintParticles.Stop();
        }

        moveVector = new Vector2(Mathf.Lerp(moveVector.x, Input.GetAxis("Horizontal") * currentSpeed, friction * Time.deltaTime), moveVector.y);

        if (controller.isGrounded)
        {
            moveVector = new Vector2(moveVector.x, -1);
            jumps = 0;
            if (Input.GetKeyDown("space"))
            {
                moveVector.y = jumpSpeed;
                jumps++;
                sprintParticles.Emit(20);
            }
        }
        else
        {
            moveVector.y -= gravity;
        }

        if (Input.GetKeyDown("space") && jumps < jumpLimit && jumps > 1)
        {
            moveVector.y = jumpSpeed;
            jumps++;
            sprintParticles.Emit(20);
        }

        moveVector = new Vector2(Mathf.Clamp(moveVector.x, -speedClamp.x, speedClamp.x), Mathf.Clamp(moveVector.y, -speedClamp.y, speedClamp.y));
        controller.Move(moveVector * Time.deltaTime);

    }
}
