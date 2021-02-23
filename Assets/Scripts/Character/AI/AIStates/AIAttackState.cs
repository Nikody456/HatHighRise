using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIAttackState : AIState
    {
        AIInput _ai;
        public AIAttackState(AIInput ai)
        {
            _ai = ai;
        }

        public override bool CanExit(eAIStates nextState)
        {
            return true;
        }
        public override void OnDisable(Transform target)
        {

        }
        public override void OnEnable(Transform target)
        {

        }

        public override void Execute(Transform target)
        {
            if (!CheckExitConditions(target))
            {
                ///TODO
                _ai.SetMovement(0);
            }
        }



        protected virtual bool CheckExitConditions(Transform target)
        {
            if (Vector3.Distance(_ai.transform.position, target.position) > _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.IDLE);
                return true;
            }
            if (Vector3.Distance(_ai.transform.position, target.position) > _ai.AttackRange)
            {
                _ai.SetState(eAIStates.MOVE);
                return true;
            }

            return false;
        }

    }
}