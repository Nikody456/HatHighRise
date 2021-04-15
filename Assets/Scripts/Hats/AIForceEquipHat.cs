#pragma warning disable CS0649 // Ignore : "Field is never assigned to, and will always have its default value"
using System.Collections.Generic;
using UnityEngine;



public class AIForceEquipHat : MonoBehaviour
{
    [SerializeField] Hat _hat = default;
    [SerializeField] CharacterCollider _charCollider;
    public void Start()
    {
        _charCollider.PickUpHat(_hat);
    }
}

