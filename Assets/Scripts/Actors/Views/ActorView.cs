using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorView : MonoBehaviour
{

    ///Will Need to Manage Animations 
    protected Animator _animator;


    /*********INIT******************************************************************************************************/

    protected virtual void Awake()
    {
        _animator = this.GetComponent<Animator>();

    }
}
