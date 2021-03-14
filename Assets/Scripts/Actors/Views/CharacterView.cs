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
    const string ONWALL = "onWall";
    const string ATTACK = "Attack";
    const string MELEE = "melee_atk";

    ///Will Need to Manage Hats added
    [SerializeField] 
    HatManager _hatManager = default;
    ///Will Need to Manage Animations 
    private int _attackIndex=1;
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
        _animator.SetBool(MIRROR, mirror);
    }

    public void SetIsOnWall(bool isOnWall)
    {
        _animator.SetBool(ONWALL, isOnWall);
    }

    public void SetMeleeAttack()
    {
        ///Need a way to validate they are not already attacking
        _animator.SetInteger(MELEE, _attackIndex++);
        _animator.SetTrigger(ATTACK);
    }

    public void OnAttackFinish()
    {
        //Validate attack index todo:
        _attackIndex = 1;
        //Clear the trigger and hope this doesnt mess up an atk queue 
        //(this prevents looping as triggers sometimes dont clear reliably in mecanim)
        _animator.ResetTrigger(ATTACK);
        //Debug.Log("WE CALLED ONATKFINISH");
        
    }

    public void Test()
    {
        //Debug.Log($"WE CALLED TEST for {this.gameObject.name}");
    }


}
