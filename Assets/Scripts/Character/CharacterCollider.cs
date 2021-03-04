using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;
///we should start a namespace Character


[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterCollider : MonoBehaviour
{
    [SerializeField] CharacterView _characterView = default;
    [SerializeField] Stats _characterStats = default;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log($"{this.gameObject.name} Trigger2D with {collision.gameObject.name} ");
        Hat hat = collision.gameObject.GetComponent<Hat>();
        if(hat)
        {
            hat.OnPickup(_characterStats, _characterView);
        }
    }
}
