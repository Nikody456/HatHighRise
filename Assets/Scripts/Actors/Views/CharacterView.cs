using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class CharacterView : ActorView
{
    const string XSPEED = "x_speed";
    const string YSPEED = "y_speed";
    const string GROUNDED = "isGrounded";
    const string ONWALL = "onWall";
    const string ATTACK = "Attack";
    const string MELEE = "melee_atk";
    const string RANGED = "ranged_atk";
    const string HIT = "hit_react";

    ///Will Need to Manage Hats added
    [SerializeField] 
    HatManager _hatManager = default;
    SpriteRenderer _sr;
    WeaponSystem _wp;

    bool _isInteracting = false;
    /*********INIT******************************************************************************************************/


    protected override void Awake()
    {
        base.Awake();
        if (_hatManager == null)
            _hatManager = this.GetComponentInChildren<HatManager>();
        _sr = this.GetComponent<SpriteRenderer>();
        _wp = this.GetComponentInChildren<WeaponSystem>();
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
        _sr.flipX = mirror;
    }

    public void SetIsOnWall(bool isOnWall)
    {
        _animator.SetBool(ONWALL, isOnWall);
    }

    public void SetIsInteracting(bool cond)
    {
        _isInteracting = cond;
    }

    public void TrySetMeleeAttack()
    {
        ///Need a way to validate they are not already attacking
        /// could have the atk state call this script and set to "isAttacking"

        if (!_isInteracting && _hatManager.HasMeleeAttackHat(out int attackIndex))
        {
            _animator.SetInteger(MELEE, attackIndex);
            _animator.SetTrigger(ATTACK);
            _wp.PlayAnim(true, attackIndex);
            var playerHack = this.GetComponent<PlayerInput>(); 
            if (playerHack)
            {
                playerHack.SetIsInteracting(true);
            }
        }
    }

    public void TrySetRangedAttack()
    {
        if (!_isInteracting && _hatManager.HasRangedAttackHat(out int attackIndex))
        {
            _animator.SetInteger(RANGED, attackIndex);
            _animator.SetTrigger(ATTACK);
            _wp.PlayAnim(false, attackIndex);
        }
    }

    public void OnAttackFinish()
    {
        //Clear the trigger and hope this doesnt mess up an atk queue 
        //(this prevents looping as triggers sometimes dont clear reliably in mecanim)
        _animator.ResetTrigger(ATTACK);
        SetIsInteracting(false);
        //Debug.Log("WE CALLED ONATKFINISH");
    }

    public void ImHit()
    {
        _animator.SetTrigger(HIT);
    }


}
