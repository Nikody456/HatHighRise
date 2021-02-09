﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Statistics;

[CreateAssetMenu(menuName = "Stats/Hat Data")]
public class HatData : ScriptableObject
{

    public string Name => _name;
    [Header("Info")]
    [SerializeField] string _name = default;

    public Sprite Image => _img;
    [SerializeField] Sprite _img = default;
  
    public eStat Stat => _stat;
    [Header("Statistics")]
    [SerializeField] eStat _stat = default;
    
    public Modifier.eType Type => _type;
    [SerializeField] Modifier.eType _type = default;

    public float Value => _value;
    [SerializeField] float _value = default;

}
