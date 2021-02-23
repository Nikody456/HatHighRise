using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIMoveState : AIState
    {
        AIInput _ai;
        public AIMoveState(AIInput ai)
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
                _ai.SetMovement(PickADirection(target.position));
            }
        }



        protected virtual bool CheckExitConditions(Transform target)
        {
            if (Vector3.Distance(_ai.transform.position, target.position) > _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.IDLE);
                return true;
            }
            if (Vector3.Distance(_ai.transform.position, target.position) <= _ai.AttackRange)
            {
                _ai.SetState(eAIStates.ATTACK);
                return true;
            }

            return false;
        }

        protected virtual float PickADirection(Vector3 pos)
        {
            ///MOVE TOWARDS
            var dir = (_ai.transform.position.x > pos.x);

            return dir ? -1 : 1 ;

        }
    }
}