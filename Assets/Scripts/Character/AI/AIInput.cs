using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(CharMovement))]
    public class AIInput : MonoBehaviour
    {
        [SerializeField] Transform _target;
        [SerializeField] float _detectionRange;
        [SerializeField] float _atkRange;

        private CharMovement _charMovement;

        private AIState _currentState;
        private AIState _idleState;
        private AIState _moveState;
        private AIState _deathState;
        private AIState _attackState;
        private AIState _jumpState;

        bool _applicationIsQuitting = false;
        /***********INIT**************************************************************************************************/
        void Awake()
        {
            _charMovement = GetComponent<CharMovement>();
            CreateStates();
        }

        void CreateStates()
        {
            _idleState = new AIIdleState(this);
            _moveState = new AIMoveState(this);
            _attackState = new AIAttackState(this);
            _currentState = _idleState;

        }
        /*************************************************************************************************************/

        void Update()
        {
            if (!_applicationIsQuitting)
            {
                _currentState.Execute(_target);
            }

        }
        /*************************************************************************************************************/

        public void SetState(AIState.eAIStates state)
        {

            if(_currentState.CanExit(state))
            {
                AIState nextState = GetAIState(state);
                if (nextState!=null)
                {
                    _currentState.OnDisable(_target);
                    _currentState = nextState;
                    _currentState.OnEnable(_target);
                }
            }
        }

        public float DetectionRange => _detectionRange;
        public float AttackRange => _atkRange;
        public void SetMovement(float dir)
        {
            _charMovement.SetInput(dir);
        }
        /*************************************************************************************************************/

        private AIState GetAIState(AIState.eAIStates nextState)
        {
            AIState state = null;
            switch(nextState)
            {
                case AIState.eAIStates.IDLE:
                    {
                        state = _idleState;
                        break;
                    }
                case AIState.eAIStates.MOVE:
                    {
                        state = _moveState;
                        break;
                    }
                case AIState.eAIStates.DEATH:
                    {
                        state = _deathState;
                        break;
                    }
                case AIState.eAIStates.ATTACK:
                    {
                        state = _attackState;
                        break;
                    }
                case AIState.eAIStates.JUMP:
                    {
                        state = _jumpState;
                        break;
                    }
            }
            return state;
        }

        private void OnApplicationQuit()
        {
            _applicationIsQuitting = true;
        }
    }
}