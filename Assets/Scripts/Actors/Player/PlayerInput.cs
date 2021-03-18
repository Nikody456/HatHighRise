using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharMovement))]
public class PlayerInput : MonoBehaviour
{

    private PlayerMovement character;

    private void Start()
    {
        character = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            character.TryJump();
        }

        if (Input.GetKey("left shift"))
        {
            character.TrySprint();
        }

        if (Input.GetKeyUp("left shift"))
        {
            character.StopSprint();
        }

        if (Input.GetMouseButtonUp(0))//IDK
        {
            character.TryMeleeAttack();
        }

        character.SetInput(Input.GetAxis("Horizontal"));

    }
}
