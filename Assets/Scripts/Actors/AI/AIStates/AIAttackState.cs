using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIAttackState : AIState
    {
        AIInput _ai;
        System.Action _OnAttack;

        private float _attackDelayMax = 0.5f;
        private float _attackDelay = 0;

        /*************************************************************************************************************/

        public AIAttackState(AIInput ai, System.Action attackCall)
        {
            _ai = ai;
            _OnAttack = attackCall;
        }
        /*************************************************************************************************************/

        public override bool CanExit(eAIStates nextState)
        {
            return true;
        }
        public override void OnDisable(Transform target)
        {

        }
        public override void OnEnable(Transform target)
        {
            _attackDelay = 0;
        }
        /*************************************************************************************************************/

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                DoAttack();
            }
        }

        /*************************************************************************************************************/

        protected virtual bool CheckExitConditions(Transform target)
        {
         
            if (target==null)
            {
                return true;
            }
            ///Obv refactor this
            else if (Vector3.Distance(_ai.transform.position, target.position) > _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.IDLE);
                return true;
            }
            else if(Vector3.Distance(_ai.transform.position, target.position) > _ai.AttackRange)
            {
                _ai.SetState(eAIStates.MOVE);
                return true;
            }
            if(_attackDelay < _attackDelayMax)
            {
                _attackDelay += Time.deltaTime;
                _ai.SetMovement(0);
                return true;
            }

            return false;
        }


        protected virtual void DoAttack()
        {
            _OnAttack();
            _ai.SetMovement(0);
            _attackDelay = 0;
        }
    }
}