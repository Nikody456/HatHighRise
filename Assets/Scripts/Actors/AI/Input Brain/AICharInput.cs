﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(CharMovement))]
    public class AICharInput : AIInput
    {
        private AIState _jumpState;
        private CharMovement _charMovement;

        /***********INIT**************************************************************************************************/
        protected override void Awake()
        {
            _charMovement = this.GetComponent< CharMovement>();
            base.Awake();
        }

        protected override void CreateStates()
        {
            _idleState = new AIIdleState(this);
            _moveState = new AIMoveState(this);
            _attackState = new AIAttackState(this, _charMovement.TryMeleeAttack);
            _currentState = _idleState;
            ///Todo make jump state

        }
        /*************************************************************************************************************/

        protected override AIState GetAIState(AIState.eAIStates nextState)
        {
            AIState state = null;
            switch (nextState)
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

        /*************************************************************************************************************/

        /*************************************************************************************************************/


    }
}