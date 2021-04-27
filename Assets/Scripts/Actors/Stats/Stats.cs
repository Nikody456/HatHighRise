using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Statistics
{
    public enum eStat { HPMAX, MOVESPEED, SPRINT, JUMP, ATTACK, DEFENSE, RAWDAMAGE, JUMPLIMIT }


    public class Stats : MonoBehaviour
    {

        ///TMP way to inject stats:
        [SerializeField] BaseStats _baseStats = default;

        #region Modifiers
        private Transform _modiferChild;
        private List<Modifier>[] _modifiers;
        #endregion

        #region MaxStats

        private static readonly int MAXHP = 100;
        private static readonly int MAXMOVESPEED = 150;
        private static readonly int MAXSPRINT  = 150;
        private static readonly int MAXJUMP = 150;
        private static readonly int MAXATTACK  = 120;
        private static readonly int MAXDEFENSE = 120;
        private static readonly int MAXRAWDAMAGE  = 999;
        private static readonly int MAXJUMPLIMIT = 5;

        private static readonly int[] _maxStats = new int[]
            {
                MAXHP,
                MAXMOVESPEED,
                MAXSPRINT,
                MAXJUMP,
                MAXATTACK,
                MAXDEFENSE,
                MAXRAWDAMAGE,
                MAXJUMPLIMIT
            };

        #endregion

        #region Properties
        private int _currentHealth; ///Need to apply some modifiers properly here or _healthMAX
        private float _baseMoveSpeed;
        private float _baseSprintSpeed;
        private float _baseJump;  ///James wants a push to hold jump mechanic 
        private int _baseAttack;
        private int _baseDefense;
        private int _jumpLimit;
        #endregion


        #region Dynamic Meters
        ///WHY R U HERE? TODO
        private int _healthMAX = 100;

        #endregion

        #region Getters
        public int CurrentHealth => _currentHealth; //(int) GetCurrentStat(eStat.HPMAX);
        public float CurrentMoveSpeed => GetCurrentStat(eStat.MOVESPEED);
        public float CurrentSprintSpeed => GetCurrentStat(eStat.SPRINT);
        public float CurrentJumpSpeed => GetCurrentStat(eStat.JUMP);
        public int CurrentAttack => (int)GetCurrentStat(eStat.ATTACK);
        public int CurrentDefense => (int)GetCurrentStat(eStat.DEFENSE);

        public int CurrentJumpLimit => (int)GetCurrentStat(eStat.JUMPLIMIT);

        #endregion


        private bool _isPlayer;
        public delegate void PlayerResetHack(GameObject go);
        public event PlayerResetHack OnPlayerResetHack;

        public delegate void HealthChanged(int currHealth);
        public event HealthChanged OnHealthChanged;
        private void Awake()
        {
            ///TEMP HACKY way
            if (_baseStats)
            {
                bool isPlayer = this.GetComponent<PlayerMovement>() != null;
                Initalize(_baseStats, isPlayer);
            }
            
        }

        /*********INIT******************************************************************************************************/

        public void Initalize(BaseStats baseStats, bool isPlayer)
        {
            _isPlayer = isPlayer;
            //Soldier = soldier;
            ///Grab our Initial dynamic meter stats
            _healthMAX = baseStats.HpMax < MAXHP ? baseStats.HpMax : MAXHP;

            ///Grab our initial static stats
            _baseJump = baseStats.Jump < MAXJUMP ? baseStats.Jump : MAXJUMP;
            _baseAttack = baseStats.Attack < MAXATTACK ? baseStats.Attack : MAXATTACK;
            _baseDefense = baseStats.Defense < MAXDEFENSE ? baseStats.Defense : MAXDEFENSE;
            _baseMoveSpeed = (int)baseStats.MovementSpeed; ///Todo do we need a max?
            _baseSprintSpeed = baseStats.Sprint; ///Todo do we need a max?
            _currentHealth = 1; /// Player starts off with 1 hitpoint, more hats will increase hp
            _jumpLimit = baseStats.JumpLimit < MAXJUMP ? baseStats.JumpLimit : MAXJUMP;
            
            /// Set up our modifier array for each enum
            var modifierSize=System.Enum.GetValues(typeof(eStat)).Length;
            _modifiers = new List<Modifier>[modifierSize];
            
            for (int i = 0; i < modifierSize; i++)
            {
                _modifiers[i] = new List<Modifier>();
            }

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

        public void PrintMods()
        {

        }

        public void ImDeadHack()
        {
            if (_isPlayer)
            {
                StartCoroutine(PlayerReset());
            }
            else
            {
                StartCoroutine(DeathDelay());
            }
        }
        public void IsHitDelay()
        {
            if(_isPlayer)
            {
                this.GetComponent<PlayerInput>().SetIsInteracting(true);
            }
            else
            {

            }
        }

        public IEnumerator PlayerReset()
        {
            yield return new WaitForSecondsRealtime(1f);
            OnPlayerResetHack?.Invoke(this.gameObject);
            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                IncreaseHealthHack(1);
            }
        }

        IEnumerator DeathDelay()
        {
            yield return new WaitForSecondsRealtime(0.5f);
            Destroy(this.gameObject);
        }

        public void IncreaseHealthHack(int amount)
        {
            _currentHealth += amount;
            OnHealthChanged?.Invoke(_currentHealth);
        }

        public void DecreaseHealthHack(int amount)
        {
            _currentHealth -= amount;
            OnHealthChanged?.Invoke(_currentHealth);
            ModifyHealth(0);
        }

        public int TakeDamage(int incommingDamage)
        {
            int rawDamage = CalculateDefense(incommingDamage);

            //Debug.Log($"incommingDamage]{incommingDamage}, vs raw= {rawDamage}");
            return ModifyHealth(rawDamage);
        }


        public void AddModifier(Modifier modifier)
        {
            GetListForModifier(modifier.Stat).Add(modifier);
            ///xfer the modifier to us and reset
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



        private float GetCurrentStat(eStat stat)
        {
            float retVal = CalculateModifier(stat);
            float max = GetStatLimit(stat);
            return retVal < max ? retVal : max;
        }

        private int ModifyHealth(int rawDamage)
        {
            _currentHealth -= rawDamage;

            if (_currentHealth < 0)
                _currentHealth = 0;
            else if (_currentHealth > _healthMAX)
                _currentHealth = _healthMAX;

            //float currentHealthPct = (float)_currentHealth / (float)_healthMAX;
            float currentHealthPct = (float)_currentHealth / (float)CalculateModifier(eStat.HPMAX);

            //Debug.Log($"currentHealthPct={currentHealthPct} from : { (float)CurrentHealth} / {(float)_healthMAX} ");

            ///Let anyone subscribed to our delegate know we changed 
            OnHealthChanged?.Invoke(_currentHealth);
            IsHitDelay();
           // _debugHealth = _currentHealth;
            return _currentHealth;
        }
        private int CalculateDefense(int rawDamage)
        {
            float retVal = rawDamage; ///A negative Number 
            //float defenseBlock = (retVal / 2) * ((float)CurrentDefense / 100);

            //retVal -= defenseBlock;

            ////Debug.Log($" incommingDamage={rawDamage} , DEF:{GetCurrentDefense()} Blocked={defenseBlock}  finalDmg={retVal}");
            //if (retVal > 0)
            //    Debug.LogWarning("Somehow Positive damage is going thru??");

            return (int)retVal;
        }
        private float CalculateModifier(eStat stat)
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
                        return _baseAttack;
                    }
                case eStat.DEFENSE:
                    {
                        return _baseDefense;
                    }
                case eStat.RAWDAMAGE:
                    {
                        return 0; ///TODO?
                    }
                case eStat.MOVESPEED:
                    {
                        return _baseMoveSpeed;
                    }
                case eStat.SPRINT:
                    {
                        return _baseSprintSpeed;
                    }
                case eStat.JUMP:
                    {
                        return _baseJump;
                    }
                case eStat.JUMPLIMIT:
                    {
                        return _jumpLimit;
                    }
            }
            return 0;
        }
        private int GetStatLimit(eStat stat)
        {
            return _maxStats[(int)stat];
        }
        private List<Modifier> GetListForModifier(eStat stat)
        {
            return _modifiers[(int)stat];
        }
    }




}