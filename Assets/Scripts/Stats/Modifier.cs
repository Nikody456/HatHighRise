using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Statistics 
{
    public enum eStat { HPMAX, MOVESPEED, JUMP, ATTACK, DEFENSE, RAWDAMAGE }

    public class Modifier 
    {
        public enum eType { FLAT, PERCENT }

        public eStat Stat { get; private set; }
        public eType Type { get; private set; }
        public float Value { get; private set; }

        /*********INIT******************************************************************************************************/

        /** Can be used by a static object like a pick up to carry an unstarted modifier*/
        public Modifier(eStat statAffected, eType type, float value)
        {
            InitHelper(statAffected, type, value);
        }

        private void InitHelper(eStat statAffected, eType type, float value)
        {
            Stat = statAffected;
            Type = type;
            if (Type == eType.FLAT)
                Value = value;
            else
                Value = value / 100;
        }

        /*********PUBLIC******************************************************************************************************/


    }
}
