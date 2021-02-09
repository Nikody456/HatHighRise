using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

public class Hat : MonoBehaviour
{
    [SerializeField] HatData _hatData;

    public Modifier Modifier { get; private set; }

    /*********INIT******************************************************************************************************/

    void Awake()
    {
        SetUpModifier();
    }

    void SetUpModifier()
    {
        Modifier = new Modifier(_hatData.Stat, _hatData.Type, _hatData.Value);
    }

    /***************************************************************************************************************/

    public void OnPickup(Stats stats)
    {
        stats.AddModifier(Modifier);
        ///TODO tell the characterView to wear this
    }


    public void OnPutDown(Stats stats)
    {
        stats.RemoveModifier(Modifier);
        ///TODO tell the characterView to remove this
    }

    /***************************************************************************************************************/

}
