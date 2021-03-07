using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(Rigidbody2D))]
public class ActorMovement : MonoBehaviour
{
    [SerializeField] protected float moveDirection = default; //the direction the actor is moving
    [SerializeField] protected LayerMask groundedMask = default;
    protected Rigidbody2D _controller;

    protected float _input;

    protected virtual void Start()
    {
        _controller = GetComponent<Rigidbody2D>();
        moveDirection = 0;
    }

  
    public virtual void SetInput(float newInput)
    {
        _input = newInput;
    }

    protected virtual void Update()
    {

    }

   
}
