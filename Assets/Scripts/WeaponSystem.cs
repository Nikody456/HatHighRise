#pragma warning disable CS0649 // Field is never assigned to, and will always have its default value.
using UnityEngine;



[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class WeaponSystem : MonoBehaviour
{
    Animator _animator;
    SpriteRenderer _sr;

    private void Awake()
    {
        _animator = this.GetComponent<Animator>();
        _sr = this.GetComponent<SpriteRenderer>();
    }
    public void PlayAnim(bool isMelee, int index)
    {

    }
}
