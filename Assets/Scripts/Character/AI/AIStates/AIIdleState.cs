using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AI
{
    public class AIIdleState : AIState
    {
        AIInput _ai;
        
        public AIIdleState(AIInput ai)
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
            _ai.SetMovement(0);
        }

        public override void Execute(Transform target)
        {
            if(!CheckExitConditions(target))
            {
                _ai.SetMovement(0);
            }
        }

        protected virtual bool CheckExitConditions(Transform target)
        {
            //Debug.Log($"Dis= {Vector3.Distance(_ai.transform.position, target.position)}");
            if (Vector3.Distance(_ai.transform.position, target.position) < _ai.DetectionRange)
            {
                _ai.SetState(eAIStates.MOVE);
                return true;

            }

            return false;
        }

    }
}