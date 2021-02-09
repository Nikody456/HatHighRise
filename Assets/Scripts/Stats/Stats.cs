using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Statistics
{
    public class Stats : MonoBehaviour
    {
        #region Modifiers
        private Transform _modiferChild;

        private List<Modifier> _healthModifiers = new List<Modifier>();
        private List<Modifier> _moveSpeedModifiers = new List<Modifier>();
        private List<Modifier> _jumpModifiers = new List<Modifier>();
        private List<Modifier> _attackModifiers = new List<Modifier>();
        private List<Modifier> _defenseModifiers = new List<Modifier>();
        private List<Modifier> _damageModifiers = new List<Modifier>();
        #endregion

        #region MaxStats
        public static int MAXHP { get; private set; } = 1000;
        public static int MAXMOVESPEED { get; private set; } = 150;
        public static int MAXJUMP { get; private set; } = 150;
        public static int MAXATTACK { get; private set; } = 120;
        public static int MAXDEFENSE { get; private set; } = 120;
        #endregion

        #region Properties
        private int _currentHealth;
        private int _currentMoveSpeed;
        private float _currentJump;  ///James wants a push to hold jump mechanic 
        private int _currentAttack;
        private int _currentDefense;
        #endregion


        #region Dynamic Meters
        private int _healthMAX = 100;

        #endregion

        #region Getters
        public int CurrentHealth => _currentHealth;
        public int CurrentMoveSpeed => _currentMoveSpeed;


        public int GetCurrentAttack()
        {
            var retVal = CalculateModifier(eStat.ATTACK);
            return retVal < MAXATTACK ? retVal : MAXATTACK;
        }
        public int GetCurrentDefense() ///Will eventually be used by UI im sure
        {
            var retVal = CalculateModifier(eStat.DEFENSE);
            return retVal < MAXDEFENSE ? retVal : MAXDEFENSE;
        }

        #endregion


        /*********INIT******************************************************************************************************/

        public void Initalize(GameObject soldier, StartingStats initalStats, bool isPlayer)
        {
            //Soldier = soldier;
            ///Grab our Initial dynamic meter stats
            _healthMAX = initalStats.hpMAX < MAXHP ? initalStats.hpMAX : MAXHP;



            ///Grab our initial static stats
            _currentJump = initalStats.BaseJump < MAXJUMP ? initalStats.BaseJump : MAXJUMP;
            _currentAttack = initalStats.attack < MAXATTACK ? initalStats.attack : MAXATTACK;
            _currentDefense = initalStats.defense < MAXDEFENSE ? initalStats.defense : MAXDEFENSE;

            _currentHealth = _healthMAX;


            InitModifierComponent();

            ///register ourselves to this classes static events to set up a health bar in UI
           

        }

        private void InitModifierComponent()
        {
            _modiferChild = new GameObject().transform;
            _modiferChild.parent = this.transform;
            _modiferChild.gameObject.name = "Modifiers";
        }

        /*********PUBLIC METHODS******************************************************************************************************/

        public int TakeDamage(int incommingDamage)
        {
            int rawDamage = CalculateDefense(incommingDamage);

            return ModifyHealth(rawDamage);
        }


        public void AddModifier(Modifier modifier)
        {
            GetListForModifier(modifier.Stat).Add(modifier);
            ///xfer the modifier to us and reset
            modifier.AssignOwner(this);

            HandleModifierExpections(modifier, true);
        }

        public void RemoveModifier(Modifier modifier)
        {
            if (modifier == null)
                return;

            List<Modifier> myList = GetListForModifier(modifier.Stat);

            if (!myList.Contains(modifier))
                return;

            myList.Remove(modifier);
            HandleModifierExpections(modifier, false);
        }
        
        /***********PRIVATE HELPERS**************************************************************************************************/

        private int ModifyHealth(int rawDamage)
        {
            _currentHealth += rawDamage;

            if (_currentHealth < 0)
                _currentHealth = 0;
            else if (_currentHealth > _healthMAX)
                _currentHealth = _healthMAX;

            //float currentHealthPct = (float)_currentHealth / (float)_healthMAX;
            float currentHealthPct = (float)_currentHealth / (float)CalculateModifier(eStat.HPMAX);

            //Debug.Log($"currentHealthPct={currentHealthPct} from : { (float)CurrentHealth} / {(float)_healthMAX} ");

            ///Let anyone subscribed to our delegate know we changed 
           // OnHealthPctChanged(currentHealthPct);
           // _debugHealth = _currentHealth;
            return _currentHealth;
        }
        private int CalculateDefense(int rawDamage)
        {
            float retVal = rawDamage; ///A negative Number 
            float defenseBlock = (retVal / 2) * ((float)GetCurrentDefense() / 100);

            retVal -= defenseBlock;

            //Debug.Log($" incommingDamage={rawDamage} , DEF:{GetCurrentDefense()} Blocked={defenseBlock}  finalDmg={retVal}");
            if (retVal > 0)
                Debug.LogWarning("Somehow Positive damage is going thru??");

            return (int)retVal;
        }
        private int CalculateModifier(eStat stat)
        {
            float retVal = FindBaseStat(stat);
            foreach (var mod in GetListForModifier(stat))
            {

                if (mod.Type == Modifier.eType.FLAT)
                    retVal += mod.Value;
                else if (mod.Type == Modifier.eType.PERCENT)
                    retVal += (retVal * mod.Value);

                //if (false) ///Debugging only 
                //{
                //    if (mod.Type == Modifier.eType.FLAT)
                //        Debug.Log($"Found FLAT Val with: {mod.Value} so retVal={retVal}");
                //    else if (mod.Type == Modifier.eType.PERCENT)
                //        Debug.Log($"Found PERCENT Val with: {mod.Value}  so retVal={retVal}");
                //}
            }

            return (int)retVal;
        }
        private void HandleModifierExpections(Modifier modifier, bool isAdd)
        {
            switch (modifier.Stat)
            {
                case eStat.HPMAX:
                    {
                        if (isAdd)
                        {
                            /// add the difference percentage wise to current health?
                        }
                        else
                        {
                            /// remove the difference percentage wise to current health?
                        }

                        ModifyHealth(0); ///Update the UI 
                        break;
                    }

                case eStat.ATTACK:
                    {
                        break;
                    }
                case eStat.DEFENSE:
                    {
                        break;
                    }
                case eStat.RAWDAMAGE:
                    {
                        break;
                    }
            }
        }
        private float FindBaseStat(eStat stat)
        {

            switch (stat)
            {
                case eStat.HPMAX:
                    {
                        return _healthMAX;
                    }
                case eStat.ATTACK:
                    {
                        return _currentAttack;
                    }
                case eStat.DEFENSE:
                    {
                        return _currentDefense;
                    }
                case eStat.RAWDAMAGE:
                    {
                        return 0;
                    }
            }
            return 0;
        }
        private List<Modifier> GetListForModifier(eStat stat)
        {

            switch (stat)
            {
                case eStat.HPMAX:
                    {
                        return _healthModifiers;
                    }
                case eStat.ATTACK:
                    {
                        return _attackModifiers;
                    }
                case eStat.DEFENSE:
                    {
                        return _defenseModifiers;
                    }
                case eStat.RAWDAMAGE:
                    {
                        return _damageModifiers;
                    }
            }
            return null;
        }
    }




}