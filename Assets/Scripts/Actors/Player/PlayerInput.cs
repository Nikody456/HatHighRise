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
        if (Input.GetButtonDown("Jump"))
        {
            _player.TryJump();
        }

        if (Input.GetButton("Sprint"))
        {
            _player.TrySprint();
        }

        if (Input.GetButtonUp("Sprint"))
        {
            _player.StopSprint();
        }

        if (Input.GetButtonUp("Attack")) //LMB
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
