using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

//[RequireComponent(typeof(Rigidbody2D))]
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

    Stats _myStats;
    /*********INIT******************************************************************************************************/

    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = "Hats";
        _collider = this.GetComponent<Collider2D>();
        _rb = this.GetComponent<Rigidbody2D>();
        EnforcePhysics();
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

    public bool IsMeleeHat => _hatData.IsMeleeHat;
    public bool IsRangedHat => _hatData.IsRangedHat;
    public int AnimatorAtkIndex => _hatData.AnimatorAttackIndex;

    public void OnPickup(Stats stats, CharacterView view)
    {
        _myStats = stats;
        _myStats.AddModifier(Modifier);
        /// tell the characterView to wear this
        view.PickUpHat(this);
        IsPickedUp = true;
        Destroy(_rb);
    }


    public void OnPutDown()
    {
        _myStats.RemoveModifier(Modifier);
        /// tell the characterView to remove this
        IsPickedUp = false;
        _myStats = null;
        EnforcePhysics();
    }

    public void SetFlipX(bool cond)
    {
        if(_spriteRenderer)
            _spriteRenderer.flipX = cond;
    }

    public void SaveDataToScore(HatScore score)
    {
        score.SaveHat(_hatData);
    }

    public void SetOrderInSortingLayer(int order){ _spriteRenderer.sortingOrder = order; }
    /***************************************************************************************************************/

    private void EnforcePhysics()
    {
        ///CANT SET UNTIL WE FIX LEVEL COLLIDERS to be 2D:
        _collider.isTrigger = false;
        if(_rb)
            _rb.isKinematic = false;
        
    }


}
