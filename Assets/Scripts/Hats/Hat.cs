using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

public class Hat : MonoBehaviour
{
    [SerializeField] HatData _hatData;

    public Modifier Modifier { get; private set; }

    void Awake()
    {
        SetUpModifier();
    }

    void SetUpModifier()
    {
        Modifier = new Modifier(_hatData.Stat, _hatData.Type, _hatData.Value);
    }
}
