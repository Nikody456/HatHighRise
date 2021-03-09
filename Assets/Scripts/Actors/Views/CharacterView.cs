﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class CharacterView : ActorView
{
    const string XSPEED = "x_speed";
    const string YSPEED = "y_speed";
    const string GROUNDED = "isGrounded";
    const string MIRROR = "mirror";


    ///Will Need to Manage Hats added
    [SerializeField] 
    HatManager _hatManager = default;
    ///Will Need to Manage Animations 
    
    /*********INIT******************************************************************************************************/


    protected override void Awake()
    {
        base.Awake();
        if (_hatManager == null)
            _hatManager = this.GetComponentInChildren<HatManager>();
    }

    /*********HATS******************************************************************************************************/

    public void PickUpHat(Hat hat)
    {
        _hatManager.OnPickUpHat(hat);
    }

    public void PutDownHat(Hat hat)
    {
        _hatManager.OnPutDownHat(hat);
    }

    /*********Animations******************************************************************************************************/


    public void SetXSpeed(float input)
    {
        _animator.SetFloat(XSPEED, input);
    }

    public void SetYSpeed(float input)
    {
        _animator.SetFloat(YSPEED, input);
    }

    public void SetIsGrounded(bool isGrounded)
    {
        _animator.SetBool(GROUNDED, isGrounded);
    }

    public void SetMirror(bool mirror)
    {
        _animator.SetBool(MIRROR, mirror); ;
    }


}