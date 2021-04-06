using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharMovement))]
public class PlayerInput : MonoBehaviour
{

    private PlayerMovement _player;

    private bool _isInteracting = false;

    private void Start()
    {
        _player = GetComponent<PlayerMovement>();
    }

    public void SetIsInteracting(bool cond)
    {
        _isInteracting = cond;
        Debug.Log($"Set Isinterfactiong to : {_isInteracting}");
    }

    void Update()
    {

        if (!_isInteracting)
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

            if (Input.GetMouseButtonDown(1))  //RMB
            {
                _player.TryRangedAttack();
            }

            _player.SetInput(Input.GetAxis("Horizontal"));
        }
        else
        {
            _player.SetInput(0); ///Helps with melee atk sliding when hit, it kinda sucks on spikes
            StartCoroutine(InteractionDelay());
        }

    }

    IEnumerator InteractionDelay()
    {
        yield return new WaitForSeconds(0.5f);
        _isInteracting = false;
    }
}
