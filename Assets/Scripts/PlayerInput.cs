using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharMovement))]
public class PlayerInput : MonoBehaviour
{

    private CharMovement character;

    private void Start()
    {
        character = GetComponent<CharMovement>();
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

        character.SetInput(Input.GetAxis("Horizontal"));

    }
}
