using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(CharMovement))]
    public class AICharInput : AIInput
    {
        [SerializeField] DetectionFilter _contactFilterGround = default;
        [SerializeField] LayerMask _groundLayer = default;
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
            _idleState = new AIIdleState(this, _contactFilterGround.DetectionInfo);
            _moveState = new AIMoveState(this, _groundLayer, _contactFilterGround.DetectionInfo);
            _attackState = new AIAttackState(this, _charMovement.TryMeleeAttack);
            _jumpState = new AIJumpState(this, _charMovement.TryJump);
            _currentState = _idleState;
        }
        /*************************************************************************************************************/

        protected override AIState GetAIState(AIState.eAIStates nextState)
        {
            AIState state = null;
            ///Could put enums into an array 
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

        public void AttemptDelayedDoubleJump()
        {
            StartCoroutine(TryDoubleJump());
        }
        /*************************************************************************************************************/

        IEnumerator TryDoubleJump()
        {
            ///Not sure why this doesnt prock sometimes but kinda have to spam it w 2 inputs
            ///depsite the inital call setting this, so it shud be three attempts..
            _charMovement.TryJump();
            yield return new WaitForSeconds(0.25f);
            _charMovement.TryJump();
        }
    }
}