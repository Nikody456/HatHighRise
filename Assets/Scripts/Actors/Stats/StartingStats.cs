﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Statistics
{

    [CreateAssetMenu(menuName = "Stats/Starting Stats")]
    public class StartingStats : ScriptableObject
    {

        #region meters
        [Range(1, 100)]
        [SerializeField] int _hpMax = default;
        public int hpMAX => _hpMax;

        #endregion

        #region actual 

        [Range(1, 25f)]
        [SerializeField] float _baseJump = default;
        public float BaseJump => _baseJump;

        [Range(1, 15f)]
        [SerializeField] float _baseMove = default;
        public float BaseMove => _baseMove;

        [Range(1, 15f)]
        [SerializeField] float _baseSprint = default;
        public float BaseSprint => _baseSprint;
        [Range(1, 120)]
        [SerializeField] int _attack = default;
        public int attack => _attack;

        [Range(1, 120)]
        [SerializeField] int _defense = default;
        public int defense => _defense;


        #endregion

    }
}