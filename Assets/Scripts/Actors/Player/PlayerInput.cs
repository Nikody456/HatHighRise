using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharMovement))]
public class PlayerInput : MonoBehaviour
{

    private PlayerMovement _player;

    private void Start()
    {
        _player = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            _player.TryJump();
        }

        if (Input.GetKey("left shift"))
        {
            _player.TrySprint();
        }

        if (Input.GetKeyUp("left shift"))
        {
            _player.StopSprint();
        }

        if (Input.GetMouseButtonUp(0)) //LMB
        {
            _player.TryMeleeAttack();
        }

        if(Input.GetMouseButtonDown(1))  //RMB
        {
            _player.TryRangedAttack();
        }

        _player.SetInput(Input.GetAxis("Horizontal"));

    }
}
