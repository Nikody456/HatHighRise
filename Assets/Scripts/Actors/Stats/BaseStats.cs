using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Statistics
{

    [CreateAssetMenu(menuName = "Stats/Base Stats")]
    public class BaseStats : ScriptableObject
    {

        #region meters
        [Range(1, 100)]
        [SerializeField] int _hpMax = default;
        public int HpMax => _hpMax;

        #endregion

        #region actual 

        [Range(1, 25f)]
        [SerializeField] float _baseJump = default;
        public float Jump => _baseJump;

        [Range(1, 15f)]
        [SerializeField] float _baseMove = default;
        public float MovementSpeed => _baseMove;

        [Range(1, 15f)]
        [SerializeField] float _baseSprint = default;
        public float Sprint => _baseSprint;
        [Range(1, 120)]
        [SerializeField] int _attack = default;
        public int Attack => _attack;

        [Range(1, 120)]
        [SerializeField] int _defense = default;
        public int Defense => _defense;


        #endregion

    }
}