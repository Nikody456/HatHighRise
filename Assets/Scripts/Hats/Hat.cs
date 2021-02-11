using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Hat : MonoBehaviour
{
    [SerializeField] HatData _hatData = default;
    SpriteRenderer _spriteRenderer;

    public Modifier Modifier { get; private set; }


    /*********INIT******************************************************************************************************/

    void Awake()
    {
        _spriteRenderer = this.GetComponent<SpriteRenderer>();
        SetUpModifier();
    }

    void SetUpModifier()
    {
        Modifier = new Modifier(_hatData.Stat, _hatData.Type, _hatData.Value);
        _spriteRenderer.sprite = _hatData.Image;
    }

    /***************************************************************************************************************/

    public void OnPickup(Stats stats, CharacterView view)
    {
        stats.AddModifier(Modifier);
        /// tell the characterView to wear this
        view.PickUpHat(this);
    }


    public void OnPutDown(Stats stats, CharacterView view)
    {
        stats.RemoveModifier(Modifier);
        /// tell the characterView to remove this
        view.PutDownHat(this);
    }

    /***************************************************************************************************************/

}
