using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class CharacterView : MonoBehaviour
{
    const string SPEED = "speed";
    const string JUMPING = "isJumping";


    ///Will Need to Manage Hats added
    [SerializeField] 
    HatManager _hatManager = default;
    ///Will Need to Manage Animations 
    Animator _animator;
    
    
    /*********INIT******************************************************************************************************/


    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        if (_hatManager == null)
            _hatManager = this.GetComponentInChildren<HatManager>();
    }

    /*********HATS******************************************************************************************************/

    public void PickUpHat(Hat hat)
    {

    }

    public void PutDownHat(Hat hat)
    {

    }

    /*********Animations******************************************************************************************************/


    public void SetMoveSpeed(float input)
    {
        _animator.SetFloat(SPEED, input);
    }

    public void SetIsJumping(bool isJumping)
    {
        _animator.SetBool(JUMPING, isJumping);
    }

}
