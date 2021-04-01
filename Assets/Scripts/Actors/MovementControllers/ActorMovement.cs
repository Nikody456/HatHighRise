using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class ActorMovement : MonoBehaviour
{
    [SerializeField] protected float _moveDirection = default; //the direction the actor is moving
    [SerializeField] protected LayerMask _groundedMask = default;
    protected Rigidbody2D _controller;
    protected SpriteRenderer _spriteRenderer;
    protected float _input;


    protected virtual void Awake()
    {
        _controller = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _moveDirection = 0;

    }
    protected virtual void Start()
    {
    }

    protected void Update()
    {
        DoMovement();
    }

    protected abstract void DoMovement();
  
    public virtual void SetInput(float newInput)
    {
        _input = newInput;
    }

    public bool isFacingRight()
    {
        return _spriteRenderer.flipX==false;
    }

}
