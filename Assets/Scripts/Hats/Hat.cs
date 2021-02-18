using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Hat : MonoBehaviour
{
    [SerializeField] HatData _hatData = default;
    public Modifier Modifier { get; private set; }
    public bool IsPickedUp { get; private set; }



    SpriteRenderer _spriteRenderer;
    Collider2D _collider;
    Rigidbody2D _rb;


    /*********INIT******************************************************************************************************/

    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = "Hats";
        _collider = this.GetComponent<Collider2D>();
        _rb = this.GetComponent<Rigidbody2D>();
        TogglePhysics();
        SetUpModifier();
    }

    void SetUpModifier()
    {
        Modifier = new Modifier(_hatData.Stat, _hatData.Type, _hatData.Value);
        _spriteRenderer.sprite = _hatData.Image;
        BoxCollider2D boxCol = _collider as BoxCollider2D;
        if (boxCol)
            boxCol.size = _hatData.ColliderSize;
        else
            Debug.LogWarning("Not using a BoxCollider for hat?");
    }

    /***************************************************************************************************************/

    public void OnPickup(Stats stats, CharacterView view)
    {
        stats.AddModifier(Modifier);
        /// tell the characterView to wear this
        view.PickUpHat(this);
        IsPickedUp = true;
        TogglePhysics();
    }


    public void OnPutDown(Stats stats, CharacterView view)
    {
        stats.RemoveModifier(Modifier);
        /// tell the characterView to remove this
        view.PutDownHat(this);
        IsPickedUp = false;
        TogglePhysics();
    }


    public void SetOrderInSortingLayer(int order){ _spriteRenderer.sortingOrder = order; }
    /***************************************************************************************************************/

    private void TogglePhysics()
    {
        ///CANT SET UNTIL WE FIX LEVEL COLLIDERS to be 2D:
        _collider.isTrigger = !IsPickedUp;
       // _rb.isKinematic = IsPickedUp;
        
    }
}
